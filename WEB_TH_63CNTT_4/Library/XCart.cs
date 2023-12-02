using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEB_TH_63CNTT_4.Library;

namespace WEB_TH_63CNTT_4.Library
{
    public class XCart
    {
        List<CartItem> list;
        public List<CartItem> AddCart(CartItem cartitem, int productid)
        {
            if (System.Web.HttpContext.Current.Session["MyCart"].Equals(""))//session chua co gio hang
            {
                List<CartItem> list = new List<CartItem>();
                list.Add(cartitem);
                System.Web.HttpContext.Current.Session["MyCart"] = list;
            }
            else
            {
                //da co thong tin trong gio hang, lay thong tin cua session -> ep  kieu ve list 
                List<CartItem> list = (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
                //kiewm tra productid da co trong danh sach hay chua
                int count = list.Where(m => m.ProductId == productid).Count();
                if (count > 0)//da co trong danh sach gio hang truoc do
                {
                    cartitem.Amount += 1;
                    //cap nhat lai danh sach
                    int vt = 0;
                    foreach (var item in list)
                    {
                        if (item.ProductId == productid)
                        {
                            list[vt].Amount += 1;
                            list[vt].Total = list[vt].Amount * list[vt].Price;
                        }
                        vt++;
                    }
                    System.Web.HttpContext.Current.Session["MyCart"] = list;
                }
                else
                {
                    //them vao gio hang moi
                    list.Add(cartitem);
                    System.Web.HttpContext.Current.Session["MyCart"] = list;
                }
            }
            return list;
        }

        //////////////////////////////////////////////////////////////////
        ///UpdateCart
        public void UpdateCart(string[] arramout)
        {

            // da co thong tin trong gio hang, lay thong tin cua session -> ep kieu ve list
            List<CartItem> list = this.GetCart();
            int vt = 0;
            foreach (CartItem cartitem in list)
            {
                list[vt].Amount = int.Parse(arramout[vt]);
                list[vt].Total = list[vt].Amount * list[vt].Price;
                vt++;
            }
            //cap nhat lai gio hang
            System.Web.HttpContext.Current.Session["MyCart"] = list;
        }
        //////////////////////////////////////////////////////////////////
        ///DelCart
        public void DelCart(int? productid = null)
        {
            if (productid != null)
            {
                if (!System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
                {
                    List<CartItem> list = (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
                    int vt = 0;
                    foreach (var item in list)
                    {
                        if (item.ProductId == productid)
                        {
                            list.RemoveAt(vt);
                            break;
                        }
                        vt++;
                    }
                    //cap nhat lai gio hang
                    System.Web.HttpContext.Current.Session["MyCart"] = list;
                }
            }
            else
            {
                //cap nhat lai gio hang
                System.Web.HttpContext.Current.Session["MyCart"] = "";
            }
        }

        //////////////////////////////////////////////////////////////////
        ///GetCart
        public List<CartItem> GetCart()
        {
            if (System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
            {
                return null;
            }
            return (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
        }
    }
}