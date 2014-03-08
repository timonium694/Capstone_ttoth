using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
		public async Task<UserInfo> GetUserInfo(string id)
		{

			UserInfo output = new UserInfo();
			try
			{
				var query = await (from post in ParseObject.GetQuery("UserInfo")
								   where post.Get<string>("user") == id
								   select post).FindAsync();

				IEnumerable<ParseObject> ids = query.ToList();
				ParseObject p = ids.FirstOrDefault();
				if (p != null)
				{
					output.FirstName = p.Get<string>("firstName");
					output.LastName = p.Get<string>("lastName");
					output.Phone = p.Get<string>("phone");
					output.Bio = p.Get<string>("bio");
					output.Birthday = p.Get<DateTime>("birthday");
					output.User = p.Get<string>("user");
					output.MerchantEmail = p.Get<string>("merchant");
					output.Email = p.Get<string>("mail");
					output.FilterMode = p.Get<string>("filter");
					var picFile = p.Get<ParseFile>("picFile");
					output.ProfilePic = await new HttpClient().GetByteArrayAsync(picFile.Url);
				}
			}
			catch (Exception e)
			{
			}
			return output;
		}
		private bool CompareParseUsers(ParseUser user, ParseUser contact)
		{
			return user.Username.Equals(contact.Username);
		}
		public async void CreateUserInfo(UserInfo ui)
		{
			ParseObject info = new ParseObject("UserInfo");
			info["firstName"] = ui.FirstName;
			info["lastName"] = ui.LastName;
			info["phone"] = ui.Phone;
			info["bio"] = ui.Bio;
			info["birthday"] = ui.Birthday;
			info["user"] = ui.User;
			info["merchant"] = "none";
			info["mail"] = ui.Email;
			info["filter"] = "A-Z";
			ParseFile picFile = new ParseFile("default.jpg", ui.ProfilePic);
			await picFile.SaveAsync();
			info["picFile"] = picFile;
			await info.SaveAsync();
		}
		public async Task CreateUser(User u)
		{
			try
			{
				//Sign up user
				var user = new ParseUser() { Username = u.Username, Email = u.Email, Password = u.Password, };
				await user.SignUpAsync();
			}
			catch (Exception ex)
			{
			}

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
						where contact.Get<string>("user").Equals(userId)
						select contact;
			IEnumerable<ParseObject> cons = await query.FindAsync();
			List<string> contacts = new List<string>();
			foreach (ParseObject p in cons)
			{
				contacts.Add(p.Get<string>("contact"));
			}
			return contacts;
		}
		public async Task UpdateUserInfo(UserInfo ui)
		{

			var query = await (from post in ParseObject.GetQuery("UserInfo")
							   where post.Get<string>("user") == ui.User
							   select post).FindAsync();

			IEnumerable<ParseObject> ids = query.ToList();
			ParseObject info = ids.FirstOrDefault();
			info["firstName"] = ui.FirstName;
			info["lastName"] = ui.LastName;
			info["phone"] = ui.Phone;
			info["bio"] = ui.Bio;
			info["birthday"] = ui.Birthday;
			info["user"] = ui.User;
			info["merchant"] = "none";
			ParseFile picFile = new ParseFile("default.jpg", ui.ProfilePic);
			await picFile.SaveAsync();
			info["picFile"] = picFile;
			await info.SaveAsync();
		}
		public byte[] ConvertToBytes(BitmapImage bitmapImage)
		{
			
				using (MemoryStream ms = new MemoryStream())
				{

					var wBitmap = new WriteableBitmap(bitmapImage);
					wBitmap.SaveJpeg(ms, wBitmap.PixelWidth, wBitmap.PixelHeight, 0, 100);
					ms.Seek(0, SeekOrigin.Begin);
					byte[] data = new byte[4096];
					data = ms.GetBuffer();
					return data;
				}
			
		}
		public BitmapImage ToImage(byte[] bytes)
		{
			MemoryStream ms = new MemoryStream(bytes);
			BitmapImage image = new BitmapImage();
			image.SetSource(ms);
			return image;
		}


		public async Task<List<UserInfo>> SearchUsers(string input)
		{
			string[] names = input.Split(' ');
			List<UserInfo> users = new List<UserInfo>();

			if (names.Count() == 2)
			{

				var query = await (from post in ParseObject.GetQuery("UserInfo")
								   where (post.Get<string>("firstName").Contains(names[0]) &&
								   post.Get<string>("lastName").Contains(names[1])) ||
								   (post.Get<string>("firstName").Contains(names[1]) &&
								   post.Get<string>("lastName").Contains(names[0]))
								   select post).FindAsync();

				IEnumerable<ParseObject> ids = query.ToList();
				foreach (ParseObject p in ids)
				{
					UserInfo output = new UserInfo();
					if (p != null)
					{
						output.FirstName = p.Get<string>("firstName");
						output.LastName = p.Get<string>("lastName");
						output.Phone = p.Get<string>("phone");
						output.Bio = p.Get<string>("bio");
						output.Birthday = p.Get<DateTime>("birthday");
						output.User = p.Get<string>("user");
						output.MerchantEmail = p.Get<string>("merchant");
						output.Email = p.Get<string>("mail");
						output.FilterMode = p.Get<string>("filter");
						var picFile = p.Get<ParseFile>("picFile");
						output.ProfilePic = await new HttpClient().GetByteArrayAsync(picFile.Url);
					}
					users.Add(output);
				}
			}


			foreach (string word in names)
			{
				var query1 = await (from post in ParseObject.GetQuery("UserInfo")
									where post.Get<string>("firstName").Contains(word) ||
									post.Get<string>("lastName").Contains(word)
									select post).FindAsync();

				IEnumerable<ParseObject> ids1 = query1.ToList();
				foreach (ParseObject p in ids1)
				{
					UserInfo output = new UserInfo();
					if (p != null)
					{
						output.FirstName = p.Get<string>("firstName");
						output.LastName = p.Get<string>("lastName");
						output.Phone = p.Get<string>("phone");
						output.Bio = p.Get<string>("bio");
						output.Birthday = p.Get<DateTime>("birthday");
						output.User = p.Get<string>("user");
						output.MerchantEmail = p.Get<string>("merchant");
						output.Email = p.Get<string>("mail");
						output.FilterMode = p.Get<string>("filter");
						var picFile = p.Get<ParseFile>("picFile");
						output.ProfilePic = await new HttpClient().GetByteArrayAsync(picFile.Url);
					}
					users.Add(output);
				}

			}


			return users;
		}
	}
}
