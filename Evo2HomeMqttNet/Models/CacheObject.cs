using System;

namespace Evo2HomeMqttNet.Models
{
    public abstract class CacheObject
    {
        public abstract TimeSpan Expiry {get;}

        public DateTime SavedOn { get; set; }
    }
}