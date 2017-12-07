using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.BusinessLogic.Models.Nexmo
{
    public class NexmoMessageStatus
    {
        public string MessageId { get; set; }
        public string To { get; set; }
        public string clientRef;
        public string Status { get; set; }
        public string ErrorText { get; set; }
        public string RemainingBalance { get; set; }
        public string MessagePrice { get; set; }
        public string Network;
    }
}
