using System;
using System.Linq;

using AutoMapper;

using DAO = Microservices.Data.DAO;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	public static class MessageExtensions
	{
		private readonly static IMapper mapper;

		/// <summary>
		/// Type initializer.
		/// </summary>
		static MessageExtensions()
		{
			var config = new MapperConfiguration(cfg =>
				{
					#region Contact
					cfg.CreateMap<Contact, Contact>();
					cfg.CreateMap<ContactProperty, ContactProperty>();
					#endregion

					#region Message
					cfg.CreateMap<Message, Message>();
					#endregion
				});
			mapper = config.CreateMapper();
		}

	}
}
