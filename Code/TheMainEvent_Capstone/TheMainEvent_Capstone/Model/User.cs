using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone.Model
{
	public class User
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Phone { get; set; }
		public string Bio { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Active { get; set; }
		public DateTime Birthday { get; set; }
	}
}
