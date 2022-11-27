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

            var events = new List<Event>()
            {
                new Event("Lecture 1", "FESB - A101",new DateTime(2021,10,05,13,15,00), new DateTime(2021,10,05,16,00,00)),
                new Event("Lecture 2", "FESB - A320", new DateTime(2022,01,21,16,15,00), new DateTime(2022,01,21,19,00,00)),
                new Event("Lecture 3", "FESB - C501", new DateTime(2022,05,25,17,15,00), new DateTime(2022,05,25,20,00,00)),
                new Event("Lecture 4", "FESB - B525", new DateTime(2022,11,26,9,00,00), new DateTime(2022,11,28,10,30,00)),
                new Event("Lecture 5", "FESB - B320", new DateTime(2023,03,16,14,15,00), new DateTime(2023,03,16,16,00,00)),
            };

            events[0].AddParticipant(new string[2] { "palmer.garland@gmail.com", "ingram.morin@gmail.com" });
            events[1].AddParticipant(new string[1] { "trisha.bunker@gmail.com" });
            events[2].AddParticipant(new string[3] { "harmon.dukes@gmail.com", "levi.sharp@gmail.com", "meredith.sherburne@gmail.com" });
            events[4].AddParticipant(new string[4] { "palmer.garland@gmail.com", "davina.carver@gmail.com", "levi.sharp@gmail.com", "adrian.faulkner@gmail.com" });

            people[6].SetAttendance(events[0].Id, true);
            people[9].SetAttendance(events[0].Id, true);
            people[0].SetAttendance(events[1].Id, true);
            people[5].SetAttendance(events[2].Id, true);
            people[8].SetAttendance(events[2].Id, true);
            people[3].SetAttendance(events[2].Id, true);
            people[6].SetAttendance(events[4].Id, true);
            people[7].SetAttendance(events[4].Id, true);
            people[8].SetAttendance(events[4].Id, true);
            people[2].SetAttendance(events[4].Id, true);

            var menuChoice = 0;
            do
            {
                menuChoice = GetMainMenuOptionFromUser();

                switch (menuChoice)
                {
                    case 1:
                        {
                            ActiveEventsSubmenu(events, people);
                            break;
                        }
                    case 2:
                        {
                            events = UpcomingEventsSubmenu(events, people);
                            break;
                        }
                    case 3:
                        {
                            PastEvents(events, people);
                            break;
                        }
                    case 4:
                        {
                            events = CreateNewEvent(events, people);
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

            int GetMainMenuOptionFromUser()
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

                Console.Write("\nInsert emails of people you want note as absent with commas in between: ");
                string[] emails = Console.ReadLine().Split(",");

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
                int count = 0;
                foreach (var ev in events)
                {
                    if(ev.StartDate > DateTime.Now)
                    {
                        count++;
                        Console.WriteLine($"Id: {ev.Id}\n"
                            + $"Event: {ev.EventName} - Location: {ev.Location} - Starts in: {ev.StartDate.Subtract(DateTime.Now).Hours}\n"
                            + $"Participants: {(ev.Participants.Count != 0 ? string.Join(", ", ev.Participants) : "None")}\n");
                    }
                }
                if(count == 0)
                {
                    Console.WriteLine("No upcoming events!\n");
                }
            }

            int UpcomingEventsMenu()
            {
                int choice = 0;
                bool isValid = false;

                do
                {
                    Console.WriteLine("\n1 - Delete event"
                    + "\n2 - Remove attending persons"
                    + "\n0 - Return to main menu\n");
                    Console.Write("Enter your choice: ");

                    isValid = int.TryParse(Console.ReadLine(), out choice);

                } while (!isValid);

                return choice;
            }

            List<Event>? UpcomingEventsSubmenu(List<Event> events, List<Person> people)
            {
                if(events == null)
                {
                    Console.WriteLine("\nThere are no upcoming events!");
                    return events;
                }

                GetAllUpcomingEvents(events);

                var upcomingEventsChoice = 0;

                do
                {
                    upcomingEventsChoice = UpcomingEventsMenu();

                    switch (upcomingEventsChoice)
                    {
                        case 1:
                            {
                                events = DeleteEvent(events, people);
                                break;
                            }
                        case 2:
                            {
                                RemoveAttendingPeopleFromEvent(events, people);
                                break;
                            }
                        case 0:
                            {
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("There is no option for current input!\n");
                                break;
                            }
                    }

                } while (upcomingEventsChoice != 0);

                return events;
            }

            List<Event> DeleteEvent(List<Event> events, List<Person> people)
            {
                Guid targetedId;

                Console.Write("\nInput id of event you want to delete: ");
                var success = Guid.TryParse(Console.ReadLine(), out targetedId);

                if (!success)
                {
                    Console.WriteLine("Invalid input for event id!\n");
                }
                else
                {                  
                    var eventToRemove = events.Find(ev => ev.Id == targetedId);
                    if(eventToRemove == null)
                    {
                        Console.WriteLine("\nNo event with matching id found!");
                        return events;
                    }
                    if(eventToRemove.StartDate < DateTime.Now)
                    {
                        Console.WriteLine("\nSelected event is not an upcoming event!");
                        return events;
                    }
                    events.Remove(eventToRemove);
                    foreach (var person in people)
                    {
                        if(person.Attendance.ContainsKey(targetedId))
                            person.RemoveAttendance(targetedId);
                    }
                }

                return events;
            }

            void RemoveAttendingPeopleFromEvent(List<Event> events, List<Person> people)
            {
                Guid targetedId;

                Console.Write("\nInput id of the event from which you want to remove attending people: ");
                var success = Guid.TryParse(Console.ReadLine(), out targetedId);

                if (!success)
                {
                    Console.WriteLine("Invalid input for event id!\n");
                }
                var targetEvent = events.Find(ev => ev.Id == targetedId);
                if(targetEvent == null)
                {
                    Console.WriteLine("\nNo event by that id!");
                    return;
                }
                if (!(targetEvent.StartDate < DateTime.Now))
                {
                    Console.WriteLine("\nSelected event is not an upcoming event!");
                    return;
                }

                Console.Write("\nInsert emails of people you want to remove from an event with commas in between: ");
                string[] emails = Console.ReadLine().Split(",");

                targetEvent.RemoveParticipant(emails);

                foreach (var person in people)
                {
                    if (emails.Contains(person.Email) && person.Attendance.ContainsKey(targetedId))
                    {
                        person.RemoveAttendance(targetedId);
                    }
                }
            }

            void PastEvents(List<Event> events, List<Person> people)
            {
                Console.WriteLine("\n<<<---------- FINISHED EVENTS ---------->>>\n");
                int count = 0;
                foreach (var ev in events)
                {
                    if(ev.EndDate < DateTime.Now)
                    {
                        count++;
                        Console.WriteLine($"Id: {ev.Id}\n"
                           + $"Event: {ev.EventName} - Location: {ev.Location} - Ended before: {DateTime.Now.Subtract(ev.EndDate).Days} days - Duration: {ev.EndDate.Subtract(ev.StartDate).Hours} hours");
                        List<string> attendingPeople = new List<string>();
                        List<string> nonAttendingPeople = new List<string>();
                        foreach (var person in people)
                        {
                            if (person.Attendance.ContainsKey(ev.Id))
                            {
                                if (person.Attendance[ev.Id])
                                    attendingPeople.Add(person.Email);
                                else
                                    nonAttendingPeople.Add(person.Email);
                            }
                        }
                        Console.WriteLine($"Attending people: {(attendingPeople.Count != 0 ? string.Join(", ", attendingPeople) : "None")}\n"
                            + $"Non-attending people: {(nonAttendingPeople.Count != 0 ? string.Join(", ",nonAttendingPeople) : "None")}\n\n");
                    }
                }
            }

            List<Event> CreateNewEvent(List<Event> events, List<Person> people)
            {
                DateTime startDate, endDate;
                List<string> participants = new List<string>();

                Console.Write("\nInput event name: ");
                var eventName = Console.ReadLine();

                Console.Write("\nInput event location: ");
                var eventLocation = Console.ReadLine();

                Console.Write("\nInput start date in format yyyy-mm-dd hh:mm : ");
                var startDateString = Console.ReadLine();

                bool startDateParseSuccess = DateTime.TryParse(startDateString, out startDate);

                Console.Write("\nInput end date in format yyyy-mm-dd hh:mm : ");
                var endDateString = Console.ReadLine();

                var endDateParseSuccess = DateTime.TryParse(endDateString, out endDate);

                if(!(startDateParseSuccess && endDateParseSuccess))
                {
                    Console.WriteLine("\nInvalid date input!");
                    return events;
                }
                if(DateTime.Now>startDate || endDate < startDate)
                {
                    Console.WriteLine("\nStart date must be in the future and end date has to be after the start date!");
                    return events;
                }

                Console.Write("\nEnter emails of the participants divided by commas: ");
                var emails = Console.ReadLine().Trim().Split(",");

                foreach (var email in emails)
                {
                    var overlap = CheckForOverlap(events, email, startDate, endDate);
                    if (!overlap)
                    {
                        if(GetPersonByEmail(people,email) != null)
                            Console.WriteLine($"\nPerson with email {email} does not exist!");
                        else
                        {
                            participants.Add(email);
                        }
                    }
                }

                var newEvent = new Event(eventName, eventLocation, startDate, endDate);
                events.Add(newEvent);

                foreach (var participantEmail in participants)
                {
                    var p = GetPersonByEmail(people, participantEmail);
                    p.SetAttendance(newEvent.Id, true);
                }

                return events;
            }

            //<<<---------- HELPER FUNCTIONS ---------->>>

            bool CheckForOverlap(List<Event> events, string email, DateTime startDate, DateTime endDate)
            {
                foreach (var ev in events)
                {
                    if (ev.Participants.Contains(email))
                    {
                        if((ev.StartDate >= startDate && ev.StartDate <= endDate) || (ev.EndDate >= startDate && ev.EndDate <= endDate))
                        {
                            Console.WriteLine($"\nThere is overlap for person {email}, they won't be added to this event!");
                            return true;
                        }
                    }
                }
                return false;
            }

            Person? GetPersonByEmail(List<Person> people, string email)
            {
                return people.Find(p => p.Email == email);
            }
        }
    }
}
