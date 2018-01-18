using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class Phone
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? PhoneNumber { get; set; }

        public User User { get; set; }
    }
}
