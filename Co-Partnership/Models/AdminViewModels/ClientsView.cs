using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Models
{
    public class ClientsView
    {
        public List<ClientsViewModel> Clients { get; set; }
        public int UserId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
