using System;
namespace Calendar.Classes
{
	public class Person
	{
		public string Name { get; set; }
		public string Surname { get; }
		public string Email { get; }
		public Dictionary<Guid, bool> Attendance { get; private set; } = new Dictionary<Guid, bool>();

		public Person(string surname, string email)
		{
			Surname = surname;
			Email = email;
		}

		public Person(string name, string surname, string email)
		{
			Name = name;
			Surname = surname;
			Email = email;
        }

		public void SetAttendance(Guid eventId, bool isAttending)
		{
			if (Attendance.ContainsKey(eventId))
			{
				Attendance[eventId] = isAttending;
			}
			Attendance.Add(eventId, isAttending);
		}

		public void RemoveAttendance(Guid eventId)
		{
			if (Attendance.ContainsKey(eventId))
			{
				Attendance.Remove(eventId);
				return;
			}
			Console.WriteLine("There is no event to be removed by the chosen key!\n");
		}
	}
}

