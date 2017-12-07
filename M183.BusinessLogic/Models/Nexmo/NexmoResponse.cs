using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.BusinessLogic.Models.Nexmo
{
    public class NexmoResponse
    {
        public string Messagecount { get; set; }
        public List<NexmoMessageStatus> Messages { get; set; }
    }
}
