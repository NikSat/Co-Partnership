using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Message
    {
        public int Id { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public DateTime? DateSent { get; set; }
        public bool? Processed { get; set; }
        public DateTime? DateProcessed { get; set; }
        public string Message1 { get; set; }

        public User Receiver { get; set; }
        public User Sender { get; set; }
    }
}
