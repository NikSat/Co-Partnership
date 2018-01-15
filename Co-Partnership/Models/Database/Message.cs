using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Message
    {
        public int Id { get; set; }
        public int? Sender { get; set; }
        public int? Receiver { get; set; }
        public DateTime? Date { get; set; }
        public bool? Viewed { get; set; }
        public string Message1 { get; set; }

        public User ReceiverNavigation { get; set; }
        public User SenderNavigation { get; set; }
    }
}
