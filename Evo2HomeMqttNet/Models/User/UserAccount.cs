using System;

namespace Evo2HomeMqttNet.Models.User
{
    public class UserAccount : CacheObject
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public override TimeSpan Expiry { get {return TimeSpan.FromDays(1); }}
    }
}

