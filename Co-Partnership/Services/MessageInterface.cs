using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;
using Microsoft.EntityFrameworkCore;

namespace Co_Partnership.Services
{
    public class MessageInterface : IMessageInterface
    {
        private Co_PartnershipContext db;

        public MessageInterface(Co_PartnershipContext db)
        {
            this.db = db;
        }
        
        public IQueryable<Message> Messages => db.Message
                                                        .Include(a => a.Sender)
                                                        .Include(a => a.Receiver);

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
        
        // This function returns only the messages sender date sent if it is processed and the title
        public IQueryable<Object> GetMessageSummary(int userId)
        {
            var MessageList =
                from Message in Messages
                where Message.ReceiverId ==userId
                select new
                {
                    Id = Message.Id,
                    SenderName=Message.Sender.FirstName +" "+ Message.Sender.LastName,
                    Title=Message.Title,
                    DateSent=Message.DateSent,
                    Status=Message.Processed
                };

            return MessageList.OrderByDescending(a => a.Id);

        }

        //This function returns the detailed message
        public Object GetDetailed(int id)
        {
            var selection = Messages.SingleOrDefault(a => a.Id==id);
            if (selection != null)
            {
                return new
                {
                    Id = selection.Id,
                    SenderName = selection.Sender.FirstName + " " + selection.Sender.LastName,
                    Title = selection.Title,
                    DateSent = selection.DateSent,
                    Status = selection.Processed,
                    Text =selection.Message1
                };
            }
            else
            {
                return null;

            }

        }

        // This function flags an unread message as read
        public void MarkRead(int id)
        {
            Message mes = db.Message.FirstOrDefault(p => p.Id == id);
            if (mes != null)
            {
                if (mes.Processed==false)
                {
                    mes.Processed = true;
                    mes.DateProcessed = DateTime.Now;
                    db.Update(mes);
                    db.SaveChanges();
                }
            }
        }

        public IQueryable<object> GetSentMessageSummary(int userId)
        {
            var MessageList =
               from Message in Messages
               where Message.SenderId == userId
               select new
               {
                   Id = Message.Id,
                   SenderName = "To: "+Message.Receiver.FirstName + " " + Message.Receiver.LastName,
                   Title = Message.Title,
                   DateSent = Message.DateSent,
                   Status = Message.Processed
               };

            return MessageList.OrderByDescending(a => a.Id);
        }

    }
}
