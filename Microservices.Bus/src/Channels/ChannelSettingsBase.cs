using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Bus.Channels
{
	/// <summary>
	/// Дополнительные настройки канала.
	/// </summary>
	public abstract class ChannelSettingsBase
	{
		private List<ChannelProperty> _properties;


		#region Ctor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="properties"></param>
		protected ChannelSettingsBase(string prefix, IEnumerable<ChannelProperty> properties)
		{
			#region Validate parameters
			if ( properties == null )
				throw new ArgumentNullException("properties");
			#endregion

			_properties = properties.Where(p => p.Name.StartsWith(prefix)).ToList();
		}
		#endregion


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		protected virtual string PropertyValue(string propName)
		{
			if ( _properties != null )
			{
				ChannelProperty prop = _properties.SingleOrDefault(p => p.Name == propName);
				if ( prop == null )
					return null;

				return prop.Value;
			}

			return null;
		}
		#endregion

	}
}
