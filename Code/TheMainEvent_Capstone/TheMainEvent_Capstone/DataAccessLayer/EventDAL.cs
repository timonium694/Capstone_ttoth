using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using TheMainEvent_Capstone.Model;
using System.Globalization;
using TheMainEvent_Capstone.Model.ViewModels;

namespace TheMainEvent_Capstone.DataAccessLayer
{
	public class EventDAL
	{
		/// <summary>
		/// Input the eventId to receive the event code.
		/// </summary>
		/// <param name="objectId"></param>
		/// <returns></returns>
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
			   ID = p.ObjectId,
			   City = p.Get<string>("city"),
			   State = p.Get<string>("state"),
			   Cost = p.Get<double>("cost"),
			   IsDonatable = p.Get<bool>("isDonatable"),
			   Type = p.Get<string>("type")
			};
			return returnEvent;
		}

		/// <summary>
		/// Input the event in order to add it to the Parse DB.
		/// </summary>
		/// <param name="e"></param>
		public async Task CreateEvent(Event e)
		{
			ParseObject temp = new ParseObject("Event");
			temp["title"] = e.Title;
			temp["address"] = e.Address;
			temp["description"] = e.Description;
			temp["otherDetails"] = e.OtherDetails;
			temp["date"] = e.Date;
			temp["city"] = e.City;
			temp["state"] = e.State;
			temp["type"] = e.Type;
			temp["cost"] = e.Cost;
			temp["isDonatable"] = e.IsDonatable;
			await temp.SaveAsync();
		}

		public async Task SetOwner(string eventId, string ownerId)
		{
			ParseObject po = new ParseObject("EventOwner");
			po["owner"] = ownerId;
			po["event"] = eventId;
			await po.SaveAsync();
			ParseObject p = new ParseObject("EventAttendee");
			po["user"] = ownerId;
			po["event"] = eventId;
			await p.SaveAsync();
		}
		public async Task AddAttendee(string eventId, string userId)
		{
			ParseObject po = new ParseObject("EventAttendee");
			po["user"] = userId;
			po["event"] = eventId;



			var query = (from accept in ParseObject.GetQuery("UserInvites")
						 where accept.Get<string>("user").Equals(userId)
						 where accept.Get<string>("event").Equals(eventId)
						 select accept);
			IEnumerable<ParseObject> events = await query.FindAsync();
			ParseObject invite = events.FirstOrDefault();
			if (invite != null)
			{
				await invite.DeleteAsync();
			}
			await po.SaveAsync();
		}
		public async Task UpdateEvent(string id, Event e)
		{

			ParseQuery<ParseObject> query = ParseObject.GetQuery("Event");
			ParseObject temp = await query.GetAsync(id);
			temp["title"] = e.Title;
			temp["address"] = e.Title;
			temp["description"] = e.Description;
			temp["otherDetails"] = e.OtherDetails;
			temp["date"] = e.Date;
			temp["city"] = e.City;
			temp["state"] = e.State;
			temp["type"] = e.Type;
			await temp.SaveAsync();

		}
		public async Task<List<string>> GetAttendees(string eventId)
		{
			var query = (from attendee in ParseObject.GetQuery("EventAttendee")
						 where attendee.Get<string>("event") == eventId
						 select attendee);
			IEnumerable<ParseObject> ids = await query.FindAsync();
			List<string> users = new List<string>();
			foreach (ParseObject p in ids)
			{
				users.Add(p.Get<string>("user"));
			}

			return users;
		}
		public async Task<string> GetOwner(string eventId)
		{
			var query = (from owner in ParseObject.GetQuery("EventOwner")
						 where owner.Get<string>("event") == eventId
						 select owner);
			ParseObject last = await query.FirstAsync();
			string output = last.Get<string>("owner");
			return output;
		}
		public async Task InviteUser(string userId, string eventId, string inviterId)
		{
			ParseObject p = new ParseObject("UserInvites");
			p["user"] = userId;
			p["event"] = eventId;
			p["inviter"] = inviterId;
			await p.SaveAsync();
		}
		public async Task<List<string>> GetInvitesForUser(string userId)
		{
			var query = (from invite in ParseObject.GetQuery("UserInvites")
						 where invite.Get<string>("user").Equals(userId)
						 select invite);
			IEnumerable<ParseObject> events = await query.FindAsync();
			List<string> invites = new List<string>();
			foreach (ParseObject o in events)
			{
				invites.Add(o.Get<string>("event"));
			}
			return invites;
		}
		public async Task<List<string>> GetInvitees(string eventId)
		{
			var query = (from invite in ParseObject.GetQuery("UserInvites")
						 where invite.Get<string>("event").Equals(eventId)
						 select invite);
			IEnumerable<ParseObject> invites = await query.FindAsync();	
			List<string> users = new List<string>();
			foreach (ParseObject o in invites)
			{
				users.Add(o.Get<string>("user"));
			}
			return users;
		}
		public async Task RejectInvitation(string userId, string eventId)
		{
			var query = (from accept in ParseObject.GetQuery("UserInvites")
						 where accept.Get<string>("user").Equals(userId)
						 where accept.Get<string>("event").Equals(eventId)
						 select accept);
			IEnumerable<ParseObject> events = await query.FindAsync();
			ParseObject invite = events.FirstOrDefault();
			await invite.DeleteAsync();
		}
		public async Task<List<Event>> GetUserEvents(string userId)
		{
			List<Event> evs = new List<Event>();
			try
			{
				var query = await (from accept in ParseObject.GetQuery("EventAttendee")
							 where accept.Get<string>("user").Equals(userId)
							 select accept).FindAsync();
				IEnumerable<ParseObject> events = query.ToList();
				foreach (ParseObject p in events)
				{
					Event e = await this.RetrieveEvent(p.Get<string>("event"));
					evs.Add(e);
				}
				var query1 = (from accept in ParseObject.GetQuery("EventOwner")
							 where accept.Get<string>("owner").Equals(userId)
							 select accept);
				IEnumerable<ParseObject> moreEvents = await query1.FindAsync();
				foreach (ParseObject p in moreEvents)
				{
					Event e = await this.RetrieveEvent(p.Get<string>("event"));
					if(!evs.Contains(e)) 
						evs.Add(e);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return evs;
		}
		public async Task<List<Event>> BasicEventSearch(string searchTerm)
		{
			List<Event> output = new List<Event>();
			var query = (from accept in ParseObject.GetQuery("Event")
						 where (accept.Get<string>("title").Contains(searchTerm) || accept.Get<string>("description").Contains(searchTerm)) &&
						 accept.Get<string>("type")=="Public"
						select accept);
			IEnumerable<ParseObject> events = await query.FindAsync();
			foreach (ParseObject p in events)
			{
				Event returnEvent = new Event()
				{
					Title = p.Get<string>("title"),
					Address = p.Get<string>("address"),
					Date = p.Get<DateTime>("date"),
					Description = p.Get<string>("description"),
					OtherDetails = p.Get<string>("otherDetails"),
					ID = p.ObjectId,
					City = p.Get<string>("city"),
					State = p.Get<string>("state"),
					Type = p.Get<string>("type"),
					Cost = p.Get<double>("cost")
				};
				output.Add(returnEvent);
			}
			return output;
		}
		public List<EventViewModel> BasicFilter(List<EventViewModel> events, int sort, string option = "")
		{
			List<EventViewModel> sorted = new List<EventViewModel>();
			switch (sort)
			{
				//title
				case 0:
					sorted = events.OrderBy(x => x.Title).ToList();
					break;

				//date
				case 1:
					sorted = events.OrderBy(x => x.Date).ToList();
					break;

				case 2:
					sorted = events.Where(x => x.Type == option).OrderBy(x => x.Title).ToList();
					break;
			}
			return sorted;
		}
		public async Task UnattendEvent(string eventId, string userId)
		{
			var query1 = (from attendee in ParseObject.GetQuery("eventAttendee")
						  where attendee.Get<string>("event") == eventId &&
						  attendee.Get<string>("user") == userId
						  select attendee);
			IEnumerable<ParseObject> attendees = await query1.FindAsync();
			ParseObject p = attendees.FirstOrDefault();
			if (p != null)
				await p.DeleteAsync();
		}
		public async Task DeleteEvent(string eventId)
		{
			var query = (from invite in ParseObject.GetQuery("UserInvites")
						 where invite.Get<string>("event").Equals(eventId)
						 where invite.Get<string>("isAccepted").Equals("false")
						 select invite);
			IEnumerable<ParseObject> invites = await query.FindAsync();
			foreach (ParseObject p in invites)
			{
				await p.DeleteAsync();
			}
			var query1 = (from attendee in ParseObject.GetQuery("eventAttendee")
						 where attendee.Get<string>("event") == eventId
						 select attendee);
			IEnumerable<ParseObject> attendees = await query1.FindAsync();
			foreach (ParseObject p in attendees)
			{
				await p.DeleteAsync();
			}
			var query2 = (from owner in ParseObject.GetQuery("EventOwner")
						 where owner.Get<string>("event") == eventId
						 select owner);
			ParseObject p1 = await query2.FirstAsync();
			await p1.DeleteAsync();
		}



		public async Task<string> NewestEventFromUser(string owner)
		{
			var query = await (from ev in ParseObject.GetQuery("Event")
						 where ev.Get<string>("otherDetails") != "1"
						select ev).FindAsync();
			IEnumerable<ParseObject> events = query.ToList().OrderBy(x => x.CreatedAt).	ToList();
			ParseObject e = events.Last();
			string id = e.ObjectId;
			return id;
		}
	}
}