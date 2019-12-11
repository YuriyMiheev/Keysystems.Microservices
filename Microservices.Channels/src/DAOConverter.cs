using System;
using System.Linq;

using AutoMapper;

using DAO = Microservices.Data.DAO;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	public static class DAOConverter
	{
		private static IMapper mapper;

		/// <summary>
		/// Type initializer.
		/// </summary>
		static DAOConverter()
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



		#region Contact
		/// <summary>
		/// Копирование информации в другой объект за исключением LINK и Properties.
		/// </summary>
		/// <param name="src"></param>
		/// <param name="dest"></param>
		public static void CopyTo(this Contact src, Contact dest)
		{
			#region Validate parameters
			if ( src == null )
				throw new ArgumentNullException("src");

			if ( dest == null )
				throw new ArgumentNullException("dest");
			#endregion

			//dest.AccessMode = src.AccessMode;
			dest.Address = src.Address;
			dest.Comment = src.Comment;
			dest.Enabled = src.Enabled;
			dest.IsMyself = src.IsMyself;
			dest.IsRemote = src.IsRemote;
			dest.IsService = src.IsService;
			dest.Name = src.Name;
			dest.Online = src.Online;
			dest.Opened = src.Opened;
			dest.Type = src.Type;
		}

		/// <summary>
		/// Создание копии объекта без свойства LINK и ContactLINK.
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static ContactProperty Copy(this ContactProperty src)
		{
			#region Validate parameters
			if (src == null)
				throw new ArgumentNullException("src");
			#endregion

			var dest = new ContactProperty();
			src.CopyTo(dest);

			return dest;
		}

		/// <summary>
		/// Копирование свойств в другой объект за исключением свойства LINK и ContactLINK.
		/// </summary>
		/// <param name="src"></param>
		/// <param name="dest"></param>
		public static void CopyTo(this ContactProperty src, ContactProperty dest)
		{
			#region Validate parameters
			if (src == null)
				throw new ArgumentNullException("src");

			if (dest == null)
				throw new ArgumentNullException("dest");
			#endregion

			mapper.Map<ContactProperty, ContactProperty>(src, dest);
			dest.LINK = 0;
			dest.ContactLINK = null;
		}

		#region DAO
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static DAO.Contact ToDao(this Contact obj)
		{
			if ( obj == null )
				return null;

			var dao = new DAO.Contact();
			//dao.AccessMode = (String.IsNullOrEmpty(obj.AccessMode) ? null : obj.AccessMode);
			dao.Address = obj.Address;
			dao.Comment = (String.IsNullOrEmpty(obj.Comment) ? null : obj.Comment);
			dao.ContactID = obj.ContactID;
			dao.Enabled = (obj.Enabled == false ? new Nullable<bool>() : obj.Enabled);
			dao.IsMyself = (obj.IsMyself == false ? new Nullable<bool>() : obj.IsMyself);
			dao.IsService = (obj.IsService == false ? new Nullable<bool>() : obj.IsService);
			dao.LINK = obj.LINK;
			dao.Name = (String.IsNullOrEmpty(obj.Name) ? null : obj.Name);
			dao.Online = obj.Online;
			dao.Opened = (obj.Opened == false ? new Nullable<bool>() : obj.Opened);
			dao.Properties = obj.Properties.Select(prop => prop.ToDao(dao)).ToList();
			dao.Type = obj.Type;

			return dao;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		/// <returns></returns>
		public static Contact ToObj(this DAO.Contact dao)
		{
			if ( dao == null )
				return null;

			var obj = new Contact();
			dao.CloneTo(obj);

			return obj;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		/// <param name="obj"></param>
		public static void CloneTo(this DAO.Contact dao, Contact obj)
		{
			#region Validate parameters
			if ( dao == null )
				throw new ArgumentNullException("dao");

			if ( obj == null )
				throw new ArgumentNullException("obj");
			#endregion

			//obj.AccessMode = dao.AccessMode;
			obj.Address = dao.Address;
			obj.Comment = dao.Comment;
			obj.Enabled = (dao.Enabled == null ? false : dao.Enabled.Value);
			obj.IsMyself = (dao.IsMyself == null ? false : dao.IsMyself.Value);
			obj.IsService = (dao.IsService == null ? false : dao.IsService.Value);
			obj.LINK = dao.LINK;
			obj.Name = dao.Name;
			obj.Online = dao.Online;
			obj.Opened = (dao.Opened == null ? false : dao.Opened.Value);
			obj.Properties = dao.Properties.Select(cont => cont.ToObj()).ToArray();
			obj.Type = dao.Type;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="contact"></param>
		/// <returns></returns>
		public static DAO.ContactProperty ToDao(this ContactProperty obj, DAO.Contact contact)
		{
			if ( obj == null )
				return null;

			var dao = new DAO.ContactProperty();
			dao.Comment = (String.IsNullOrEmpty(obj.Comment) ? null : obj.Comment);
			dao.Contact = contact;
			dao.Format = (String.IsNullOrEmpty(obj.Format) ? null : obj.Format);
			dao.LINK = obj.LINK;
			dao.Name = obj.Name;
			dao.Type = (String.IsNullOrEmpty(obj.Type) ? null : obj.Type);
			dao.Value = obj.Value;

			return dao;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		/// <returns></returns>
		public static ContactProperty ToObj(this DAO.ContactProperty dao)
		{
			if ( dao == null )
				return null;

			var obj = new ContactProperty();
			obj.Comment = dao.Comment;
			obj.ContactLINK = dao.Contact.LINK;
			obj.Format = dao.Format;
			obj.LINK = dao.LINK;
			obj.Name = dao.Name;
			obj.Type = dao.Type;
			obj.Value = dao.Value;

			return obj;
		}
		#endregion

		#endregion

	}
}
