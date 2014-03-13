using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMainEvent_Capstone.Model.ViewModels
{
	class InviteViewModel
	{
		public string ID { get; set; }
		public string Address { get; set; }
		public string Title { get; set; }
		public string Type { get; set; }
		public double Cost { get; set; }
		public DateTime Date { get; set; }
		public DateTime Time { get; set; }
		public string Description { get; set; }
		public bool IsDonatable { get; set; }
	}
}
