using MyClass.Model.MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    public class MyDBContext: DbContext
    {
        // Tạo kết nối
        public MyDBContext(): base("name = StrConnect"){}

        // kết nối các bảng
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Links> Links { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Sliders> Sliders { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Topics> Topics { get; set; }
        public DbSet<Users> Users { get; set; }

    }
}
