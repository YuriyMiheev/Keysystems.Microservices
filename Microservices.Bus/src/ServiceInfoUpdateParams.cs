using System;

namespace Microservices.Bus
{
   /// <summary>
   /// 
   /// </summary>
   [Serializable]
    public class ServiceInfoUpdateParams
    {

        #region Properties
        /// <summary>
        /// {Get,Set}
        /// </summary>
        public bool Online { get; set; }

        /// <summary>
        /// {Get,Set}
        /// </summary>
        public string ExternalAddress { get; set; }

        /// <summary>
        /// {Get,Set}
        /// </summary>
        public string InternalAddress { get; set; }
        #endregion

    }
}
