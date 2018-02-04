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

        public Message DeleteItem(int mesId)
        {
            Message mes = db.Message.FirstOrDefault(p => p.Id == mesId);

            if (mes != null)
            {
                db.Message.Remove(mes);
                db.SaveChanges();
            }

            return mes;

        }

        public void UpdateMessage(Message message)
        {
            db.Update(message);
            db.SaveChanges();
        }
        
    }
}
