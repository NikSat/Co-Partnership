using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Co_Partnership.Models;
using Co_Partnership.Services;
using Co_Partnership.Models.Database;
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
        private IPersonalAccountRepository personalAccount;
        private IMessageInterface messageInterface;

        public FinanceController(ITransactionRepository repository, ICompAccountRepository varcomprepository, IUserRepository us, UserManager<ApplicationUser> mngr, IPersonalAccountRepository psac, IMessageInterface msg)
        {
            financeRepository = repository;
            compRepository = varcomprepository;
            userRepository = us;
            manager = mngr;
            personalAccount = psac;
            messageInterface = msg;
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
            var transaction = financeRepository.Transactions.FirstOrDefault(a => a.Id == id);
            transaction.IsProcessed = 1;
            transaction.DateProcessed = DateTime.Now;
            financeRepository.UpdateTransaction(transaction);
            // Also update the whole fund
            var found = compRepository.Account;
            found.MemberShare = found.MemberShare + (transaction.Price / 2);
            found.CoOpShare = found.CoOpShare + (transaction.Price / 2);
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

        // This function gets the total founds in the co-partnership account
        [Route("/api/Finance/TotalFounds")]
        [HttpPut]
        public Object GetTotalFounds()
        {
            return new
            {
                CoOpShare= compRepository.Account.CoOpShare,
                MemberShare = compRepository.Account.MemberShare
            };
        }

        // This function gets the total number of members
        [Route("/api/Finance/TotalMembers")]
        [HttpPost]
        public Object NumberOfMembers()
        {
            return new {
                Number= userRepository.Users.Count(a => a.UserType == 2),
                Founds = compRepository.Account.MemberShare
            };
        }

        // This function gets the orders offers summary
        [Route("/api/Finance/OrderSummary")]
        [HttpPost]
        public Object GetOrderSummary([FromBody]Transaction trans)
        {
            int id = trans.Id;
            return financeRepository.SummarizeNewTransactions(id);
        }

        // Well, here we go again ....
        // This function awards all the dividends
        [Route("/api/Finance/AwardDividents")]
        [HttpPost]
        public async Task<IActionResult> AwardDividents()
        {
            // Lets take it slowly step by step
            // Checks
            // Find the total number of members
            IEnumerable<User> Members= userRepository.Users.Where(a => a.UserType == 2);

            if (!Members.Any())
            {
                // Bad request if no members 
                return StatusCode(406);
            }
            // Number of members
            int NoMembers = Members.Count();

            // Get the total member founds
            decimal TotalShare = (decimal)compRepository.Account.MemberShare;
            if (TotalShare==0)
            {
                // Precondition Fail, no money!
                return StatusCode(412);
            }

            // The devident for each user
            decimal Divident = TotalShare / ((decimal)NoMembers);

            // Get the current user
            int CuId = await GetUserId();

            // For each member
            foreach (User us in Members)
            {
                // Give money if he does not have an account create one
                PersonalFinancialAccount personal = personalAccount.Account.SingleOrDefault(a => a.UserId == us.Id);
                if (personal == null)
                {
                    personal = new PersonalFinancialAccount();
                    personal.UserId = us.Id;
                    personal.Amount = Divident;
                    personalAccount.AddAccount(personal);
                }
                else
                {
                    personal.Amount += Divident;
                    personalAccount.UpdateAccount(personal);
                }
                // Register Transaction

                Transaction Current = new Transaction();
                Current.Date = DateTime.Now;
                Current.OwnerId = CuId;
                Current.RecipientId = us.Id;
                Current.DateProcessed = DateTime.Now;
                Current.IsProcessed = 1;
                Current.Price = Divident;
                Current.Type = 3;
                financeRepository.SaveTransaction(Current);

                // Inform the user
                Message message = new Message();

                message.SenderId = CuId;
                message.ReceiverId = us.Id;
                message.DateSent = DateTime.Now;
                message.Processed = false;
                message.Title = "Divident Award";
                message.Message1 = $"Today {DateTime.Today.ToString()},<br> the CoPartnership awards an intermittent divident which amounts to {Divident.ToString("#.##")} euros. <br> Kind regards, <br> the Administrative Team ";
                messageInterface.SaveMessage(message);
            }

            // For the Company found

            // Empty the account 
            CompanyFinancialAccount account = compRepository.Account;
            account.MemberShare = 0;
            compRepository.UpdateAccount(account);

            return Ok();
        }
    }
}