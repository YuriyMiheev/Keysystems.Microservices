using System;

namespace Microservices.Bus.Web.VMO
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

	}
}
