using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using WEB_TH_63CNTT_4.Library;

namespace WEB_TH_63CNTT_4.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {

        CategoriesDAO categoriesDAO = new CategoriesDAO();
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        ///
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        //// GET: Admin/Category/Create
        public ActionResult Create()
        {

            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //---Create At
                categories.CreateAt = DateTime.Now;
                //---Create By
                categories.CreateBy = Convert.ToInt32(Session["UserID"]);
                // Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                // ParentId
                if(categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if(categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order++;
                }

                //Update at
                categories.UpdateAt = DateTime.Now;
                //Update by
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);

                // hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success","Tạo mới loại sản phẩm thành công!");


                categoriesDAO.Insert(categories);
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }


        //// GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật thông tin thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật thông tin thất bại");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                
                // Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                // ParentId
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order++;
                }

                //Update at
                categories.UpdateAt = DateTime.Now;
                //Update by
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Cap nhat database
                categoriesDAO.Update(categories);
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Cập nhật thông tin thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        //// GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }
        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            categoriesDAO.Delete(categories);
            return RedirectToAction("Index");
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
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //Cap nhat Update At
            categories.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            categoriesDAO.Update(categories);
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
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            categories.Status = 0;
            //Cap nhat Update At
            categories.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            categoriesDAO.Update(categories);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            //Tro ve trang Index
            return RedirectToAction("Index");
        }

        //
        // GET: Admin/Category/Trash= luc thung rac
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
        }
    }
}
