using System;

namespace Evo2HomeMqttNet
{
    public class StoreException : Exception
    {
        public StoreException(string message, Exception e) : base(message, e)
        {
  
        }
    }
}