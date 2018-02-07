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


        // This function gets the user Id of the current user
        public async Task<int> GetUserId()
        {
            var currentuser = await manager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);


            var use = await userep.RetrieveByExternalAsync(currentuser.Id);
            return use.Id;
        }


        // This function returns the user role
        public async Task<string> GetUserRole()
        {
            
            var currentuser = await manager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);
            string rolename="";
            return rolename;
            
        }





        // This  function gets the message summaries for all items
        // GET: api/MessageBoard
        [Route("/api/MessageBoard/GetSum")]
        [HttpGet]
        public async Task<IEnumerable<Object>> GetSummaries()
        {
            int userId = await GetUserId();
            return messageInterface.GetMessageSummary(userId);
        }

        // This  function gets the message summaries for all items
        // GET: api/MessageBoard
        [Route("/api/MessageBoard/Sent")]
        [HttpGet]
        public async Task<IEnumerable<Object>> GetSent()
        {
            int userId = await GetUserId();
            return messageInterface.GetSentMessageSummary(userId);
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
        

        // This function gets a specific message summary from the list
        // GET: api/MessageBoard/5
        [Route("/api/MessageBoard")]
        [HttpGet("{id}")]
        public IActionResult GetMessage(int id)
        {
            return Ok(messageInterface.GetDetailed(id));
        }
        */
        
        // This function gets a sepcific item using post
        // POST: api/MessageBoard
        [Route("/api/MessageBoard/Detail")]
        [HttpPost]
        public Object Post([FromBody]Message message)
        {
            int id = message.Id;
            return Ok(messageInterface.GetDetailed(id));
        }

        // This function marks this message as read if it is unread or unread if it is read (same as previous but with post)
        [Route("/api/MessageBoard/Read")]
        [HttpPost]
        public void MarkRead([FromBody]Message message)
        {
            int id = message.Id;
            messageInterface.MarkRead(id);

        }

        // This function deletes a message using the post method
        [Route("/api/MessageBoard/Delete")]
        [HttpPost]
        public void Delete([FromBody]Message message)
        {
            int id = message.Id;
            messageInterface.DeleteItem(id);
        }


        // This function searches for members depending on the user role
        [Route("/api/MessageBoard/Search")]
        [HttpPost]
        public IEnumerable<Object> Search([FromBody]Message message)
        {
            // The search querry
            string quer = message.Title;
            // Now get the results depending on the role of the user
            IQueryable<User> UserPool;

            if (User.IsInRole("Admin"))
            {
                // Admin can select all
                UserPool =userep.Users;
                
            }
            else if (User.IsInRole("Member"))
            {
                // Members can contact admin or other members
                UserPool = userep.Users.Where(a => a.UserType == 2 || a.UserType == 3);
            }
            else
            {
                //Clients can contact only admins
                UserPool = userep.Users.Where(a => a.UserType == 3);
            }

            var Selection =
                from User in UserPool
                where User.FirstName.Contains(quer) || User.LastName.Contains(quer)
                select new
                {
                    Id = User.Id,
                    SenderName = User.FirstName + " " + User.LastName,
                };

            return Selection;

        }


        // This function returns the list of administrators
        [Route("/api/MessageBoard/GetAdmins")]
        [HttpPost]
        public IEnumerable<Object> GetAdmins([FromBody]Message message)
        {
            IQueryable<User> UserPool=userep.Users.Where(a => a.UserType == 3);
  

        var Selection =
            from User in UserPool
            select new
            {
                Id = User.Id,
                SenderName = User.FirstName + " " + User.LastName,
            };

            return Selection;
        }


        // This function creates a new message from a form
        [Route("/api/MessageBoard/Send")]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody]Message message)
        {
            
            if (message.Message1 == "" || message.Title == "" || message.ReceiverId==0)
            {
                return BadRequest();
                
            }
            else
            {

                message.SenderId =await GetUserId();
                message.DateSent = DateTime.Now;
                message.Processed = false;
                messageInterface.SaveMessage(message);

                return Ok();

            }

            
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
