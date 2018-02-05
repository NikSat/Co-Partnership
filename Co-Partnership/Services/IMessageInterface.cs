﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Services
{
    public interface IMessageInterface
    {
        IQueryable<Message> Messages { get; }

        void SaveMessage(Message message);

        Message DeleteItem(int mesId);
        
        void UpdateMessage(Message message);

        IQueryable<Object> GetMessageSummary();

        Object GetDetailed(int id);
    }
}
