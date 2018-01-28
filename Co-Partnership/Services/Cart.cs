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

        public virtual void AddItem(Item item, int quantity)
        {
            //if the item already exists inside the cart save it in product else null
            var product = cartItems
                .Where(i => i.Item.Id == item.Id)
                .SingleOrDefault(); 
            
            if (product == null)
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
                product.Quantinty += quantity;
            }
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
