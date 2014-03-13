using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using TheMainEvent_Capstone.Model;

namespace TheMainEvent_Capstone.DataAccessLayer
{
	public class NotificationDAL
	{
		public async Task CreateNotification(Notification note)
		{
			ParseObject p = new ParseObject("Notification");
			p["user"] = note.User;
			p["message"] = note.User;
			
			await p.SaveAsync();
		}
		public async Task<List<Notification>> GetNotifications(string userId)
		{
			List<Notification> nots = new List<Notification>();
			var query = await (from post in ParseObject.GetQuery("Notification")
							   where post.Get<string>("user").Equals(userId)
							   select post).FindAsync();

			IEnumerable<ParseObject> ids = query.ToList();
			foreach (ParseObject p in ids)
			{
				if (p != null)
				{

					Notification output = new Notification();
					output.User = p.Get<string>("user");
					output.Message = p.Get<string>("message");
					output.ID = p.ObjectId;
					output.Date = p.Get<DateTime>("date");
					nots.Add(output);

				}
			}
			return nots;
		}
		public async Task DeleteNotification(string noteId)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("Notification");
			ParseObject p = await query.GetAsync(noteId);
			await p.DeleteAsync();

		}
	}
}
