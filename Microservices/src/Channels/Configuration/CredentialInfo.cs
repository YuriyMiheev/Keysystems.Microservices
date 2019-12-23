using System;
using System.Data.Common;
using System.Linq;

namespace Microservices.Configuration
{
	/// <summary>
	/// Удостоверение безопасности.
	/// </summary>
	[Serializable]
	public class CredentialInfo
	{

		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		public CredentialInfo()
		{ }
		#endregion


		#region Properties
		/// <summary>
		/// {Get,Set} Имя пользователя.
		/// </summary>
		public string UserName { get; set; }

		///// <summary>
		///// {Get,Set} Роль пользователя.
		///// </summary>
		//public string Role { get; set; }

		/// <summary>
		/// {Get,Set} Пароль.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// {Get,Set}  Хэш пароля.
		/// </summary>
		public string PasswordHash { get; set; }
		#endregion


		#region Methods
		/// <summary>
		/// "UserName=;Role=;Password=;PasswordHash="
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var builder = new DbConnectionStringBuilder();
			builder.Add("UserName", this.UserName ?? "");
			//builder.Add("Role", this.Role ?? "");
			builder.Add("Password", this.Password ?? "");
			builder.Add("PasswordHash", this.PasswordHash ?? "");
			return builder.ConnectionString;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public static string GetPasswordHash(string password)
		{
			#region Validate parameters
			if ( password == null )
				throw new ArgumentNullException("password");
			#endregion

			return password.GetHashCode().ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="configString"></param>
		/// <returns></returns>
		public static CredentialInfo Parse(string configString)
		{
			#region Validate parameters
			if ( configString == null )
				throw new ArgumentNullException("configString");
			#endregion

			var result = new CredentialInfo();

			var builder = new DbConnectionStringBuilder();
			builder.ConnectionString = configString;

			if ( builder.ContainsKey("UserName") )
				result.UserName = (string)builder["UserName"];

			//if ( builder.ContainsKey("Role") )
			//   result.Role = (string)builder["Role"];

			if ( builder.ContainsKey("Password") )
				result.Password = (string)builder["Password"];

			if ( builder.ContainsKey("PasswordHash") )
				result.PasswordHash = (string)builder["PasswordHash"];

			return result;
		}
		#endregion

	}
}
