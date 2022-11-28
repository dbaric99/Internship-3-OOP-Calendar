using System;
namespace Calendar.Classes
{
	public class Event
	{
		public Guid Id { get; }
		public string EventName { get; set; }
		public string Location { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public List<string> Participants { get; private set; } = new List<string>();

		public Event()
		{
			Id = Guid.NewGuid();
		}

		public Event(string eventName, string location, DateTime startDate, DateTime endDate)
		{
			Id = Guid.NewGuid();
			EventName = eventName;
			Location = location;
			StartDate = startDate;
			EndDate = endDate;
        }

		public void AddParticipant(string[] participants)
		{
			Participants.AddRange(participants.Where(p2 =>
				Participants.All(p1 => p1 != p2)));
		}

		public void RemoveParticipant(string[] participants)
		{
			foreach (var p in participants)
			{
				if (Participants.Contains(p))
				{
					Participants.Remove(p);
				}
				else
				{
					Console.WriteLine($"Person with email {p} does not exist in the participants list!\n");
				}
			}
		}
	}
}

