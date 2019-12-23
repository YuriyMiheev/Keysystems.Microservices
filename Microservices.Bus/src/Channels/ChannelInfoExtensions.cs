using System;
using System.Collections.Generic;
using System.Linq;

using Microservices.Bus.Addins;
using Microservices.Channels.Configuration;
using Microservices.Configuration;

using DAO = Microservices.Bus.Data.DAO;

namespace Microservices.Bus.Channels
{
	public static class ChannelInfoExtensions
	{
		public static void SetDescription(this ChannelInfo channelInfo, MicroserviceDescription description)
		{
			if (channelInfo == null)
				throw new ArgumentNullException(nameof(channelInfo));
 
			if (description == null)
				throw new ArgumentNullException(nameof(description));

			channelInfo.Provider = description.Provider;

			IDictionary<string, ChannelInfoProperty> properties = channelInfo.Properties;
			List<string> existProps = properties.Values.Select(p => p.Name).Where(prop => description.Properties.Values.Select(p => p.Name).Contains(prop)).ToList();
			List<string> delProps = properties.Values.Select(p => p.Name).Where(prop => !description.Properties.Values.Select(p => p.Name).Contains(prop)).ToList();
			List<string> newProps = description.Properties.Values.Select(p => p.Name).Where(prop => !properties.Values.Select(p => p.Name).Contains(prop)).ToList();

			existProps.ForEach(p =>
			{
				MicroserviceDescriptionProperty dp = description.GetProperty(p);
				ChannelInfoProperty prop = channelInfo.GetProperty(p);
				string value = prop.Value;
				dp.CopyTo(prop);
				prop.Value = value;
			});
			delProps.ForEach(p => channelInfo.RemoveProperty(p));
			newProps.ForEach(p =>
			{
				MicroserviceDescriptionProperty dp = description.GetProperty(p);
				var prop = new ChannelInfoProperty();
				dp.CopyTo(prop);
				channelInfo.AddNewProperty(prop);
			});
		}

		public static ChannelInfoProperty GetProperty(this ChannelInfo channelInfo, string propName)
		{
			if (channelInfo == null)
				throw new ArgumentNullException(nameof(channelInfo));
			
			return channelInfo.Properties[propName];
		}

		public static void RemoveProperty(this ChannelInfo channelInfo, string propName)
		{
			if (channelInfo == null)
				throw new ArgumentNullException(nameof(channelInfo));
			
			channelInfo.Properties.Remove(propName);
		}

		public static void AddNewProperty(this ChannelInfo channelInfo, ChannelInfoProperty prop)
		{
			if (channelInfo == null)
				throw new ArgumentNullException(nameof(channelInfo));

			if (prop == null)
				throw new ArgumentNullException(nameof(prop));

			if (String.IsNullOrEmpty(prop.Name))
				throw new ArgumentException("Отсутствует имя свойства.", nameof(prop.Name));

			if (prop.LINK != 0)
				throw new ArgumentException("Свойство должно иметь LINK = 0.", nameof(prop.LINK));

			prop.ChannelLINK = channelInfo.LINK;
			channelInfo.Properties.Add(prop.Name, prop);
		}

		public static ChannelSettings ChannelSettings(this ChannelInfo channelInfo)
		{
			var appConfigSettings = new Dictionary<string, AppConfigSetting>();
			foreach (ChannelInfoProperty channelProp in channelInfo.Properties.Values)
			{
				AppConfigSetting appConfigSetting = channelProp.ToAppConfigSetting();
				appConfigSettings.Add(appConfigSetting.Name, appConfigSetting);
			}

			return new ChannelSettings(appConfigSettings);
		}

		public static DAO.ChannelInfo ToDao(this ChannelInfo obj)
		{
			if (obj == null)
				return null;

			var dao = new DAO.ChannelInfo();
			//dao.AccessMode = (obj.AccessMode == AccessMode.FULL ? null : obj.AccessMode);
			dao.Comment = (String.IsNullOrEmpty(obj.Comment) ? null : obj.Comment);
			dao.Enabled = (obj.Enabled == false ? new Nullable<bool>() : obj.Enabled);
			//dao.IsolatedDomain = (obj.IsolatedDomain == false ? new Nullable<bool>() : obj.IsolatedDomain);
			dao.LINK = obj.LINK;
			dao.Name = (String.IsNullOrEmpty(obj.Name) ? null : obj.Name);
			dao.PasswordIn = (String.IsNullOrEmpty(obj.PasswordIn) ? null : obj.PasswordIn);
			dao.PasswordOut = (String.IsNullOrEmpty(obj.PasswordOut) ? null : obj.PasswordOut);
			dao.Properties = obj.Properties.Values.Select(prop => prop.ToDao(dao)).ToList();
			dao.Provider = obj.Provider;
			dao.RealAddress = obj.RealAddress;
			dao.SID = (String.IsNullOrEmpty(obj.SID) ? null : obj.SID);
			dao.Timeout = obj.Timeout;
			dao.VirtAddress = obj.VirtAddress;

			return dao;
		}

		public static ChannelInfo ToObj(this DAO.ChannelInfo dao)
		{
			if (dao == null)
				return null;

			var obj = new ChannelInfo();
			dao.CloneTo(obj);

			return obj;
		}

		public static void CloneTo(this DAO.ChannelInfo dao, ChannelInfo obj)
		{
			#region Validate parameters
			if (dao == null)
				throw new ArgumentNullException("dao");

			if (obj == null)
				throw new ArgumentNullException("obj");
			#endregion

			//obj.AccessMode = dao.AccessMode;
			obj.Comment = dao.Comment;
			obj.Enabled = (dao.Enabled == null ? false : dao.Enabled.Value);
			obj.LINK = dao.LINK;
			obj.Name = dao.Name;
			obj.PasswordIn = dao.PasswordIn;
			obj.PasswordOut = dao.PasswordOut;
			obj.Properties.Clear();
			foreach (DAO.ChannelInfoProperty prop in dao.Properties)
			{
				obj.Properties.Add(prop.Name, prop.ToObj());
			}
			//obj.Properties = dao.Properties.Select(prop => prop.ToObj()).ToArray();
			obj.Provider = dao.Provider;
			obj.RealAddress = dao.RealAddress;
			obj.SID = dao.SID;
			obj.Timeout = (dao.Timeout == null ? 0 : dao.Timeout.Value);
			obj.VirtAddress = dao.VirtAddress;
		}

		public static bool IsSystem(this ChannelInfo channelInfo)
		{
			return channelInfo.Provider == "SYSTEM";
		}

		public static ChannelInfoProperty FindProperty(this ChannelInfo channelInfo, string propName)
		{
			if (channelInfo.Properties.ContainsKey(propName))
				return channelInfo.Properties[propName];
			else
				return null;
		}
	}
}
