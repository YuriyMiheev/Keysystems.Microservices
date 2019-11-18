using System;
using System.Net;
using System.Text;
using System.Xml;
using System.Collections.Generic;

namespace Microservices.Channels
{
	/// <summary>
	/// Парсер.
	/// </summary>
	public static class Parser
	{
		/// <summary>
		/// Разобрать адрес в формате "host:port".
		/// </summary>
		/// <param name="value">Адрес.</param>
		/// <param name="host">Хост.</param>
		/// <param name="port">Порт.</param>
		public static void ParseServerAddress(string value, ref string host, ref int port)
		{
			if ( value != null )
			{
				string[] parts = value.Split(':');
				if ( parts.Length == 1 )
				{
					host = parts[0];
				}
				else if ( parts.Length > 1 )
				{
					host = parts[0];
					Int32.TryParse(parts[1], out port);
				}
			}
		}

		/// <summary>
		/// Разобрать логин в формате "user@domain" или "domain\user".
		/// </summary>
		/// <param name="value">Логин.</param>
		/// <param name="user">Имя пользователя.</param>
		/// <param name="domain">Пароль.</param>
		public static void ParseServerLogin(string value, ref string user, ref string domain)
		{
			if ( !String.IsNullOrEmpty(value) )
			{
				if ( value.Contains("@") )
				{
					string[] userAndDomain = value.Split('@');
					if ( userAndDomain.Length == 1 )
					{
						user = value;
					}
					else if ( userAndDomain.Length > 1 )
					{
						user = userAndDomain[0];
						domain = userAndDomain[1];
					}
				}
				else if ( value.Contains("\\") )
				{
					string[] domainAndUser = value.Split('\\');
					if ( domainAndUser.Length == 1 )
					{
						user = value;
					}
					else if ( domainAndUser.Length > 1 )
					{
						domain = domainAndUser[0];
						user = domainAndUser[1];
					}
				}
				else
				{
					user = value;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string ParseString(string value, string defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
				return defaultValue;
			else
				return value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Encoding ParseEncoding(string value, Encoding defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
			{
				return defaultValue;
			}
			else
			{
				try
				{
					return Encoding.GetEncoding(value);
				}
				catch
				{
					int codepage;
					if ( Int32.TryParse(value, out codepage) )
					{
						try
						{
							return Encoding.GetEncoding(codepage);
						}
						catch
						{
							return defaultValue;
						}
					}
					else
					{
						return defaultValue;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">true: [TRUE | ИСТИНА | YES | ДА | 1 | ON | ВКЛ]</param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool ParseBool(string value, bool defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
			{
				return defaultValue;
			}
			else
			{
				value = value.ToUpper();
				if ( value == "TRUE" || value == "ИСТИНА" || value == "YES" || value == "ДА" || value == "1" || value == "ON" || value == "ВКЛ" )
					return true;
				else
					return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static byte ParseByte(string value, byte defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
			{
				return defaultValue;
			}
			else
			{
				byte result;
				if ( Byte.TryParse(value, out result) )
					return result;
				else
					return defaultValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int ParseInt(string value, int defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
			{
				return defaultValue;
			}
			else
			{
				int result;
				if ( Int32.TryParse(value, out result) )
					return result;
				else
					return defaultValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long ParseLong(string value, long defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
			{
				return defaultValue;
			}
			else
			{
				long result;
				if ( Int64.TryParse(value, out result) )
					return result;
				else
					return defaultValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float ParseFloat(string value, float defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
			{
				return defaultValue;
			}
			else
			{
				float result;
				if ( Single.TryParse(value, out result) )
					return result;
				else
					return defaultValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double ParseDouble(string value, double defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
			{
				return defaultValue;
			}
			else
			{
				double result;
				if ( Double.TryParse(value, out result) )
					return result;
				else
					return defaultValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static TimeSpan? ParseTime(string value, TimeSpan? defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
			{
				return defaultValue;
			}
			else
			{
				TimeSpan result;
				if ( TimeSpan.TryParse(value, out result) )
					return result;
				else
					return defaultValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime? ParseDate(string value, DateTime? defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
			{
				return defaultValue;
			}
			else
			{
				DateTime result;
				if ( DateTime.TryParse(value, out result) )
					return result;
				else
					return defaultValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="kind"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Uri ParseUri(string value, UriKind kind, Uri defaultValue)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
			{
				return defaultValue;
			}
			else
			{
				Uri result;
				if ( Uri.TryCreate(value, kind, out result) )
					return result;
				else
					return defaultValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="separators"></param>
		/// <param name="defaultValues"></param>
		/// <returns></returns>
		public static string[] ParseStrings(string value, char[] separators, string[] defaultValues)
		{
			value = (value ?? "").Trim();

			if ( String.IsNullOrEmpty(value) )
				return defaultValues;
			else
				return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
