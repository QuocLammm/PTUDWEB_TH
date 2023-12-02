using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using WEB_TH_63CNTT_4.Library;


namespace WEB_TH_63CNTT_4.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductsDAO productDAO = new ProductsDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO supplierDAO = new SuppliersDAO();

        // GET: Admin/Products
        public ActionResult Index()
        {

            var productList = productDAO.getList("Index");
            // Truy xuất danh sách nhà cung cấp
            var supplierList = supplierDAO.getList("Index");

            // Truyền danh sách nhà cung cấp vào View
            ViewBag.Suppliers = supplierList;

            return View(productList);
        }
        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hiển thị thông báo
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm!");
                return RedirectToAction("Index");
            }
            Products products = productDAO.getRow(id);
            if (products == null)
            {
                //hiển thị thông báo
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm!");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.ListSupId = new SelectList(supplierDAO.getList("Index"), "Id", "Name");
            ViewBag.ListCatId = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                products.Slug = XString.Str_Slug(products.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        string id = products.Id.ToString();
                        //Chinh sua sau khi phat hien dieu chua dung cua Edit: them Id
                        //ten file = Slug + Id + phan mo rong cua tap tin
                        string imgName = slug + id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;

                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //upload hinh
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Xu ly cho muc CreateAt
                products.CreateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                products.CreateBy = Convert.ToInt32(Session["UserID"]);
                //Xu ly cho muc CreateAt
                products.UpdateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);

                productDAO.Insert(products);//chen them row vào Table Products

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListSupId = new SelectList(supplierDAO.getList("Index"), "Id", "Name");
            ViewBag.ListCatId = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            return View(products);
        }


        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Product");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Products products = productDAO.getRow(id);
            //kiem tra id cua categories co ton tai?
            if (products == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Product");
            }
            //thay doi trang thai Status tu 1 thanh 2 va nguoc lai
            products.Status = (products.Status == 1) ? 2 : 1;

            //cap nhat gia tri cho UpdateAt/By
            products.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
            products.UpdateAt = DateTime.Now;

            //Goi ham Update trong CategoryDAO
            productDAO.Update(products);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Product");
        }



        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListSupId = new SelectList(supplierDAO.getList("Index"), "Id", "Name");
            ViewBag.ListCatId = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = productDAO.getRow(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                products.Slug = XString.Str_Slug(products.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        string id = products.Id.ToString();

                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;

                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //cap nhat thi phai xoa file cu
                        ////Xoa file
                        //if (products.Image != null)
                        //{
                        //    string DelPath = Path.Combine(Server.MapPath(PathDir), products.Image);
                        //    System.IO.File.Delete(DelPath);
                        //}

                        //upload hinh
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Xu ly cho muc UpdateAt
                products.UpdateAt = DateTime.Now;

                //Xu ly cho muc UpdateBy
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);

                productDAO.Update(products);//chen them row vào Table Products

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật thông tin sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListSupId = new SelectList(supplierDAO.getList("Index"), "Id", "Name");
            ViewBag.ListCatId = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            return View(products);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/DelTrash/5:Thay doi trang thai cua mau tin
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Xóa sản phẩm thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Product");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Products products = productDAO.getRow(id);
            //kiem tra id cua categories co ton tai?
            if (products == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Xóa sản phẩm thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Product");
            }
            //thay doi trang thai Status = 0
            products.Status = 0;

            //cap nhat gia tri cho UpdateAt/By
            products.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
            products.UpdateAt = DateTime.Now;

            //Goi ham Update trong ProductDAO
            productDAO.Update(products);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa sản phẩm thành công");

            //khi cap nhat xong thi chuyen ve Index
            return RedirectToAction("Index", "Product");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // TRASH: Admin/Product: select * from where Status = 0
        public ActionResult Trash(int? id)
        {
            return View(productDAO.getList("Trash"));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Recover/5:Thay doi trang thai cua mau tin
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi sản phẩm thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Product");
            }

            //khi nhap nut thay doi Status cho mot mau tin
            Products products = productDAO.getRow(id);
            //kiem tra id cua categories co ton tai?
            if (products == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi sản phẩm thất bại");

                //chuyen huong trang
                return RedirectToAction("Index", "Product");
            }
            //thay doi trang thai Status = 2
            products.Status = 2;

            //cap nhat gia tri cho UpdateAt/By
            products.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
            products.UpdateAt = DateTime.Now;

            //Goi ham Update trong ProductDAO
            productDAO.Update(products);

            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi sản phẩm thành công");

            //khi cap nhat xong thi chuyen ve Trash
            return RedirectToAction("Trash", "Product");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = productDAO.getRow(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Truy van mau tin theo Id
            Products products = productDAO.getRow(id);

            if (productDAO.Delete(products) == 1)
            {
                //duong dan den anh can xoa
                string PathDir = "~/Public/img/product/";
                //cap nhat thi phai xoa file cu
                if (products.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), products.Image);
                    System.IO.File.Delete(DelPath);
                }
            }
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa danh mục thành công");
            //O lai trang thung rac
            return RedirectToAction("Trash");
        }
    }
}