using System;
using System.Collections.Generic;
using System.Linq;

using Microservices.Channels.Configuration;
using Microservices.Configuration;

using DAO = Microservices.Bus.Data.DAO;

namespace Microservices.Bus.Channels
{
	public static class ChannelInfoExtensions
	{
		public static void SetDescription(this ChannelInfo channelInfo, MicroserviceDescription description)
		{
			#region Validate parameters
			if (channelInfo == null)
				throw new ArgumentNullException(nameof(channelInfo));
 
			if (description == null)
				throw new ArgumentNullException(nameof(description));
			#endregion

			//if (this.Description != null)
			//	throw new InvalidOperationException("Описание канала уже задано.");

			//this.Description = description;
			//this.Provider = description.Provider;

			IDictionary<string, ChannelProperty> properties = channelInfo.Properties;
			List<string> existProps = properties.Values.Select(p => p.Name).Where(prop => description.Properties.Values.Select(p => p.Name).Contains(prop)).ToList();
			List<string> delProps = properties.Values.Select(p => p.Name).Where(prop => !description.Properties.Values.Select(p => p.Name).Contains(prop)).ToList();
			List<string> newProps = description.Properties.Values.Select(p => p.Name).Where(prop => !properties.Values.Select(p => p.Name).Contains(prop)).ToList();

			existProps.ForEach(p =>
			{
				MicroserviceDescriptionProperty dp = description.GetProperty(p);
				ChannelProperty prop = channelInfo.GetProperty(p);
				string value = prop.Value;
				dp.CopyTo(prop);
				prop.Value = value;
			});
			delProps.ForEach(p => channelInfo.RemoveProperty(p));
			newProps.ForEach(p =>
			{
				MicroserviceDescriptionProperty dp = description.GetProperty(p);
				var prop = new ChannelProperty();
				dp.CopyTo(prop);
				channelInfo.AddNewProperty(prop);
			});
		}

		public static ChannelProperty GetProperty(this ChannelInfo channelInfo, string propName)
		{
			return channelInfo.Properties[propName];
		}

		public static void RemoveProperty(this ChannelInfo channelInfo, string propName)
		{
			channelInfo.Properties.Remove(propName);
		}

		public static void AddNewProperty(this ChannelInfo channelInfo, ChannelProperty prop)
		{
			#region Validate parameters
			if (prop == null)
				throw new ArgumentNullException(nameof(prop));

			if (String.IsNullOrEmpty(prop.Name))
				throw new ArgumentException("Отсутствует имя свойства.", nameof(prop.Name));

			if (prop.LINK != 0)
				throw new ArgumentException("Свойство должно иметь LINK = 0.", nameof(prop.LINK));
			#endregion

			if (channelInfo.Properties.ContainsKey(prop.Name))
				throw new InvalidOperationException($"Канал уже содержит свойство {prop.Name}.");

			prop.ChannelLINK = channelInfo.LINK;
			channelInfo.Properties.Add(prop.Name, prop);
		}

		public static ChannelSettings ChannelSettings(this ChannelInfo channelInfo)
		{
			return new ChannelSettings(channelInfo.Properties.ToAppSettings());
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
			dao.Timeout = (obj.Timeout.TotalSeconds == 0 ? new Nullable<int>() : (int)obj.Timeout.TotalSeconds);
			dao.VirtAddress = obj.VirtAddress;

			return dao;
		}

		public static bool IsSystem(this ChannelInfo channelInfo)
		{
			return channelInfo.Provider == "SYSTEM";
		}

		public static void SetRealAddress(this ChannelInfo channelInfo, string realAddress)
		{
			channelInfo.Properties[".RealAddress"].Value = realAddress;
		}
	}
}
