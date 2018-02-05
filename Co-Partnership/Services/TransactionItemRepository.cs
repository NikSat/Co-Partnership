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
        public TransactionItemRepository(
            Co_PartnershipContext db,
            ITransactionRepository transactionRepository,
            Cart cart)
        {
            this.db = db;
            _transactionRepository = transactionRepository;
            _cart = cart;
        }

        // Get a queryable for items get all, apply queries at the controller level
        public IQueryable<TransactionItem> TIRepository => db.TransactionItem
                                                                .Include(a => a.Item)
                                                                .Include(b => b.Transaction);

        public TransactionItem DeleteItem(TransactionItem item)
        {
            db.TransactionItem.Remove(item);
            db.SaveChanges();
            return item;
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

        public void AddOrUpdate(TransactionItem item)
        {
            var transItem = GetItem(item.Id, item.ItemId);
            if (transItem == null)
            {
                db.TransactionItem.Add(item);
            }
            else
            {
                //db.Update(transItem);
                transItem.Quantinty = item.Quantinty;
                db.Update(transItem);
            }
            db.SaveChanges();
        }

        //Gets a list of transaction items for a specific transaction if tthey exist
        public List<TransactionItem> GetTransactionItems(int transactionId)
        {
            return TIRepository.Where(ti => ti.TransactionId == transactionId).ToList();
        }

        //Gets an item if it exists inside a transaction
        public TransactionItem GetItem(int transactionId, int? itemId)
        {
            return TIRepository.SingleOrDefault(ti => ti.TransactionId == transactionId && ti.ItemId == itemId);
        }

        // if there isnt a cart in DB
        // creates one and adds session items      
        public void CreateDbCart(int userid)
        {
            var price = _cart.ComputeTotalValue();
            var incTransaction = new Transaction() // make cart in DB
            {
                OwnerId = userid,
                Date = DateTime.Now,
                Type = 0,
                IsProcessed = 0,
                Price = price
            };
            _transactionRepository.SaveTransaction(incTransaction); // save cart to db
            var incomplete = _transactionRepository.Transactions.FirstOrDefault(t => t.OwnerId == userid && t.Type == 0); // get it from db (auto id)

            var transactionId = incomplete.Id;

            foreach (var cItem in _cart.CartItems) // save session items to db
            {
                var itemExistsInDb = GetItem(transactionId, (int)cItem.ItemId);
                if (itemExistsInDb != null) // if item exists in DB
                {
                    itemExistsInDb.Quantinty = cItem.Quantinty;
                    UpdateItem(itemExistsInDb);
                }
                else // if item doesnt exist in db, save sessionitem to DB
                {
                    var transactionItem = new TransactionItem()
                    {
                        TransactionId = transactionId,
                        ItemId = cItem.ItemId,
                        Quantinty = cItem.Quantinty,
                        Acceptance = true
                    };
                    AddOrUpdate(transactionItem);
                }
            }
        }

        //if there isnt a cart in db => create cart in db
        //else =>
        //recalculate price and save it in db
        // then Get session items 
        // if they exist in DB update them in db
        // if they dont add them to db
        // if there are items in DB that arent in session delete them from db
        // 
        public void SaveCartToDB(int userid) //from session to db
        {
            var incomplete = _transactionRepository.GetIncompleteTransaction(userid); // incomplete transaction = cart saved tto db

            if (incomplete != null)
            {
                var price = _cart.ComputeTotalValue(); // update price in db
                incomplete.Price = price;
                _transactionRepository.UpdateTransaction(incomplete);

                var transactionId = incomplete.Id;
                var cartItems = _cart.CartItems;
                foreach (var cItem in cartItems)
                {
                    var itemExistsInDb = GetItem(transactionId, (int)cItem.ItemId);
                    if (itemExistsInDb != null && itemExistsInDb.Quantinty != cItem.Quantinty) // if item exists in DB
                    {
                        itemExistsInDb.Quantinty = cItem.Quantinty;
                        UpdateItem(itemExistsInDb);
                    }
                    else if (itemExistsInDb == null)// if item doesnt exist in db, save sessionitem to DB
                    {
                        var transactionItem = new TransactionItem()
                        {
                            TransactionId = transactionId,
                            ItemId = cItem.ItemId,
                            Quantinty = cItem.Quantinty,
                            Acceptance = true
                        };
                        AddOrUpdate(transactionItem);
                    }
                }

                var dbItems = GetTransactionItems(transactionId);
                foreach (var dbItem in dbItems) // if db item doesnt exist in cart
                {
                    var itemExistsInCart = _cart.GetCartItem((int)dbItem.ItemId);
                    if (itemExistsInCart == null)
                    {
                        DeleteItem(dbItem);
                    }
                }
            }
            else
            {
                CreateDbCart(userid);
            }
        }
    }
}
