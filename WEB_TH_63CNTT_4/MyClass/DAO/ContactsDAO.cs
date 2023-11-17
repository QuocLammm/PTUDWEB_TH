using MyClass.Model;
using MyClass.Model.MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{

    public class ContactsDAO
    {
        private MyDBContext db = new MyDBContext();

        //INDEX
        public List<Contacts> getList()
        {
            return db.Contacts.ToList();
        }

        //INDEX DỰA VÀO STATUSS = 1,2 con status = 0 == thùng rác

        public List<Contacts> getList(string status = "ALL")
        {
            List<Contacts> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Contacts
                        .Where(m => m.Status != 0)
                        .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Contacts
                        .Where(m => m.Status == 0)
                        .ToList();
                        break;
                    }
                default:
                    {
                        return db.Contacts.ToList();
                    }
            }
            return list;
        }

        // Details
        public Contacts getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Contacts.Find(id);
            }
        }

        //CREATE
        public int Insert(Contacts row)
        {
            db.Contacts.Add(row);
            return db.SaveChanges();
        }

        //UPDATE
        public int Update(Contacts row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //DELETE
        public int Delete(Contacts row)
        {
            db.Contacts.Remove(row);
            return db.SaveChanges();
        }

        //TRASH

    }
}