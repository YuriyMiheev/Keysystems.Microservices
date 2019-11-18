using System;

namespace Microservices.Channels
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Все вложенные сообщения.
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static string AllMessages(this Exception ex)
		{
			if ( ex == null )
				return String.Empty;
			else
				return (ex.Message + Environment.NewLine + AllMessages(ex.InnerException)).Trim('\r', '\n');
		}

		/// <summary>
		/// Найти ошибку указанного типа.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static T Find<T>(this Exception ex) where T : Exception
		{
			if ( ex == null )
				return null;

			if ( ex is T || typeof(T).IsAssignableFrom(ex.GetType()) )
				return (T)ex;

			return Find<T>(ex.InnerException);
		}

		///// <summary>
		///// Обернуть ошибку для передачи (сериализации/десериализации) через границу AppDomain.
		///// </summary>
		///// <param name="ex"></param>
		///// <returns></returns>
		//public static ExceptionWrapper Wrap(this Exception ex)
		//{
		//	return new ExceptionWrapper(ex);
		//}
	}
}
