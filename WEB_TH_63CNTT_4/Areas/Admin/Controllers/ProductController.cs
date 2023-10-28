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
        ProductsDAO productsDAO = new ProductsDAO();

        // GET: Admin/Product
        public ActionResult Index()
        {
            return View(productsDAO.getList("Index"));
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            //ViewBag.productlist = new SelectList(productsDAO.getList("Index"),"Id","Name");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Products products)
        {
            if (ModelState.IsValid)
            {
                // xử lý tự động
                //Xử lý tự động cho các trường sau:
                //---Create At
                products.CreateAt = DateTime.Now;
                //---Create By
                products.CreateBy = Convert.ToInt32(Session["UserID"]);
                // Slug
                products.Slug = XString.Str_Slug(products.Name);
                //
      
                
                //Update at
                products.UpdateAt = DateTime.Now;
                //Update by
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                productsDAO.Insert(products);// Thêm mới
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Thêm mới nhà cung cấp thành công!");
                return RedirectToAction("Index");
            }

            return View(products);
        }

        //// GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            //ViewBag.productlist = new SelectList(productsDAO.getList("Index"), "Id", "Name");
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật thông tin nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật thông tin nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                // Slug
                products.Slug = XString.Str_Slug(products.Name);

                ////Order
                //if (products.Order == null)
                //{
                //    suppliers.Order = 1;
                //}
                //else
                //{
                //    suppliers.Order++;
                //}

                // Update at
                products.UpdateAt = DateTime.Now;
                //Update by
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //xóa file
                        if (products.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), products.Image);
                            System.IO.File.Delete(PathFile);
                        }
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                productsDAO.Update(products);
                TempData["message"] = new XMessage("success", "Cập nhật thông tin nhà cung cấp thành công!");
                return RedirectToAction("Index");
            }
            //ViewBag.productlist = new SelectList(productsDAO.getList("Index"), "Id", "Name");
            return View(products);
        }

        //// GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            return View(products);
        }
        
        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = productsDAO.getRow(id);
        //Tìm thấy mẫu tin tiến hành xóa
            productsDAO.Delete(products);

        // Xóa hình ảnh liên quan
            string imagePath = Path.Combine(Server.MapPath("~/Public/img/product"), products.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }//kết thúc xóa ảnh 

            //Hiển thị thông báo
            TempData["message"] = new XMessage("success", "Xóa thông tin nhà cung cấp thành công!");
            return RedirectToAction("Trash");// ở lại thùng rác
        }

        //Status
        //// GET: Admin/Category/Edit/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            products.Status = (products.Status == 1) ? 2 : 1;
            //Cap nhat Update At
            products.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);

            productsDAO.Update(products);//Xac nhan Update database
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công!");
            //Tro ve trang Index
            return RedirectToAction("Index");
        }
        ///////////////////////
        /////// GET: Admin/Category/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            products.Status = 0;
            //Cap nhat Update At
            products.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            productsDAO.Update(products);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            //Tro ve trang Index
            return RedirectToAction("Index");
        }

        //
        // GET: Admin/Category/Trash= luc thung rac
        public ActionResult Trash()
        {
            return View(productsDAO.getList("Trash"));
        }

        ///////
        /////Status
        //// GET: Admin/Category/Edit/5
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi thông tin thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi thông tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai status = 2
            products.Status = 2;
            //Cap nhat Update At
            products.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            productsDAO.Update(products);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi thông tin thành công thành công!");
            //Tro ve trang Index
            return RedirectToAction("Trash");// ở lại thùng rác tiếp tục phục hồi or xóa
        }
    }   
}
