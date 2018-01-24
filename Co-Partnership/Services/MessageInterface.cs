using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;

namespace Co_Partnership.Services
{
    public class MessageInterface : IMessageInterface
    {
        private Co_PartnershipContext db;

        public MessageInterface(Co_PartnershipContext db)
        {
            this.db = db;
        }


        public IQueryable<Message> Messages => db.Message;

        public void SaveMessage(Message message)
        {
            db.Message.Add(message);
            db.SaveChanges();
        }
    }
}
