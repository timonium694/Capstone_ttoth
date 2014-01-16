using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone.Model
{
	public class UserInvite
	{
		public Event Event { get; set; }
		public string InviterId { get; set; }
		public string InviteeId { get; set; }
		public string IsAccepted { get; set; }
	}
}
