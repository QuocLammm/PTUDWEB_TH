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
using System.IO;
using System.Web.Services.Description;


namespace WEB_TH_63CNTT_4.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        SlidersDAO slidersDAO = new SlidersDAO();
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Supplier = INDEX
        public ActionResult Index()
        {
            return View(slidersDAO.getList("Index"));//hien thi toan bo danh sach Slider
        }


        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/ Sliders /Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Tim row Sliders co id cho truoc
            Sliders sliders = slidersDAO.getRow(id);
            if (sliders == null)
            {
                return HttpNotFound();
            }
            return View(sliders);
        }


        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(slidersDAO.getList("Index"), "Order", "Name");
            return View();
        }

        // POST: Admin/Slider/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sliders sliders)
        {
            if (ModelState.IsValid)
            {
                ////---Create At
                //sliders.CreateAt = DateTime.Now;
                ////---Create By
                //sliders.CreateBy = Convert.ToInt32(Session["UserID"]);
                //Xu ly cho muc Order
                if (sliders.Order == null)
                {
                    sliders.Order = 1;
                }
                else
                {
                    sliders.Order = sliders.Order + 1;
                }

                //Xu ly cho muc Slug
                string slug = XString.Str_Slug(sliders.Name);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    //lay phan mo rong cua tap tin
                    {
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        sliders.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/slider/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Xu ly cho muc CreateAt
                sliders.CreateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                sliders.CreateBy = Convert.ToInt32(Session["UserID"]);

                slidersDAO.Insert(sliders);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm danh mục thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(slidersDAO.getList("Index"), "Order", "Name");
            return View(sliders);
        }


        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Slider/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.OrderList = new SelectList(slidersDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            Sliders sliders = slidersDAO.getRow(id);
            if (sliders == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Index");
            }
            return View(sliders);
        }


        // POST: Admin/Slider/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sliders sliders)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                string slug = XString.Str_Slug(sliders.Name);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -
                //Xu ly cho muc Order
                if (sliders.Order == null)
                {
                    sliders.Order = 1;
                }
                else
                {
                    sliders.Order = sliders.Order + 1;
                }

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    //lay phan mo rong cua tap tin
                    {
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + sliders.Id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        sliders.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/slider/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);

                        //cap nhat thi phai xoa file cu
                        //Xoa file
                        if (sliders.Image != null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), sliders.Image);
                            System.IO.File.Delete(DelPath);
                        }

                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Xu ly cho muc UpdateAt
                sliders.CreateAt = DateTime.Now;

                //Xu ly cho muc UpdateBy
                sliders.CreateBy = Convert.ToInt32(Session["UserID"]);
                //Xu ly cho muc UpdateAt
                sliders.UpdateAt = DateTime.Now;

                //Xu ly cho muc UpdateBy
                sliders.UpdateBy = Convert.ToInt32(Session["UserID"]);

                slidersDAO.Update(sliders);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Sửa danh mục thành công");
                return RedirectToAction("Index");
            }
            return View(sliders);
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
            Sliders sliders = new Sliders();
            if (sliders == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            return View(sliders);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sliders sliders = slidersDAO.getRow(id);

            //Tìm thấy mẫu tin tiến hành xóa
            slidersDAO.Delete(sliders);

            // Xóa hình ảnh liên quan
            string imagePath = Path.Combine(Server.MapPath("~/Public/img/slider"), sliders.Image);
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
            Sliders sliders = slidersDAO.getRow(id);
            if (sliders == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            sliders.Status = (sliders.Status == 1) ? 2 : 1;
            //Cap nhat Update At
            sliders.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            sliders.UpdateBy = Convert.ToInt32(Session["UserID"]);

            slidersDAO.Update(sliders);//Xac nhan Update database
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
            Sliders sliders = slidersDAO.getRow(id);
            if (sliders == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            sliders.Status = 0;
            //Cap nhat Update At
            sliders.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            sliders.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            slidersDAO.Update(sliders);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            //Tro ve trang Index
            return RedirectToAction("Index");
        }

        //
        // GET: Admin/Category/Trash= luc thung rac
        public ActionResult Trash()
        {
            return View(slidersDAO.getList("Trash"));
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
            Sliders sliders = slidersDAO.getRow(id);
            if (sliders == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi thông tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai status = 2
            sliders.Status = 2;
            //Cap nhat Update At
            sliders.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            sliders.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            slidersDAO.Update(sliders) ;
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi thông tin thành công thành công!");
            //Tro ve trang Index
            return RedirectToAction("Trash");// ở lại thùng rác tiếp tục phục hồi or xóa
        }
    }
}
