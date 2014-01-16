using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone.Model.ViewModels
{
	class EventViewModel
	{
		public string ID { get; set; }
		public string Address { get; set; }
		public string Title { get; set; }
		public DateTime Date { get; set; }
		public string OtherDetails { get; set; }
		public string Description { get; set; }
		public User Owner { get; set; }
		public List<User> Attendees { get; set; }
		public List<User> Invitees { get; set; }
	}
}
