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
using Microsoft.AspNetCore.Identity;

namespace Co_Partnership.Controllers
{
    [Produces("application/json")]
    public class FinanceController : Controller
    {
        // This controller creates the apis for the financial panel

        // It requires both  transactions and the total found
        private ITransactionRepository financeRepository;
        private ICompAccountRepository compRepository;
        private IUserRepository userRepository;
        private UserManager<ApplicationUser> manager;


        public FinanceController(ITransactionRepository repository,ICompAccountRepository varcomprepository, IUserRepository us, UserManager<ApplicationUser> mngr)
        {
            financeRepository = repository;
            compRepository = varcomprepository;
            userRepository = us;
            manager = mngr;
        }

        // This function gets the current user
        public async Task<int> GetUserId()
        {
            var currentuser = await manager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);


            var use = await userRepository.RetrieveByExternalAsync(currentuser.Id);
            return use.Id;
        }


        // This function gets the sales/orders etc for a period of time
        [Route("/api/Finance/PerDate")]
        [HttpPost]
        public IEnumerable<Object> FinSum([FromBody]Transaction trans)
        {
            DateTime start = (DateTime)trans.Date;
            DateTime end = (DateTime)trans.DateProcessed;
            int id = trans.Id;
            return financeRepository.getSummaries(id, start, end);


        }


        // This function gets the orders that are not reviewed yet (order type is integer 1 )
        // GET: Admin/api/financies/Order
        [Route("Admin/api/Finance/Order")]
        [HttpGet]
        public IEnumerable<Object> Get()
        {
            return financeRepository.ListTransactions(1);
        }
        
        // This function gets the items from each order
        [Route("Admin/api/Finance/Order/Details")]
        [HttpGet("{id}")]
        public IEnumerable<Object> Get(int id)
        {
            return financeRepository.ListItems(id);
        }
        
        // This function updates the orders 
        [Route("Admin/api/Finance/Order/Update")]
        [HttpPost]
        public IActionResult Update([FromBody]int id)
        {
            //Get the transaction and update it
            var transaction=financeRepository.Transactions.FirstOrDefault(a => a.Id == id);
            transaction.IsProcessed = 1;
            transaction.DateProcessed = DateTime.Now;
            financeRepository.UpdateTransaction(transaction);
            // Also update the whole fund
            var found = compRepository.Account.FirstOrDefault(a=> a.Id==1);
            found.MemberShare = found.MemberShare + (transaction.Price / 2);
            found.CoOpShare=found.CoOpShare + (transaction.Price / 2);
            found.Date = DateTime.Now;
            compRepository.UpdateAccount(found);
            return Ok();

        }
        
        // This function gets all the purchases for this user
        [Route("/api/Finance/PurchaseHistory")]
        [HttpPost]
        public async Task<IEnumerable<Object>> GetPurchaseHistory()
        {
            // Get the user
            int BId = await GetUserId();
            // Get his purchases
            return financeRepository.GetPurchaseHistory(BId);

        }


    }
}