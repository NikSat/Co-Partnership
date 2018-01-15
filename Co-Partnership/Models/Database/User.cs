using System;
using System.Collections.Generic;

namespace Co_Partnership.Models.Database
{
    public partial class User
    {
        public User()
        {
            Address = new HashSet<Address>();
            MessageReceiverNavigation = new HashSet<Message>();
            MessageSenderNavigation = new HashSet<Message>();
            OfferAdmin = new HashSet<Offer>();
            OfferMember = new HashSet<Offer>();
            Order = new HashSet<Order>();
            PersonalFund = new HashSet<PersonalFund>();
            WishList = new HashSet<WishList>();
        }

        public int Id { get; set; }
        public string ExtId { get; set; }
        public string UserType { get; set; }
        public bool? IsActive { get; set; }

        public ICollection<Address> Address { get; set; }
        public ICollection<Message> MessageReceiverNavigation { get; set; }
        public ICollection<Message> MessageSenderNavigation { get; set; }
        public ICollection<Offer> OfferAdmin { get; set; }
        public ICollection<Offer> OfferMember { get; set; }
        public ICollection<Order> Order { get; set; }
        public ICollection<PersonalFund> PersonalFund { get; set; }
        public ICollection<WishList> WishList { get; set; }
    }
}
