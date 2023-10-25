using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class SuppliersDAO
    {
        private MyDBContext db = new MyDBContext();

        //INDEX
        public List<Suppliers> getList()
        {
            return db.Suppliers.ToList();
        }

        //INDEX DỰA VÀO STATUSS = 1,2 con status = 0 == thùng rác

        public List<Suppliers> getList(string status = "ALL")
        {
            List<Suppliers> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Suppliers
                        .Where(m => m.Status != 0)
                        .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Suppliers
                        .Where(m => m.Status == 0)
                        .ToList();
                        break;
                    }
                default:
                    {
                        return db.Suppliers.ToList();
                    }
            }
            return list;
        }

        // Details
        public Suppliers getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Suppliers.Find(id);
            }
        }

        //CREATE
        public int Insert(Suppliers row)
        {
            db.Suppliers.Add(row);
            return db.SaveChanges();
        }

        //UPDATE
        public int Update(Suppliers row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //DELETE
        public int Delete(Suppliers row)
        {
            db.Suppliers.Remove(row);
            return db.SaveChanges();
        }
        //
        //public Suppliers getCol(string id)
        //{
        //    if (id.ToString() == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return db.Suppliers.Find(id.ToString());
        //    }
        //}

    }
}
