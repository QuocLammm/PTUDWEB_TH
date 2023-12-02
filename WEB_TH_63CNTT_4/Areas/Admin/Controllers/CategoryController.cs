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
        LinksDAO linksDAO = new LinksDAO();
        // GET: Admin/Category

        //INDEX
        public ActionResult Index()
        {
            List<Categories> ls = categoriesDAO.getList("Index");
            return View(categoriesDAO.getList("Index"));
        }


        // GET: Admin/Category/Details
        public ActionResult Details(int? id)
        {
            List<Categories> ls = categoriesDAO.getList("Index");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");
            }
            if (categories.ParentId == 0)
            {
                ViewBag.Name = categories.Name;
            }
            else
            {
                foreach (var i in ls)
                {
                    if (i.Id == categories.ParentId)
                    {
                        ViewBag.Name = i.Name;
                    }
                }
            }

            return View(categories);
        }

        // GET: Admin/Category/Create
        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.Catlist = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");
            ViewBag.Orderlist = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }

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
                categories.CreateBy = Convert.ToInt32(Session["UserId"]);
                //Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentID
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
                    categories.Order += 1;
                }
                //Update at
                categories.UpdateAt = DateTime.Now;
                //Update by
                categories.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //xu ly cho muc Topics
                if (categoriesDAO.Insert(categories) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = categories.Slug;
                    links.TableId = categories.Id;
                    links.Type = "category";
                    linksDAO.Insert(links);
                }
                //hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Tạo mới loại sản phẩm thành công!");
                return RedirectToAction("Index");
            }
            ViewBag.Catlist = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");
            ViewBag.Orderlist = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }
        //-----------------------------------------------------------------------------
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Catlist = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");
            ViewBag.Orderlist = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại!");
                return RedirectToAction("Index");
            }
            return View(categories);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động cho các trường sau:
                //Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentID
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
                    categories.Order += 1;
                }
                //Update at
                categories.UpdateAt = DateTime.Now;
                //Update by
                categories.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //hiển thị thông báo thành công
                TempData["message"] = new XMessage("success", "Cập nhật thông tin thành công!");
                //cập nhật links
                if (categoriesDAO.Update(categories) == 1)
                {
                    //Neu trung khop thong tin: Type = category va TableID = categories.ID
                    Links links = linksDAO.getRow(categories.Id, "category");
                    //cap nhat lai thong tin
                    links.Slug = categories.Slug;
                    linksDAO.Update(links);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Catlist = new SelectList(categoriesDAO.getList("Index"), "ID", "Name");
            ViewBag.Orderlist = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        // GET: Admin/Category/Delete/
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Trash");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Trash");
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            //tim thay mau tin => xoa
            categoriesDAO.Delete(categories);
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            return RedirectToAction("Trash");
        }

        // POST: Admin/Category/Status
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //cap nhat update at
            categories.UpdateAt = DateTime.Now;
            //cap nhat update by
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }

        // POST: Admin/Category/DelTrash
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            categories.Status = 0;
            //cap nhat update at
            categories.UpdateAt = DateTime.Now;
            //cap nhat update by
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");

        }
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
        }

        // POST: Admin/Category/Undo
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai status =2
            categories.Status = 2;
            //cap nhat update at
            categories.UpdateAt = DateTime.Now;
            //cap nhat update by
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Trash");
        }

    }
}
