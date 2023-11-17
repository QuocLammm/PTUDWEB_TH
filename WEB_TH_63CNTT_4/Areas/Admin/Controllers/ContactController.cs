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
using MyClass.Model.MyClass.Model;
using WEB_TH_63CNTT_4.Library;


namespace WEB_TH_63CNTT_4.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        ContactsDAO contactsDAO = new ContactsDAO();

        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View(contactsDAO.getList("Index"));
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
            Contacts contact = contactsDAO.getRow(id);
            if (contact == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        /// //////////////////////////////////////////////
        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            //ViewBag.orderlist = new SelectList(contactsDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Supplier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contacts contact)
        {
            if (ModelState.IsValid)
            {
                // xử lý tự động
                //Xử lý tự động cho các trường sau:
                //---Create At
                contact.CreateAt = DateTime.Now;
                //Update at
                contact.UpdateAt = DateTime.Now;
                //Update by
                contact.UpdateBy = Convert.ToInt32(Session["UserID"]);


                contactsDAO.Insert(contact);// Thêm mới
                //Hien thi thong bao
                TempData["message"] = new XMessage("success", "Thêm mới nhà cung cấp thành công!");
                return RedirectToAction("Index");

            }
            //ViewBag.orderlist = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(contact);
        }

        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            //ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp!");
                return RedirectToAction("Index");
            }
            Contacts contact = contactsDAO.getRow(id);
            if (contact == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp!");
                return RedirectToAction("Index"); ;
            }
            return View(contact);
        }

        // POST: Admin/Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contacts contact)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc UpdateAt
                contact.UpdateAt = DateTime.Now;

                //Xu ly cho muc UpdateBy
                contact.UpdateBy = Convert.ToInt32(Session["UserId"]);

                //
                contactsDAO.Update(contact);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Sửa danh mục thành công");
                return RedirectToAction("Index");
            }
            return View(contact);
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
            Contacts contact = contactsDAO.getRow(id);
            if (contact == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            return View(contact);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contacts contacts = contactsDAO.getRow(id);

            //Tìm thấy mẫu tin tiến hành xóa
            contactsDAO.Delete(contacts);



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
            Contacts contact = contactsDAO.getRow(id);

            if (contact == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            contact.Status = (contact.Status == 1) ? 2 : 1;
            //Cap nhat Update At
            contact.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            contact.UpdateBy = Convert.ToInt32(Session["UserID"]);

            contactsDAO.Update(contact);//Xac nhan Update database
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
            Contacts contact = contactsDAO.getRow(id);
            if (contact == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            contact.Status = 0;
            //Cap nhat Update At
            contact.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            contact.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            contactsDAO.Update(contact);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            //Tro ve trang Index
            return RedirectToAction("Index");
        }

        //
        // GET: Admin/Category/Trash= luc thung rac
        public ActionResult Trash()
        {
            return View(contactsDAO.getList("Trash"));
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
            Contacts contact = contactsDAO.getRow(id);
            if (contact == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi thông tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai status = 2
            contact.Status = 2;
            //Cap nhat Update At
            contact.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            contact.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            contactsDAO.Update(contact);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi thông tin thành công thành công!");
            //Tro ve trang Index
            return RedirectToAction("Trash");// ở lại thùng rác tiếp tục phục hồi or xóa
        }

    }
}
