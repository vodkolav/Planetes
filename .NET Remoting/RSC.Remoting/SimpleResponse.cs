using System;
namespace RSC.Remoting
{
    public enum SimpleResponseType
    {
        Int,
        Bool,
        String,
        KeyValuePair
    }

    [Serializable]
    public class SimpleResponse : AbstractRemoteResponse
    {
        private SimpleResponseType type;
        public SimpleResponseType ResponseType
        {
            get
            {
                return type;
            }
        }

        public SimpleResponse(string message)
        {
            Data = message;
            type = SimpleResponseType.String;
        }

        public SimpleResponse(bool value)
        {
            Data = value;
            type = SimpleResponseType.Bool;
        }

        public SimpleResponse(int value)
        {
            Data = value;
            type = SimpleResponseType.Int;
        }

        public override string ToString()
        {
            if (Data != null)
                return Data.ToString();
            return base.ToString();
        }
    }
}