using System.Runtime.Serialization;

namespace Q3Client
{
    [DataContract]
    class UserConfig
    {
        [DataMember]
        public bool FirstRun = true;

        [DataMember]
        public bool PersistentNewQueueNotifications = false;

        [DataMember]
        public bool AggressiveQueueNotifications = false;
    }
}
