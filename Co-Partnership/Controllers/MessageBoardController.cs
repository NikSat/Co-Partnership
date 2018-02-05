using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Co_Partnership.Models.AccountViewModels;
using Co_Partnership.Models;
using Co_Partnership.Services;
using Co_Partnership.Models.Database;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Co_Partnership.Controllers
{   //[Authorize]
    [Produces("application/json")]
    public class MessageBoardController : Controller
    {
        // This controller creates Apis for the message panel

        //It requires the message repository, user repository  and usermanager to get current user
        private IMessageInterface messageInterface;
        private IUserRepository userep;
        private UserManager<ApplicationUser> manager;

        // Constructor
        public MessageBoardController(UserManager<ApplicationUser> mngr, IMessageInterface wRpstr, IUserRepository us)
        {
            manager = mngr;
            messageInterface = wRpstr;
            userep = us;
        }

        // This  function gets the message summaries for all items
        // GET: api/MessageBoard
        [Route("/api/MessageBoard")]
        [HttpGet]
        public IEnumerable<Object> Get()
        {
            return messageInterface.GetMessageSummary();
        }

        /*
        // This function gets all the messages in a specific time window
        [Route("/api/MessageBoard")]
        [HttpGet]
        public IEnumerable<Message> Get(DateTime start,DateTime end)
        {
            if (end == null)
            {
                end = DateTime.Now;
            }
            return messageInterface.Messages.Where(a => a.DateSent==start && (a.DateProcessed==end || a.DateProcessed==null));
        }
        */


        // GET: api/MessageBoard/5
        [Route("/api/MessageBoard")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(messageInterface.GetDetailed(id));
        }

        // This function marks this message as read
        [Route("/api/MessageBoard/Read")]
        [HttpPost]
        public void ToggleRead([FromBody]Message message)
        {
            int id = message.Id;


        }

        // POST: api/MessageBoard
        [Route("/api/MessageBoard/Detail")]
        [HttpPost]
        public Object Post([FromBody]Message message)
        {
            int id = message.Id;
            return Ok(messageInterface.GetDetailed(id));
        }
        
        // PUT: api/MessageBoard/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
