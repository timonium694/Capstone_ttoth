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
			var query = (from user in ParseUser.Query
						where user.ObjectId == id
						select user);
			IEnumerable<ParseUser> ids = await query.FindAsync();
			ParseUser last = ids.FirstOrDefault();
			this.u = new User() { Email=last.Get<string>("email"), Id = last.ObjectId, Password = last.Get<string>("password"), Username = last.Get<string>("username") };
			return this.u;
		}
		public async void CreateUser(User u) 
		{
			try
			{
				//Get the highest valid number
				var query = from id in ParseObject.GetQuery("Id")
							where id.Get<int>("number") > 1
							select id;
				IEnumerable<ParseObject> ids = await query.FindAsync();
				ParseObject last = ids.Last();
				int tempId = last.Get<int>("number");
				//Finish getting the number

				//Sign up user
				var user = new ParseUser() { Username = u.Username, Email = u.Email, Password = u.Password };
				user["number"] = tempId;
				await user.SignUpAsync();


				//save next id
				tempId++;
				ParseObject temp = new ParseObject("Id");
				temp["number"] = tempId;
				await temp.SaveAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
