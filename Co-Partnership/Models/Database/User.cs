using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class User
    {
        public User()
        {
            Address = new HashSet<Address>();
            MessageReceiver = new HashSet<Message>();
            MessageSender = new HashSet<Message>();
            TransactionOwner = new HashSet<Transaction>();
            TransactionRecipient = new HashSet<Transaction>();
            WishList = new HashSet<WishList>();
        }

        public int Id { get; set; }
        public string ExtId { get; set; }
        public int? UserType { get; set; }
        public bool? IsActive { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public PersonalFinancialAccount PersonalFinancialAccount { get; set; }
        public ICollection<Address> Address { get; set; }
        public ICollection<Message> MessageReceiver { get; set; }
        public ICollection<Message> MessageSender { get; set; }
        public ICollection<Transaction> TransactionOwner { get; set; }
        public ICollection<Transaction> TransactionRecipient { get; set; }
        public ICollection<WishList> WishList { get; set; }
    }
}
