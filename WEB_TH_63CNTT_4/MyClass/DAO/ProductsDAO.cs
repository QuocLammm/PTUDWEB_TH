using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class ProductsDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Products> getListByCatId(int catid, int limit)
        {
            List<Products> list = db.Products
                .Where(m => m.CatID == catid && m.Status == 1)
                .Take(limit)
                .OrderByDescending(m => m.CreateBy)
                .ToList();
            return list;
        }
        //danh cho trang HOME
        public List<ProductInfo> getListByListCatId(List<int> listcatid, int limit)
        {
            List<ProductInfo> list = db.Products
                .Join(
                                db.Categories, // Bảng Categories
                                p => p.CatID, // Khóa ngoại của Products liên kết với Categories
                                c => c.Id, // Khóa chính của Categories
                                (p, c) => new { Product = p, Category = c } // Kết hợp Products và Categories
                            )
                            .Join(
                                db.Suppliers, // Bảng Suppliers
                                pc => pc.Product.SupplierID, // Khóa ngoại của Product/Category liên kết với Suppliers
                                s => s.Id, // Khóa chính của Suppliers
                                (pc, s) => new ProductInfo
                                {
                                    Id = pc.Product.Id,
                                    CatID = pc.Product.CatID,
                                    Name = pc.Product.Name,
                                    CatName = pc.Category.Name, // Lấy tên danh mục từ bảng Categories
                                    SupplierId = pc.Product.SupplierID,
                                    SupplierName = s.Name, // Lấy tên nhà cung cấp từ bảng Suppliers
                                    Slug = pc.Product.Slug,
                                    Image = pc.Product.Image,
                                    Price = pc.Product.Price,
                                    SalePrice = pc.Product.SalePrice,
                                    Amount = pc.Product.Amount,
                                    MetaDesc = pc.Product.MetaDesc,
                                    MetaKey = pc.Product.MetaKey,
                                    CreateBy = pc.Product.CreateBy,
                                    CreateAt = pc.Product.CreateAt,
                                    UpdateBy = pc.Product.UpdateBy,
                                    UpdateAt = pc.Product.UpdateAt,
                                    Status = pc.Product.Status
                                }
                            )
                .Where(m => m.Status == 1 && listcatid.Contains(m.CatID))
                .Take(limit)
                .OrderByDescending(m => m.CreateBy)
                .ToList();
            return list;
        }
        public List<Products> getList()
        {
            return db.Products.ToList();
        }
        // GET: Admin/Supplier

        //INDEX dua vao status = 1,2, còn status = 0 == thung rac
        public List<Products> getList(string status = "All")
        {
            List<Products> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Products.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Products.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        return db.Products.ToList();
                    }
            }
            return list;
        }
        // Details
        public Products getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Products.Find(id);
            }
        }
        //Hien thi danh sach 1 mau tin (ban ghi)
        public Products getRow(string slug)
        {

            return db.Products
                .Where(m => m.Slug == slug && m.Status == 1)
                .FirstOrDefault();

        }

        // Create 
        public int Insert(Products row)
        {
            db.Products.Add(row);
            return db.SaveChanges();

        }
        //Update
        public int Update(Products row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Delete
        public int Delete(Products row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();
        }
        //Chi tiet san pham
        public List<ProductInfo> GetProductDetailBySlug(string slug)
        {
            List<ProductInfo> list = null;
            list = db.Products
                .Where(p => p.Slug == slug && p.Status == 1)
                .Join(
                    db.Categories,
                    p => p.CatID,
                    c => c.Id,
                    (p, c) => new { Product = p, Category = c }
                )
                .Join(
                    db.Suppliers,
                    pc => pc.Product.SupplierID,
                    s => s.Id,
                    (pc, s) => new ProductInfo
                    {
                        Id = pc.Product.Id,
                        CatID = pc.Product.CatID,
                        Name = pc.Product.Name,
                        CatName = pc.Category.Name,
                        SupplierId = pc.Product.SupplierID,
                        SupplierName = s.Name,
                        Slug = pc.Product.Slug,
                        Image = pc.Product.Image,
                        Price = pc.Product.Price,
                        SalePrice = pc.Product.SalePrice,
                        Amount = pc.Product.Amount,
                        MetaDesc = pc.Product.MetaDesc,
                        MetaKey = pc.Product.MetaKey,
                        CreateBy = pc.Product.CreateBy,
                        CreateAt = pc.Product.CreateAt,
                        UpdateBy = pc.Product.UpdateBy,
                        UpdateAt = pc.Product.UpdateAt,
                        Status = pc.Product.Status,
                        //bo sung truong Slug cua Categories
                        CategorySlug = pc.Category.Slug
                    }
                )
                .ToList();
            return list;
        }
        ///Trang giao dien chi tiet san pham
        public List<ProductInfo> GetProductDetailByCategoryId(int catid)
        {
            var relatedProducts = db.Products
                .Where(p => p.CatID == catid && p.Status == 1)
                .Join(
                    db.Categories,
                    p => p.CatID,
                    c => c.Id,
                    (p, c) => new { Product = p, Category = c }
                )
                .Join(
                    db.Suppliers,
                    pc => pc.Product.SupplierID,
                    s => s.Id,
                    (pc, s) => new ProductInfo
                    {
                        Id = pc.Product.Id,
                        CatID = pc.Product.CatID,
                        Name = pc.Product.Name,
                        CatName = pc.Category.Name,
                        SupplierId = pc.Product.SupplierID,
                        SupplierName = s.Name,
                        Slug = pc.Product.Slug,
                        Image = pc.Product.Image,
                        Price = pc.Product.Price,
                        SalePrice = pc.Product.SalePrice,
                        Amount = pc.Product.Amount,
                        MetaDesc = pc.Product.MetaDesc,
                        MetaKey = pc.Product.MetaKey,
                        CreateBy = pc.Product.CreateBy,
                        CreateAt = pc.Product.CreateAt,
                        UpdateBy = pc.Product.UpdateBy,
                        UpdateAt = pc.Product.UpdateAt,
                        Status = pc.Product.Status
                    }
                )
                .ToList();

            return relatedProducts;
        }
        public List<ProductInfo> getListBylimit(int limit)
        {
            List<ProductInfo> list = db.Products.Join(db.Categories, //Bang Categories
                                p => p.CatID, //Khoa ngoai cua Products lien ket voi Categories
                                c => c.Id, //Khoa chinh cua Categories
                                (p, c) => new { Product = p, Category = c } //Join Products va Categories
                            )
                            .Join(
                                db.Suppliers, //Bang Suppliers
                                pc => pc.Product.SupplierID, //Khoa ngoai cua Product lien ket voi Suppliers
                                s => s.Id, //Khoa chinh cua Suppliers
                                (pc, s) => new ProductInfo
                                {
                                    Id = pc.Product.Id,
                                    CatID = pc.Product.CatID,
                                    Name = pc.Product.Name,
                                    CatName = pc.Category.Name, //Lay Name tu Categories
                                    SupplierId = pc.Product.SupplierID,
                                    SupplierName = s.Name, //Lay ten NCC tu bang Suppliers
                                    Slug = pc.Product.Slug,
                                    Image = pc.Product.Image,
                                    Price = pc.Product.Price,
                                    SalePrice = pc.Product.SalePrice,
                                    Amount = pc.Product.Amount,
                                    MetaDesc = pc.Product.MetaDesc,
                                    MetaKey = pc.Product.MetaKey,
                                    CreateBy = pc.Product.CreateBy,
                                    CreateAt = pc.Product.CreateAt,
                                    UpdateBy = pc.Product.UpdateBy,
                                    UpdateAt = pc.Product.UpdateAt,
                                    Status = pc.Product.Status
                                }
                            )
                .Where(m => m.Status == 1)
                .Take(limit)
                .OrderByDescending(m => m.CreateBy)
                .ToList();
            return list;
        }
        public List<Products> getProductSup(int id)
        {
            return db.Products.Where(p => p.SupplierID == id).ToList();
        }
    }
}