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
    public class PostController : Controller
    {
        PostsDAO postsDAO = new PostsDAO();
        LinksDAO linksDAO = new LinksDAO();
        TopicsDAO topicsDAO = new TopicsDAO();

        private MyDBContext db = new MyDBContext();

        // GET: Admin/Post
        public ActionResult Index()
        {
            return View(postsDAO.getList("Index"));//hien thi toan bo danh sach loai SP
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Tim row Post co id cho truoc
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }


        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Post/Create: Them moi mot mau tin
        public ActionResult Create()
        {
            ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            return View();
        }

        // POST: Admin/Post/Create: Them moi mot mau tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Posts posts)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                posts.Slug = XString.Str_Slug(posts.Title);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    //lay phan mo rong cua tap tin
                    {
                        string slug = posts.Slug;
                        string id = posts.Id.ToString();
                        //Chinh sua sau khi phat hien dieu chua dung cua Edit: them Id
                        //ten file = Slug + Id + phan mo rong cua tap tin
                        string imgName = slug + id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;

                        string PathDir = "~/Public/img/post/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //upload hinh
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //xu ly cho muc PostType
                posts.PostType = "post";

                //Xu ly cho muc CreateAt
                posts.CreateAt = DateTime.Now;

                //Xu ly cho muc CreateBy
                posts.CreateBy = Convert.ToInt32(Session["UserId"]);

                //xu ly cho muc Topics
                if (postsDAO.Insert(posts) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = posts.Slug;
                    links.TableId = posts.Id;
                    links.Type = "post";
                    linksDAO.Insert(links);
                }
                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm bài viết thành công");
                return RedirectToAction("Index");
            }
            ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            return View(posts);
        }


        // GET: Admin/Post/Edit/5
        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Topic/Edit/5: Cap nhat mau tin
        public ActionResult Edit(int? id)
        {
            ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }


        // POST: Admin/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Admin/Post/Edit/5: Cap nhat mau tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posts posts)
        {
            if (ModelState.IsValid)
            {
                //Xu ly cho muc Slug
                posts.Slug = XString.Str_Slug(posts.Title);
                //chuyen doi dua vao truong Name de loai bo dau, khoang cach = dau -

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    //lay phan mo rong cua tap tin
                    {
                        string slug = posts.Slug;
                        string id = posts.Id.ToString();
                        //Chinh sua sau khi phat hien dieu chua dung cua Edit: them Id
                        //ten file = Slug + Id + phan mo rong cua tap tin
                        string imgName = slug + id + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        posts.Image = imgName;

                        string PathDir = "~/Public/img/post/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        //upload hinh
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //xu ly cho muc PostType
                posts.PostType = "post";

                //Xu ly cho muc UpdateAt
                posts.UpdateAt = DateTime.Now;

                //Xu ly cho muc UpdateBy
                posts.UpdateBy = Convert.ToInt32(Session["UserId"]);

                //xu ly cho muc Links
                if (postsDAO.Update(posts) == 1)//khi sua du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = posts.Slug;
                    links.TableId = posts.Id;
                    links.Type = "post";
                    linksDAO.Insert(links);
                }
                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Sửa bài viết thành công");
                return RedirectToAction("Index");
            }
            ViewBag.TopList = new SelectList(topicsDAO.getList("Index"), "Id", "Name");
            return View(posts);
        }


        // GET: Admin/Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa thông tin nhà cung cấp thất bại");
                return RedirectToAction("Trash");
            }
            return View(posts);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = postsDAO.getRow(id);

            //Tìm thấy mẫu tin tiến hành xóa
            postsDAO.Delete(posts);

            // Xóa hình ảnh liên quan
            string imagePath = Path.Combine(Server.MapPath("~/Public/img/post"), posts.Image);
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
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            posts.Status = (posts.Status == 1) ? 2 : 1;
            //Cap nhat Update At
            posts.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            posts.UpdateBy = Convert.ToInt32(Session["UserID"]);

            //

            postsDAO.Update(posts);//Xac nhan Update database
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
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai
            posts.Status = 0;
            //Cap nhat Update At
            posts.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            posts.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            postsDAO.Update(posts);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            //Tro ve trang Index
            return RedirectToAction("Index");
        }

        //
        // GET: Admin/Category/Trash= luc thung rac
        public ActionResult Trash()
        {
            return View(postsDAO.getList("Trash"));
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
            Posts posts = postsDAO.getRow(id);
            if (posts == null)
            {
                //Hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi thông tin thất bại");
                return RedirectToAction("Index");

            }
            //Cap nhat trang thai status = 2
            posts.Status = 2;
            //Cap nhat Update At
            posts.UpdateAt = DateTime.Now;
            //Cap nhat Update By
            posts.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Xac nhan Update database
            postsDAO.Update(posts);
            //Hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi thông tin thành công thành công!");
            //Tro ve trang Index
            return RedirectToAction("Trash");// ở lại thùng rác tiếp tục phục hồi or xóa
        }
    }
}
