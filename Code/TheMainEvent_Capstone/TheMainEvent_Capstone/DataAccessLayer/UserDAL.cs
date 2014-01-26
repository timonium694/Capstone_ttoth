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
		public async void CreateUser(User u)
		{
			try
			{
				//Sign up user
				var user = new ParseUser() { Username = u.Username, Email = u.Email, Password = u.Password, };
				user["phone"] = u.Phone;
				await user.SignUpAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		public async void AddContact(string userId, string ContactId)
		{
			ParseObject contact = new ParseObject("Contact");
			contact["user"] = userId;
			contact["contact"] = ContactId;
			await contact.SaveAsync();
		}
		public async Task<List<User>> GetContacts(string userId)
		{
			var query = from contact in ParseObject.GetQuery("Contact")
						where contact.Get<string>("user").Equals("user")
						select contact;
			IEnumerable<ParseObject> cons = await query.FindAsync();
			List<User> contacts = new List<User>();
			foreach (ParseObject p in cons)
			{
				User u = await this.RetrieveUser(p.Get<string>("contact"));
				contacts.Add(u);
			}
			return contacts;
		}
	}
}
