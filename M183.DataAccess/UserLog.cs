using System;

namespace M183.DataAccess
{
    public class UserLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Class { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public virtual User User { get; set; }
    }
}
