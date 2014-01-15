using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using TheMainEvent_Capstone.Model;

namespace TheMainEvent_Capstone.DataAccessLayer
{
	public class EventDAL
	{
		public async Task<Event> RetrieveEvent(string objectId)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("Event");
			ParseObject p = await query.GetAsync(objectId);
			Event returnEvent = new Event()
		   {
			   Title = p.Get<string>("title"),
			   Address = p.Get<string>("address"),
			   Date = p.Get<DateTime>("date"),
			   Description = p.Get<string>("description"),
			   OtherDetails = p.Get<string>("otherDetails"),
			   ID = p.ObjectId
		   };
			return returnEvent;
		}
		public async void CreateEvent(Event e)
		{
			ParseObject temp = new ParseObject("Event");
			temp["title"] = e.Title;
			temp["address"] = e.Title;
			temp["description"] = e.Description;
			temp["otherDetails"] = e.OtherDetails;
			temp["date"] = e.Date;
			await temp.SaveAsync();
		}
		public async void SetOwner(string eventId, string ownerId)
		{
			ParseObject po = new ParseObject("EventOwner");
			po["ownerId"] = ownerId;
			po["eventId"] = eventId;
			await po.SaveAsync();
		}
		public async void AddAttendee(string eventId, string userId)
		{
			ParseObject po = new ParseObject("EventAttendee");
			po["userId"] = userId;
			po["eventId"] = eventId;



			var query = (from accept in ParseObject.GetQuery("UserInvites")
						 where accept.Get<string>("userId").Equals(userId)
						 where accept.Get<string>("eventId").Equals(eventId)
						 select accept);
			IEnumerable<ParseObject> events = await query.FindAsync();
			ParseObject invite = events.FirstOrDefault();
			if (invite != null)
			{
				invite["isAccepted"] = "true";
				await invite.SaveAsync();
			}
			await po.SaveAsync();
		}
		public async void UpdateEvent(string id, Event e)
		{

			ParseQuery<ParseObject> query = ParseObject.GetQuery("Event");
			ParseObject temp = await query.GetAsync(id);
			temp["title"] = e.Title;
			temp["address"] = e.Title;
			temp["description"] = e.Description;
			temp["otherDetails"] = e.OtherDetails;
			temp["date"] = e.Date;
			await temp.SaveAsync();

		}
		public async Task<List<User>> GetAttendees(string eventId)
		{
			var query = (from attendee in ParseObject.GetQuery("EventAttendee")
						 where attendee.Get<string>("userId") == eventId
						 select attendee);
			IEnumerable<ParseObject> ids = await query.FindAsync();
			List<User> users = new List<User>();
			foreach (ParseObject p in ids)
			{
				users.Add(new User()
				{
					Email = p.Get<string>("email"),
					Id = p.ObjectId,
					Password = p.Get<string>("password"),
					Username = p.Get<string>("username")
				});
			}

			return users;
		}
		private async Task<User> GetOwner(string eventId)
		{
			var query = (from owner in ParseObject.GetQuery("EventOwner")
						 where owner.Get<string>("eventId") == eventId
						 select owner);
			ParseObject last = await query.FirstAsync();
			UserDAL ud = new UserDAL();
			User u = await ud.RetrieveUser(last.Get<string>("ownerId"));
			return u;
		}
		public async void InviteUser(string userId, string eventId)
		{
			ParseObject p = new ParseObject("UserInvites");
			p["userId"] = userId;
			p["eventId"] = eventId;
			p["isAccepted"] = "false";
			await p.SaveAsync();
		}
		public async Task<List<Event>> GetInvitesForUser(string userId)
		{
			var query = (from invite in ParseObject.GetQuery("UserInvites")
						 where invite.Get<string>("userId").Equals(userId)
						 where invite.Get<string>("isAccepted").Equals("false")
						 select invite);
			IEnumerable<ParseObject> events = await query.FindAsync();
			List<Event> evs = new List<Event>();
			foreach (ParseObject o in events)
			{
				string id = o.Get<string>("eventId");
				Event e = await this.RetrieveEvent(id);
				evs.Add(e);
			}
			return evs;
		}
		public async Task<List<User>> GetInvitees(string eventId)
		{
			var query = (from invite in ParseObject.GetQuery("UserInvites")
						 where invite.Get<string>("eventId").Equals(eventId)
						 where invite.Get<string>("isAccepted").Equals("false")
						 select invite);
			IEnumerable<ParseObject> invites = await query.FindAsync();
			List<User> users = new List<User>();
			foreach (ParseObject o in invites)
			{
				UserDAL ud = new UserDAL();
				User u = await ud.RetrieveUser(o.Get<string>("userId"));
				users.Add(u);
			}
			return users;
		}
		public async void RejectInvitation(string userId, string eventId)
		{
			var query = (from accept in ParseObject.GetQuery("UserInvites")
						 where accept.Get<string>("userId").Equals(userId)
						 where accept.Get<string>("eventId").Equals(eventId)
						 select accept);
			IEnumerable<ParseObject> events = await query.FindAsync();
			ParseObject invite = events.FirstOrDefault();
			await invite.DeleteAsync();
		}

	}
}
