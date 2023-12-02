
using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class OrdersDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Orders> getList()
        {
            return db.Orders.ToList();
        }
        // GET: Admin/Supplier

        //INDEX dua vao status = 1,2, còn status = 0 == thung rac
        public List<Orders> getList(string status = "All")
        {
            List<Orders> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Orders.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Orders.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        return db.Orders.ToList();
                    }
            }
            return list;
        }
        // Details
        public Orders getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Orders.Find(id);
            }
        }


        // Create 
        public int Insert(Orders row)
        {
            db.Orders.Add(row);
            return db.SaveChanges();

        }
        //Update
        public int Update(Orders row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Delete
        public int Delete(Orders row)
        {
            db.Orders.Remove(row);
            return db.SaveChanges();
        }
    }
}