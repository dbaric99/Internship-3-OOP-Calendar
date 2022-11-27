using System;
using Calendar.Classes;

namespace Calendar
{
    class Program
    {
        static void Main(string[] args)
        {
            var people = new List<Person>()
            {
                new Person("Trisha", "Bunker", "trisha.bunker@gmail.com"),
                new Person("Sabrina", "Gibb", "sabrina.gibb@gmail.com"),
                new Person("Adrian", "Faulkner", "adrian.faulkner@gmail.com"),
                new Person("Meredith", "Sherburne", "meredith.sherburne@gmail.com"),
                new Person("Edith", "Hermanson", "edith.hermanson@gmail.com"),
                new Person("Harmon", "Dukes", "harmon.dukes@gmail.com"),
                new Person("Palmer", "Garland", "palmer.garland@gmail.com"),
                new Person("Davina", "Carver", "davina.carver@gmail.com"),
                new Person("Levi", "Sharp", "levi.sharp@gmail.com"),
                new Person("Ingram", "Morin", "ingram.morin@gmail.com"),
            };

            //year month day hour minute second
            var events = new List<Event>()
            {
                new Event("Lecture 1", "FESB - A101",new DateTime(2021,10,05,13,15,00), new DateTime(2021,10,05,16,00,00)),
                new Event("Lecture 2", "FESB - A320", new DateTime(2022,01,21,16,15,00), new DateTime(2022,01,21,19,00,00)),
                new Event("Lecture 3", "FESB - C501", new DateTime(2022,05,25,17,15,00), new DateTime(2022,05,25,20,00,00)),
                new Event("Lecture 4", "FESB - B525", new DateTime(2022,11,26,9,00,00), new DateTime(2022,11,28,10,30,00)),
                new Event("Lecture 5", "FESB - B320", new DateTime(2023,03,16,14,15,00), new DateTime(2023,03,16,16,00,00)),
            };

            var menuChoice = 0;
            do
            {
                menuChoice = MainMenu();

                switch (menuChoice)
                {
                    case 1:
                        {
                            ActiveEventsSubmenu(events, people);
                            break;
                        }
                    case 2:
                        {
                            UpcomingEventsSubmenu(events, people);
                            break;
                        }
                    case 3:
                        {
                            break;
                        }
                    case 4:
                        {
                            break;
                        }
                    case 0:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("There is no option for current input!\n");
                            break;
                        }
                }

            } while (menuChoice != 0);

            Console.ReadKey();

            int MainMenu()
            {
                var isValid = false;
                var choice = 0;

                do
                {
                    Console.WriteLine("\n<<<---------- MAIN MENU ---------->>>");
                    Console.WriteLine(
                    "\n1 - Active events" +
                    "\n2 - Upcoming events" +
                    "\n3 - Past events" +
                    "\n4 - Add new event" +
                    "\n0 - Exit\n");
                    Console.Write("\nEnter your choice: ");

                    isValid = int.TryParse(Console.ReadLine(), out choice);

                } while (!isValid);

                return choice;
            }

            void GetAllActiveEvents(List<Event> events)
            {
                if(events.Count == 0)
                {
                    Console.WriteLine("\nThere are no events!\n");
                    return;
                }
                Console.WriteLine("\n<<<---------- ACTIVE EVENTS ---------->>>\n");
                foreach (var ev in events)
                {
                    if(DateTime.Now >= ev.StartDate && DateTime.Now < ev.EndDate)
                    {
                        Console.WriteLine($"Id: {ev.Id}\n"
                            + $"Event: {ev.EventName} - Location: {ev.Location} - Ends in: {ev.EndDate - DateTime.Now}\n"
                            + $"Participants: {(ev.Participants.Count != 0 ? string.Join(", ", ev.Participants) : "None")}\n");
                    }
                }
            }

            void ActiveEventsSubmenu(List<Event> events, List<Person> people)
            {
                Guid eventId;
                Event targetEvent;

                GetAllActiveEvents(events);

                Console.Write("\nNote someone's absence(y/n): ");
                string menuChoice = Console.ReadLine().ToLower();

                if (menuChoice != "y") return;

                Console.Write("\nInsert event id: ");
                bool success = Guid.TryParse(Console.ReadLine(), out eventId);

                if (!success)
                {
                    Console.WriteLine("Invalid input for event id!\n");
                    return;
                }
                else if(!events.Any(e => e.Id == eventId))
                {
                    Console.WriteLine("There is no event with certain id!\n");
                    return;
                }

                targetEvent = events.Find(e => e.Id == eventId);

                Console.Write("\nInsert emails of people you want note as absent with space in between: ");
                string[] emails = Console.ReadLine().Split(" ");

                foreach (var email in emails)
                {
                    if (people.Any(p => p.Email == email))
                    {
                        Person targetPerson = people.Find(p => p.Email == email);
                        targetPerson.SetAttendance(targetEvent.Id, false);
                    }
                    else
                    {
                        Console.WriteLine($"There is no person with the following email: {email}!\n");
                    }
                }
                targetEvent.RemoveParticipant(emails);
            }

            void GetAllUpcomingEvents(List<Event> events)
            {
                if (events.Count == 0)
                {
                    Console.WriteLine("\nThere are no events!\n");
                    return;
                }
                Console.WriteLine("\n<<<---------- UPCOMING EVENTS ---------->>>\n");
                foreach (var ev in events)
                {
                    if(ev.StartDate > DateTime.Now)
                    {
                        Console.WriteLine($"Id: {ev.Id}\n"
                            + $"Event: {ev.EventName} - Location: {ev.Location} - Starts in: {ev.StartDate.Subtract(DateTime.Now).Hours}\n"
                            + $"Participants: {(ev.Participants.Count != 0 ? string.Join(", ", ev.Participants) : "None")}\n");
                    }
                }
            }

            void UpcomingEventsSubmenu(List<Event> events, List<Person> people)
            {
                GetAllUpcomingEvents(events);
            }
        }
    }
}
