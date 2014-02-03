using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone.Model
{
	public class Event
	{
		public string ID { get; set; }
		public string Address { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string Title { get; set; }
		public DateTime Date { get; set; }
		public string OtherDetails { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public double Cost { get; set; }

	}
}
