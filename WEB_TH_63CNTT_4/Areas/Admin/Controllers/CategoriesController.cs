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

namespace WEB_TH_63CNTT_4.Areas.Admin.Controllers
{

    public class CategoriesController : Controller
    {

        CategoriesDAO categoriesDAO = new CategoriesDAO();
        //-------------------------------------------------------------------------------
        // GET: Admin/Categories/Index
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }
        //-------------------------------------------------------------------------------
        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                categories.CreateAt = DateTime.Now;
                categories.UpdateAt = DateTime.Now;
                categories.CreateBy = Convert.ToInt32(Session["UserId"]);
                categories.UpdateBy = Convert.ToInt32(Session["UserId"]);
                categoriesDAO.Insert(categories);
                return RedirectToAction("Index");
            }

            return View(categories);
        }

        //-------------------------------------------------------------------------------
        // GET: Admin/Categories/Details/5
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

        //-------------------------------------------------------------------------------
        // GET: Admin/Category/Edit/5: Cap nhat mau tin
        public ActionResult Edit(int? id)
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

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                categoriesDAO.Update(categories);
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Delete/5:Xoa mot mau tin ra khoi CSDL
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

        // POST: Admin/Category/Delete/5:Xoa mot mau tin ra khoi CSDL
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            categoriesDAO.Delete(categories);
            return RedirectToAction("Index");
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        //// GET: Admin/Category/Staus/5:Thay doi trang thai cua mau tin
        //public ActionResult Status(int? id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction("Index", "Categories");
        //    }
        //    return RedirectToAction("Index", "Categorí");
        //}

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5:Thay doi trang thai cua mau tin = 0
        //public ActionResult DelTrash(int? id)
        //{
        //    //khi nhap nut thay doi Status cho mot mau tin
        //    Categories categories = categoriesDAO.getRow(id);

        //    //thay doi trang thai Status tu 1,2 thanh 0
        //    categories.Status = 0;

        //    //cap nhat gia tri cho UpdateAt/By
        //    categories.UpdateBy = Convert.ToInt32(Session["UserId"].ToString());
        //    categories.UpdateAt = DateTime.Now;

        //    //Goi ham Update trong CategoryDAO
        //    categoriesDAO.Update(categories);

        //    //Thong bao thanh cong
        //    TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");

        //    //khi cap nhat xong thi chuyen ve Index
        //    return RedirectToAction("Index", "Category");
        //}

    }
}