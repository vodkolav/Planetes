using System;
namespace RSC.Remoting
{
    [Serializable]
    public abstract class AbstractRemoteCommand
    {
        private object commandData = null;
        /// <summary>
        /// Additional data which can be transported along with the command.
        /// </summary>
        public object Data
        {
            get
            {
                return commandData;
            }
            set
            {
                commandData = value;
            }
        }
    }
}