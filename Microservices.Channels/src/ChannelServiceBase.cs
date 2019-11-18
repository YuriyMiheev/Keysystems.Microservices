using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microservices.Common.Configuration;

namespace Microservices.Common
{
	public abstract class ChannelServiceBase : IChannelServiceBase
	{
		//private MsgboxSettings msgboxSettings;
		private IXmlConfigFileSettings _settings;


		protected ChannelServiceBase(IXmlConfigFileSettings settings)
		{
			_settings = settings ?? throw new ArgumentNullException(nameof(settings));
			//this.msgboxSettings = new MsgboxSettings(this..Info.Properties);
			//this.Database = new MsgboxDatabase(this.msgboxSettings.Provider, this.msgboxSettings.Schema);
		}


		///// <summary>
		///// {Get,Set}
		///// </summary>
		//protected virtual MessageDatabaseBase Database { get; set; }

		///// <summary>
		///// {Get} Адаптер БД сообщений.
		///// </summary>
		//public virtual MessageDataAdapter MessageDataAdapter { get; private set; }


		public abstract void Start();


		public abstract void Stop();

	}
}
