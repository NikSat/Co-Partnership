using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Co_Partnership.Models.Database;
using Co_Partnership.Data;
using Microsoft.EntityFrameworkCore;

namespace Co_Partnership.Services
{
    public class TransactionItemRepository : ITransactionItemRepository
    {
        // Set the database needed to get the products
        private Co_PartnershipContext db;
        private ITransactionRepository _transactionRepository;
        private Cart _cart;

        // The Constructor
        public TransactionItemRepository(Co_PartnershipContext db, ITransactionRepository transactionRepository, Cart cart)
        {
            this.db = db;
            _transactionRepository = transactionRepository;
            _cart = cart;
        }

        // Get a queryable for items get all, apply queries at the controller level
        public IQueryable<TransactionItem> TIRepository => db.TransactionItem
                                                                .Include(a => a.Item)
                                                                .Include(b => b.Transaction);


        public TransactionItem DeleteItem(int trItemId)
        {
            TransactionItem ite = TIRepository.FirstOrDefault(p => p.Id == trItemId);

            if (ite != null)
            {
                db.TransactionItem.Remove(ite);
                db.SaveChanges();
            }

            return ite;

        }

        public void SaveItem(TransactionItem item)
        {
            db.TransactionItem.Add(item);
            db.SaveChanges();
        }

        public void UpdateItem(TransactionItem item)
        {
            db.Update(item);
            db.SaveChanges();
        }

        //Gets a list of transaction items for a specific transaction if tthey exist
        public List<TransactionItem> GetTransactionItems(int transactionId)
        {
            return TIRepository.Where(ti => ti.TransactionId == transactionId).ToList();
        }

        //Gets an item if it exists inside a transaction
        public TransactionItem GetItem(int transactionId, int itemId)
        {
             return TIRepository.FirstOrDefault(ti => ti.TransactionId == transactionId && ti.ItemId == itemId);
        }

        public void SaveCartToDB(int userid) //from session to db
        {
            var incomplete = _transactionRepository.GetIncompleteTransaction(userid); // incomplete transaction = cart saved tto db

            //an uparxei cart sto db fernw ta items tou DB kai ta sugkrinw me tou cart(session)
            if (incomplete != null)
            {
                var dbItems = TIRepository.Where(ti => ti.TransactionId == incomplete.Id).ToList(); // citems in DB

                foreach (var itemDB in dbItems)
                {
                    var itemInCart = _cart.GetCartItem((int)itemDB.ItemId); // get the item if it exists in cart

                    if(itemInCart != null) //if it exists in both, take cart quantity and update DB
                    {
                        itemDB.Quantinty = itemInCart.Quantinty;
                        UpdateItem(itemDB);
                    }
                    else // if it doesnt exist in cart, delete it from database
                    {
                        DeleteItem(itemDB.Id);
                    }
                }
            }
            else // make the empty cart in DB (incomplete transaction type = 0)
            {
                var incTransaction = new Transaction()
                {
                    OwnerId = userid,
                    Date = DateTime.Now,
                    Type = 0,
                    IsProcessed = 0
                };
                _transactionRepository.SaveTransaction(incTransaction); // save new transaction to db
                incomplete = _transactionRepository.Transactions.FirstOrDefault(t => t.OwnerId == userid && t.Type == 0); // get incomplete transaction
            }


            var transactionId = incomplete.Id;

            //if there are items in cart(session) that arent saved in DB now we save them
            foreach (var cItem in _cart.CartItems)
            {
                var itemInDB = GetItem(transactionId, (int)cItem.ItemId);

                if (itemInDB == null) // an kapoio cart item den uparxei sto db to ftiaxnw k to pros8etw
                {
                    var transactionItem = new TransactionItem()
                    {
                        TransactionId = transactionId,
                        ItemId = cItem.ItemId,
                        Quantinty = cItem.Quantinty,
                        Acceptance = true
                    };

                    SaveItem(transactionItem);
                }
            }

            // then get all items for this incomplete transaction
            List<TransactionItem> items = GetTransactionItems(transactionId);
            // calculate price
            var price = items.Select(i => (decimal)i.Quantinty * i.Item.UnitPrice).Sum();// without VAT

            //save price into transaction
            incomplete.Price = price;
            _transactionRepository.UpdateTransaction(incomplete);
        }
    }
}
