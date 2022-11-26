using System;
namespace Calendar.Classes
{
	public class Person
	{
		public string Name { get; set; }
		public string Surname { get; }
		public string Email { get; }
		public Dictionary<Guid, bool> Attendance { get; private set; }

		public Person(string surname, string email)
		{
			Surname = surname;
			Email = email;
		}

		public Person(string name, string surname, string email, Dictionary<Guid, bool> attendance)
		{
			Name = name;
			Surname = surname;
			Email = email;
			Attendance = attendance;
		}
	}
}

