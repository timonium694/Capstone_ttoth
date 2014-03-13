using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone.Model
{
	public class Notification
	{
		public string Message { get; set; }
		public string User { get; set; }
		public string ID { get; set; }
		public DateTime Date { get; set; }


		public override bool Equals(object obj)
		{
			return ((Notification)obj).ID == this.ID;
		}
	}
}
