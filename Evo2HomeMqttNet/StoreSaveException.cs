using System;

namespace Evo2HomeMqttNet
{
    public class StoreSaveException : Exception
    {
        public StoreSaveException(string message, Exception exception) : base(message, exception)
        {
  
        }
    }
}