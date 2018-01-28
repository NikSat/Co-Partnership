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


namespace Co_Partnership.Controllers
{
    [Produces("application/json")]
    [Route("Admin/api/Finance")]
    public class FinanceController : Controller
    {
        // This creates the apis for the financial panel

        private ITransactionRepository financeRepository;

        public FinanceController(ITransactionRepository repository)
        {
            financeRepository = repository;
        }

        // This function gets the orders that are not reviewed yet
        // GET: Admin/api/financies
        [HttpGet]
        public IEnumerable<Transaction> Get()
        {
            return financeRepository.Transactions.Where(x => x.IsProcessed == 1);
        }





        
    }
}