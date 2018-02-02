using Co_Partnership.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    public class Cart
    {

        private List<TransactionItem> cartItems = new List<TransactionItem>();

        public List<TransactionItem> CartItems => cartItems;

        public virtual TransactionItem AddItem(Item item, int quantity)
        {
            //if the item already exists inside the cart save it in product else null
            var cartItem = cartItems
                .Where(i => i.Item.Id == item.Id)
                .SingleOrDefault(); 

            
            if (cartItem == null)
            {
                cartItems.Add(new TransactionItem()
                {
                    ItemId = item.Id,
                    Item = item,
                    Quantinty = quantity
                });
            }
            else
            {
                cartItem.Quantinty += quantity;
            }
            return cartItem;
        }

        public virtual TransactionItem UpdateQuantity(int ItemId, int quantity)
        {
            var cartItem = cartItems.SingleOrDefault(c => c.ItemId == ItemId);
            if(cartItem != null)
            {
                cartItem.Quantinty = quantity;
                return cartItem;
            }
            return null;
        }

        public virtual void RemoveItem(int itemId)
        {
            cartItems.RemoveAll(i => i.Item.Id == itemId);
        }

        public virtual decimal? ComputeItemValue(TransactionItem cartItem)
        {
            var product = cartItems
                .Where(i => i.Item.Id == cartItem.ItemId)
                .SingleOrDefault();
            if (product == null)
            {
                return 0;
            }

            return product.Item.UnitPrice * Convert.ToDecimal(product.Quantinty);
        }

        public virtual decimal ComputeTotalValue() => cartItems.Sum(e => e.Item.UnitPrice.Value * Convert.ToDecimal(e.Quantinty));

        public virtual void Clear() => cartItems.Clear();
       

    }
}
