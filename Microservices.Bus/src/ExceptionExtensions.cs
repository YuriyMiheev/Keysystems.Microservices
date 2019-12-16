using System;

using AutoMapper;

using VMO = Microservices.Bus.Web.VMO;

namespace Microservices.Bus
{
	public static class ExceptionExtensions
	{
		private static IMapper mapper;

		/// <summary>
		/// Type initializer.
		/// </summary>
		static ExceptionExtensions()
		{
			var config = new MapperConfiguration(cfg =>
				{
					cfg.CreateMap<ExceptionWrapper, VMO.ExceptionWrapper>();
				});
			mapper = config.CreateMapper();
		}


		public static VMO.ExceptionWrapper ToVmo(this ExceptionWrapper vmo)
		{
			return mapper.Map<ExceptionWrapper, VMO.ExceptionWrapper>(vmo);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vmo"></param>
		public static VMO.ExceptionWrapper ToVmo(this Exception obj)
		{
			if (obj == null)
				return null;

			return new ExceptionWrapper(obj).ToVmo();
		}


	}
}
