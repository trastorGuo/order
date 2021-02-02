using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrderApi.Domains;
using OrderApi.Models;
using OrderApi.MsgCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [WebApi]
    public class ProductController : BaseController
    {
        [HttpGet]
        public object GetProductList(string account)
        {
            return ProductDomain.Current.GetProductList(account);
        }

       

        [Auth]
        [HttpPost]
        public object EditProduct(JToken jt)
        {
            return ProductDomain.Current.EditProduct(jt, ACCOUNT);
        }

        

        [HttpGet]
        [Auth]
        public object DeleteProduct(string id)
        {
            return ProductDomain.Current.DeleteProduct(id);
        }

       

        [Auth]
        [HttpPost]
        public object AddProduct(JToken jt)
        {
            return ProductDomain.Current.AddProduct(jt, ACCOUNT);
        }

        

        [HttpPost]
        [Auth]
        public object AddType(JToken jt)
        {
            return ProductDomain.Current.AddType(jt, ACCOUNT, SHOP_ID);
        }

        

        [HttpPost]
        [Auth]
        public object EditType(JToken jt)
        {
            return ProductDomain.Current.EditType(jt, ACCOUNT, SHOP_ID);
        }

       

        [HttpGet]
        [Auth]
        public object DeleteType(string id)
        {
            return ProductDomain.Current.DeleteType(id, SHOP_ID);
        }

       

        [HttpPost]
        public object PlaceAnOrder(JToken jt)
        {
            return  ProductDomain.Current.PlaceAnOrder(jt);
        }

       

        [HttpGet]
        [Auth]
        public object GetOrders(string id, string datetime, string to, string userOrdered)
        {
            return  ProductDomain.Current.GetOrders(id, datetime, to, userOrdered);
        }

        


        /// <summary>
        /// 判断当前桌是否有人正在占用 true:被占用
        /// </summary>
        /// <param name="desckNum"></param>
        /// <param name="shopAcount"></param>
        /// <returns></returns>
        [HttpGet]
        public bool DeskIsOccupied(string desckNum, string shopAcount)
        {
            return  ProductDomain.Current.DeskIsOccupied(desckNum, shopAcount);
        }

        

        [HttpGet]
        public bool CloseOrder(string orderId)
        {
            return  ProductDomain.Current.CloseOrder(orderId);
        }

        

        [HttpGet]
        [Auth]
        public object GetDeskList()
        {
            using (var db = new OrderDB())
            {
                var result = from p in db.ShopDesks where p.ShopId == SHOP_ID select p;
                return result.ToList();
            }
        }


        [HttpGet]
        [Auth]
        public string AddOrEditDesk(string deskNum, string deskDesc)
        {
            return  ProductDomain.Current.AddOrEditDesk(deskNum, deskDesc, ACCOUNT, SHOP_ID);
        }

      

        [HttpGet]
        [Auth]
        public string DeleteDesk(string descNum)
        {
            return  ProductDomain.Current.DeleteDesk(descNum, ACCOUNT, SHOP_ID);
        }

       
    }
}
