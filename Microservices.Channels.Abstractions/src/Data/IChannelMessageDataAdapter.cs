using System.Collections.Generic;

using Microservices.Data;

namespace Microservices.Channels.Data
{
	/// <summary>
	/// Адаптер хранилища сообщений канала.
	/// </summary>
	public interface IChannelMessageDataAdapter : IMessageDataAdapter
	{

		#region Contacts
		List<Contact> GetContacts();

		void SaveContact(Contact contact);

		void DeleteContact(Contact contact);
		#endregion


		#region StoredProcedures

		void CallPingSP(string spName);

		void CallRepairSP(string spName);

		void CallMessageStatusChangedSP(string spName, Message msg);

		int? CallReceiveMessageSP(string spName, Message msg, bool useOutputParam);
		#endregion

	}
}
