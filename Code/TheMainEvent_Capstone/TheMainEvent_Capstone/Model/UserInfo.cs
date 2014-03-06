using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone.Model
{
	public class UserInfo
	{
		public string Phone { get; set; }
		public string Bio { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Active { get; set; }
		public string User { get; set; }
		public DateTime Birthday { get; set; }
		public string MerchantEmail { get; set; }
		public string Email { get; set; }
		public string FilterMode { get; set; }

		public override bool Equals(Object obj)
		{
			return ((UserInfo)obj).User.Equals(this.User);
		}
	}
}
