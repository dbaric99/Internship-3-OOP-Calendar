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
		public List<string> Participants { get; private set; }

		public Event()
		{
			Id = Guid.NewGuid();
		}

		public Event(string eventName, string location, DateTime startDate, DateTime endDate, List<string> participants)
		{
			Id = Guid.NewGuid();
			EventName = eventName;
			Location = location;
			StartDate = startDate;
			EndDate = endDate;
			Participants = participants;
		}
	}
}

