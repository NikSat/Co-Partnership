using Co_Partnership.Models.Database;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Co_Partnership.Models
{
    public class MemberAccountViewModel
    {
        public List<Transaction> Transactions { get; set; }
        public string Username { get; set; }
        public decimal Balance { get; set; }
        public int CurrentUserId { get; set; }
        

    }
}
