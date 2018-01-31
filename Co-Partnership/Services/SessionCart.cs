using Co_Partnership.Models.Database;
using Co_Partnership.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co_Partnership.Services
{
    public class SessionCart : Cart
    {
        private static ISession session;
        private const string sessionKey = "cart";

        public static Cart GetCart(IServiceProvider services)
        {
            session = services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
            var returnData = session.GetString(sessionKey);
            if (returnData == null)
            {
                return new SessionCart();
            }
            else
            {
                return JsonConvert.DeserializeObject<SessionCart>(returnData);
            }
        }

        public override void AddItem(Item item, int quantity)
        {
            base.AddItem(item, quantity);
            session.SetString(sessionKey, JsonConvert.SerializeObject(this));
        }

        public override void RemoveItem(Item item)
        {
            base.RemoveItem(item);
            session.SetString(sessionKey, JsonConvert.SerializeObject(this));
        }

        public override void Clear()
        {
            base.Clear();
            session.Remove(sessionKey);
        }
    }
}
