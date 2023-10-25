using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using WEB_TH_63CNTT_4.Library;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace WEB_TH_63CNTT_4.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("Index"));
        }

        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        /// //////////////////////////////////////////////
        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.orderlist = new SelectList(suppliersDAO.getList("Index"),"Order", "Name");
            return View();
        }

        // POST: Admin/Supplier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                // xử lý tự động
                //Xử lý tự động cho các trường sau:
                //---Create At
                suppliers.CreateAt = DateTime.Now;
                //---Create By
                suppliers.CreateBy = Convert.ToInt32(Session["UserID"]);
                // Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //
                //Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order++;
                }
                //Update at
                suppliers.UpdateAt = DateTime.Now;
                //Update by
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                suppliersDAO.Insert(suppliers);// Thêm mới
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Thêm mới nhà cung cấp thành công!");
                return RedirectToAction("Index");
                
            }
            ViewBag.orderlist = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.orderlist = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật thông tin nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật thông tin nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                // Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);

                //Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order++;
                }
                // Update at
                suppliers.UpdateAt = DateTime.Now;
                //Update by
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //xóa file
                        if (suppliers.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
                            System.IO.File.Delete(PathFile);
                        }
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                suppliersDAO.Update(suppliers);//Cap nhat database
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Cập nhật thông tin nhà cung cấp thành công!");
                return RedirectToAction("Index");

            }
            ViewBag.orderlist = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = suppliersDAO.getRow(id);
            //Tìm thấy mẫu tin tiến hành xóa
            suppliersDAO.Delete(suppliers);

            // Xóa hình ảnh liên quan
            string imagePath = Path.Combine(Server.MapPath("~/Public/img/supplier"), suppliers.Image);
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
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            suppliers.Status = (suppliers.Status == 1) ? 2 : 1;
            //Cap nhat Update At
            suppliers.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            
            suppliersDAO.Update(suppliers);//Xac nhan Update database
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
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            suppliers.Status = 0;
            //Cap nhat Update At
            suppliers.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            suppliersDAO.Update(suppliers);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            //Tro ve trang Index
            return RedirectToAction("Index");
        }

        //
        // GET: Admin/Category/Trash= luc thung rac
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));
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
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi thông tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai status = 2
            suppliers.Status = 2;
            //Cap nhat Update At
            suppliers.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            suppliersDAO.Update(suppliers);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi thông tin thành công thành công!");
            //Tro ve trang Index
            return RedirectToAction("Trash");// ở lại thùng rác tiếp tục phục hồi or xóa
        }

    }
}
