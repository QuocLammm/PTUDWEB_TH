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
    public class TopicController : Controller
    {
        TopicsDAO topicdao = new TopicsDAO();
        LinksDAO linksDAO = new LinksDAO();

        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View(topicdao.getList("Index"));
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
            Topics topic = topicdao.getRow(id);
            if (topicdao == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        /// //////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Create: Them moi mot mau tin
        public ActionResult Create()
        {
            ViewBag.ListTopic = new SelectList(topicdao.getList("Index"), "Id", "Name");
            ViewBag.OrderTopic = new SelectList(topicdao.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Topic/Create: Them moi mot mau tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topics topics)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                topics.Slug = XString.Str_Slug(topics.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //Xu ly cho muc ParentId
                if (topics.ParentId == null)
                {
                    topics.ParentId = 0;
                }

                //Xu ly cho muc Order
                if (topics.Order == null)
                {
                    topics.Order = 1;
                }
                else
                {
                    topics.Order = topics.Order + 1;
                }

                //Xu ly cho muc CreateAt
                topics.CreateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                topics.CreateBy = Convert.ToInt32(Session["UserID"]);
                //Update at
                topics.UpdateAt = DateTime.Now;
                //Update by
                topics.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //xu ly cho muc Topics
                if (topicdao.Insert(topics) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = topics.Slug;
                    links.TableId = topics.Id;
                    links.Type = "topic";
                    linksDAO.Insert(links);
                }
                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm chủ đề thành công");
                return RedirectToAction("Index");

            }
            ViewBag.ListTopic = new SelectList(topicdao.getList("Index"), "Id", "Name");
            ViewBag.OrderTopic = new SelectList(topicdao.getList("Index"), "Order", "Name");
            return View(topics);
        }


        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListTopic = new SelectList(topicdao.getList("Index"), "Id", "Name");
            ViewBag.OrderTopic = new SelectList(topicdao.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp!");
                return RedirectToAction("Index");
            }
            Topics topic = topicdao.getRow(id);
            if (topic == null)
            {
                //hiện thị thông báo
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp!");
                return RedirectToAction("Index"); ;
            }
            return View(topic);
        }

        // POST: Admin/Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topics topics)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                topics.Slug = XString.Str_Slug(topics.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //Xu ly cho muc Order
                if (topics.Order == null)
                {
                    topics.Order = 1;
                }
                else
                {
                    topics.Order = topics.Order + 1;
                }
                //Xu ly cho muc CreateAt
                topics.CreateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                topics.CreateBy = Convert.ToInt32(Session["UserID"]);
                //Update at
                topics.UpdateAt = DateTime.Now;
                //Update by
                topics.UpdateBy = Convert.ToInt32(Session["UserID"]);


                //Cap nhat du lieu, sua them cho phan Links phuc vu cho Topics
                if (topicdao.Update(topics) == 1)
                {
                    //Neu trung khop thong tin: Type = category va TableID = categories.ID
                    Links links = linksDAO.getRow(topics.Id, "topic");
                    //cap nhat lai thong tin
                    links.Slug = topics.Slug;
                    linksDAO.Update(links);
                }
;

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Sửa danh mục thành công");
                return RedirectToAction("Index");
            }
            return View(topics);
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
            Topics topic = topicdao.getRow(id);
            if (topic == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            return View(topic);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topics topic = topicdao.getRow(id);

            //Tìm thấy mẫu tin tiến hành xóa
            topicdao.Delete(topic);

            

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
            Topics topic = topicdao.getRow(id);
            if (topic == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            topic.Status = (topic.Status == 1) ? 2 : 1;
            //Cap nhat Update At
            topic.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            topic.UpdateBy = Convert.ToInt32(Session["UserID"]);

            topicdao.Update(topic);//Xac nhan Update database
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
            Topics topic = topicdao.getRow(id);
            if (topic == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            topic.Status = 0;
            //Cap nhat Update At
            topic.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            topic.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            topicdao.Update(topic);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            //Tro ve trang Index
            return RedirectToAction("Index");
        }

        //
        // GET: Admin/Category/Trash= luc thung rac
        public ActionResult Trash()
        {
            return View(topicdao.getList("Trash"));
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
            Topics topic = topicdao.getRow(id);
            if (topic == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi thông tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai status = 2
            topic.Status = 2;
            //Cap nhat Update At
            topic.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            topic.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            topicdao.Update(topic);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi thông tin thành công thành công!");
            //Tro ve trang Index
            return RedirectToAction("Trash");// ở lại thùng rác tiếp tục phục hồi or xóa
        }

    }
}
