using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_TH_63CNTT_4.Library
{
    public class CartItem
    {
        // cac truong thong tin duoc khai bao o day
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public decimal Total { get; set; }
        public CartItem(int proid, string name, string img, decimal price, int qty)
        {
            this.ProductId = proid;
            this.Name = name;
            this.Img = img;
            this.Price = price;
            this.Amount = qty;
            this.Total = price * qty;
        }

        public CartItem()
        {

        }
    }
}