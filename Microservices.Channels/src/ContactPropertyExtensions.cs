using System;

using AutoMapper;

using DAO = Microservices.Data.DAO;


namespace Microservices.Channels
{
	public static class ContactPropertyExtensions
	{
		private readonly static IMapper mapper;

		static ContactPropertyExtensions()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<ContactProperty, ContactProperty>();
			});
			mapper = config.CreateMapper();
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="contact"></param>
		/// <returns></returns>
		public static DAO.ContactProperty ToDao(this ContactProperty obj, DAO.Contact contact)
		{
			if (obj == null)
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
			if (dao == null)
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
	}
}
