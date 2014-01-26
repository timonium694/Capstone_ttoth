using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMainEvent_Capstone.Model;

namespace TheMainEvent_Capstone.DataAccessLayer
{
	class UserDAL
	{
		private User u;

		public async Task<User> RetrieveUser(string id)
		{
			var query = await (from user in ParseUser.Query
							   where user.Username.Equals(id)
							   select user).FindAsync();
			IEnumerable<ParseUser> ids = query.ToList();
			ParseUser last = ids.FirstOrDefault();
			this.u = new User()
			{
				Email = last.Email,
				Id = last.ObjectId,
				Username = last.Username
			};
			return this.u;
		}
		public async void CreateUserInfo(UserInfo ui)
		{
			ParseObject info = new ParseObject("UserInfo");
			info["firstName"] = ui.FirstName;
			info["lastName"] = ui.LastName;
			info["phone"] = ui.Phone;
			info["bio"] = ui.Bio;
			info["birthday"] = ui.Birthday;
			info["active"] = ui.Active;
			info["user"] = ParseUser.CurrentUser;
			await info.SaveAsync();
		}
		public async void CreateUser(User u)
		{
			//Sign up user
			var user = new ParseUser() { Username = u.Username, Email = u.Email, Password = u.Password, };
			await user.SignUpAsync();
			
		}
		public async void AddContact(string userId, string ContactId)
		{
			ParseObject contact = new ParseObject("Contact");
			contact["user"] = userId;
			contact["contact"] = ContactId;
			await contact.SaveAsync();
		}
		public async Task<List<string>> GetContacts(string userId)
		{
			var query = from contact in ParseObject.GetQuery("Contact")
						where contact.Get<string>("user").Equals("user")
						select contact;
			IEnumerable<ParseObject> cons = await query.FindAsync();
			List<string> contacts = new List<string>();
			foreach (ParseObject p in cons)
			{
				contacts.Add(p.Get<string>("contact"));
			}
			return contacts;
		}
	}
}
