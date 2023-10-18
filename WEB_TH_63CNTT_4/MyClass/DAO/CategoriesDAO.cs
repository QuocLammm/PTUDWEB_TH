using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{

    public class CategoriesDAO
    {
        private MyDBContext db = new MyDBContext();

        //INDEX
        public List<Categories> getList()
        {
            return db.Categories.ToList();
        }

        //INDEX DỰA VÀO STATUSS = 1,2 con status = 0 == thùng rác

        public List<Categories> getList(string status = "ALL")
        {
            List<Categories> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Categories
                        .Where(m=> m.Status !=0)
                        .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Categories
                        .Where(m => m.Status == 0)
                        .ToList();
                        break;
                    }
                default:
                    {
                        return db.Categories.ToList();
                    }
            }
            return list;
        }

        // Details
        public Categories getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Categories.Find(id);
            }
        }

        //CREATE
        public int Insert(Categories row)
        {
            db.Categories.Add(row);
            return db.SaveChanges();
        }

        //UPDATE
        public int Update(Categories row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //DELETE
        public int Delete(Categories row)
        {
            db.Categories.Remove(row);
            return db.SaveChanges();
        }

        //TRASH

    }
}