using Co_Partnership.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    public class Cart: ICart
    {

        private List<TransactionItem> cartItems = new List<TransactionItem>();

        public List<TransactionItem> CartItems => cartItems;

        public virtual TransactionItem AddItem(int itemId, int quantity)
        {
            //if the item already exists inside the cart save it in product else null
            var cartItem = cartItems
                .Where(i => i.Item.Id == itemId)
                .SingleOrDefault(); 

            
            if (cartItem == null)
            {
                cartItems.Add(new TransactionItem()
                {
                    ItemId = itemId,                   
                    Quantinty = quantity
                }.include(Item));
            }
            else
            {
                cartItem.Quantinty += quantity;
            }
            return cartItem;
        }

        public virtual void RemoveItem(Item item)
        {
            cartItems.RemoveAll(i => i.Item.Id == item.Id);
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
