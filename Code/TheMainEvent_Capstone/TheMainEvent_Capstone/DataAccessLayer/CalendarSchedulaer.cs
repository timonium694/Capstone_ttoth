using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMainEvent_Capstone.Model;
using TheMainEvent_Capstone.Model.ViewModels;

namespace TheMainEvent_Capstone.DataAccessLayer
{
	public static class CalendarSchedulaer
	{
		public static void ScheduleEvent(Event e)
		{
			SaveAppointmentTask saveAppointmentTask = new SaveAppointmentTask();

			saveAppointmentTask.StartTime = e.Date;
			saveAppointmentTask.EndTime = e.Date.AddHours(1);
			saveAppointmentTask.Subject = e.Title;
			saveAppointmentTask.Location = e.Address + ", " + e.City + ", " + e.State;
			saveAppointmentTask.Details = e.OtherDetails;
			saveAppointmentTask.IsAllDayEvent = false;
			saveAppointmentTask.Reminder = Reminder.OneHour;
			saveAppointmentTask.AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.Busy;

			saveAppointmentTask.Show();
		}
	}
}
