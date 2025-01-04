using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Text;
using XSystem.Security.Cryptography;

namespace WebApplication1.Models
{
	public class Cls_User
	{
		Baglanti context = new Baglanti();


		public bool loginControl(string Email)
		{
			User? usr = context.Users.FirstOrDefault(u => u.Email == Email);

			if (usr == null)
			{
				return false;
			}
			return true;

		}
		public static string loginControl(User user)
		{

			using (Baglanti context = new Baglanti())
			{
				user.Password = MD5Sifrele(user.Password);
				User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

				if (usr == null)
				{
					return "error";
				}
				else
				{
					if (usr.IsAdmin == true)
					{
						//admin
						return usr.NameSurname;
					}
					else
					{
						//normal kullanıcı
						return usr.Email;
					}
				}
			}
		}

		public static bool AddUser(User user)
		{
			Baglanti context = new Baglanti();
			try
			{
				user.Active = true;
				user.IsAdmin = false;
				user.Password = MD5Sifrele(user.Password);
				context.Users.Add(user);
				context.SaveChanges();
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}
		public static string MD5Sifrele(string value)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] btr = Encoding.UTF8.GetBytes(value);
			btr = md5.ComputeHash(btr);

			StringBuilder sb = new StringBuilder();
			foreach (byte item in btr)
			{
				sb.Append(item.ToString("x2").ToLower());
			}
			return sb.ToString();
		}
		public static User? SelectMemberInfo(string Email)
		{
			Baglanti context = new Baglanti();
			return context.Users.FirstOrDefault(u => u.Email == Email);
		}

		public User? UserDetails(int? id)
		{
			User? user = context.Users.Find(id);
			return user;
		}

		public static bool UserUpdate(User user)
		{
			try
			{
				using (var context = new Baglanti())
				{
					var existingUser = context.Users.FirstOrDefault(u => u.UserID == user.UserID);
					if (existingUser == null)
					{
						return false;
					}
					if (user.Password == null)
					{
						user.Password = existingUser.Password;
					}
					else
					{
						user.Password = MD5Sifrele(user.Password);
					}
					context.Entry(existingUser).CurrentValues.SetValues(user);
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{

				return false;
			}
		}

		public static bool ContactUsMessage(User user)
		{
			using (var context = new Baglanti())
			{
				// Find the existing user by UserID
				var existingUser = context.Users.FirstOrDefault(u => u.UserID == user.UserID);

				// Check if the user exists
				if (existingUser != null)
				{
					// Update the user's Message property
					existingUser.Message = user.Message;

					// Save the changes to the database
					context.SaveChanges();
					return true;
				}

				// If user does not exist, return false
				return false;
			}
		}


	}
}
