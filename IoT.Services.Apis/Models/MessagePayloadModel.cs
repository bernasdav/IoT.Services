using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Services.Apis.Models
{
    public class MessagePayloadModel
    {
        public int PayloadType { get; set; }
        public string PayloadText { get; set; }
    }
}
