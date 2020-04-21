using System;
namespace RSC.Remoting
{
    [Serializable]
    public abstract class AbstractRemoteResponse
    {
        private object responseData = null;
        /// <summary>
        /// Additional data that may be transported along with the response.
        /// </summary>
        public object Data
        {
            get
            {
                return responseData;
            }
            set
            {
                responseData = value;
            }
        }
    }
}