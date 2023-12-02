using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using WEB_TH_63CNTT_4.Library;

namespace WEB_TH_63CNTT_4.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        //Goi 4 lop DAO can thuc thi
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        TopicsDAO topicsDAO = new TopicsDAO();
        PostsDAO postsDAO = new PostsDAO();
        MenusDAO menusDAO = new MenusDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();//neu thich thi lam

        /////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Menu
        public ActionResult Index()
        {
            ViewBag.CatList = categoriesDAO.getList("Index");//select * from Categories voi Status !=0
            ViewBag.TopList = topicsDAO.getList("Index");//select * from Topics voi Status !=0
            ViewBag.PosList = postsDAO.getList("Index", "Page");//select * from Posts voi Status !=0
            ViewBag.SupList = suppliersDAO.getList("Index");
            List<Menus> menu = menusDAO.getList("Index");//select * from Menus voi Status !=0
            return View("Index", menu);//truyen menu duoi dang model
        }
        // POST: Admin/Menu/Create
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //-------------------------Category------------------------//
            //Xu ly cho nút ThemCategory ben Index
            if (!string.IsNullOrEmpty(form["ThemCategory"]))//nut ThemCategory duoc nhan
            {
                if (!string.IsNullOrEmpty(form["nameCategory"]))//check box được nhấn
                {
                    var listitem = form["nameCategory"];
                    //chuyen danh sach thanh dang mang: vi du 1,2,3,...
                    var listarr = listitem.Split(',');//cat theo dau ,
                    foreach (var row in listarr)//row = id cua các mau tin
                    {
                        int id = int.Parse(row);//ep kieu int
                        //lay 1 ban ghi
                        Categories categories = categoriesDAO.getRow(id);
                        //tao ra menu
                        Menus menu = new Menus();
                        menu.Name = categories.Name;
                        menu.Link = categories.Slug;
                        menu.TableID = categories.Id;
                        menu.TypeMenu = "category";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.CreateAt = DateTime.Now;
                        menu.Status = 2;//chưa xuất bản
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu danh mục thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục loại sản phẩm");
                }
            }

            //Xu ly cho nút ThemTopic ben Index
            if (!string.IsNullOrEmpty(form["ThemTopic"]))//nut ThemCategory duoc nhan
            {
                if (!string.IsNullOrEmpty(form["nameTopic"]))//check box được nhấn
                {
                    var listitem = form["nameTopic"];
                    //chuyen danh sach thanh dang mang: vi du 1,2,3,...
                    var listarr = listitem.Split(',');//cat theo dau ,
                    foreach (var row in listarr)//row = id cua các mau tin
                    {
                        int id = int.Parse(row);//ep kieu int
                                                //lay 1 ban ghi
                        Topics topics = topicsDAO.getRow(id);
                        //tao ra menu
                        Menus menu = new Menus();
                        menu.Name = topics.Name;
                        menu.Link = topics.Slug;
                        menu.TableID = topics.Id;
                        menu.TypeMenu = "topic";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.CreateAt = DateTime.Now;
                        menu.Status = 2;//chưa xuất bản
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu chủ đề bài viết thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục chủ đề bài viết");
                }
            }

            //-------------------------Page------------------------//
            //Xử lý cho nut Thempage ben Index
            if (!string.IsNullOrEmpty(form["ThemPage"]))
            {
                if (!string.IsNullOrEmpty(form["namePage"]))//check box được nhấn tu phia Index
                {
                    var listitem = form["namePage"];
                    //chuyen danh sach thanh dang mang: vi du 1,2,3,...
                    var listarr = listitem.Split(',');//cat theo dau ,
                    foreach (var row in listarr)//row = id cua các mau tin
                    {
                        int id = int.Parse(row);//ep kieu int
                        Posts post = postsDAO.getRow(id);
                        //tao ra menu
                        Menus menu = new Menus();
                        menu.Name = post.Title;
                        menu.Link = post.Slug;
                        menu.TableID = post.Id;
                        menu.TypeMenu = "page";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.CreateAt = DateTime.Now;
                        menu.Status = 2;//chưa xuất bản
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm menu bài viết thành công");
                }
                else//check box chưa được nhấn
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục trang đơn");
                }
            }
            //Them Nha cung cap
            if (!string.IsNullOrEmpty(form["ThemSupplier"]))
            {
                //kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameSupplier"]))
                {
                    var listitem = form["nameSupplier"];
                    //chuyen danh sach thanh dang mang: 1,2,3,4...
                    var listarr = listitem.Split(',');//ngat mang thanh tung phan tu cach nhau boi dau ,
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);//ep kieu int
                        //lay 1 ban ghi
                        Suppliers suppliers = suppliersDAO.getRow(id);
                        //ta ra menu
                        Menus menu = new Menus();
                        menu.Name = suppliers.Name;
                        menu.Link = suppliers.Slug;
                        menu.TypeMenu = "supplier";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.Order = 0;
                        menu.CreateAt = DateTime.Now;
                        menu.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //tam thoi chua xuat ban
                        //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn nhà cung cấp");
                }
            }
            //-------------------------Custom------------------------//
            //Xử lý cho nút ThemCustom ben Index
            if (!string.IsNullOrEmpty(form["ThemCustom"]))
            {
                if (!string.IsNullOrEmpty(form["name"]) && !string.IsNullOrEmpty(form["link"]))
                {
                    //tao ra menu
                    Menus menus = new Menus();
                    menus.Name = form["name"];
                    menus.Link = form["link"];
                    menus.TypeMenu = "custom";
                    menus.Position = form["Position"];
                    menus.ParentID = 0;
                    menus.Order = 0;
                    menus.CreateBy = Convert.ToInt32(Session["UserID"].ToString());
                    menus.CreateAt = DateTime.Now;
                    menus.Status = 2;//chưa xuất bản
                    menusDAO.Insert(menus);

                    TempData["message"] = new XMessage("success", "Thêm danh mục thành công");
                }

                else//check box chưa được nhấn
                {
                    TempData["message"] = new XMessage("danger", "Chưa đủ thông tin cho mục tùy chọn Menu");
                }
            }

            return RedirectToAction("Index", "Menu");
        }

        // GET: Admin/Menu/Status
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật không thành công");
                return RedirectToAction("Index");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật không thành công");
                return RedirectToAction("Index");
            }
            menus.UpdateAt = DateTime.Now;
            menus.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
            menus.Status = (menus.Status == 1) ? 2 : 1;
            menusDAO.Update(menus);
            TempData["message"] = new XMessage("success", "Cập nhật thành công");
            return RedirectToAction("Index");
        }

        // Admin/Menus/Detail: Hien thi mot mau tin
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                return HttpNotFound();
            }
            return View(menus);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // Admin/Menu/Edit: Thay doi mot mau tin
        public ActionResult Edit(int? id)
        {

            ViewBag.ParentList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Menus menus = menusDAO.getRow(id);

            if (menus == null)
            {
                return HttpNotFound();
            }
            return View("Edit", menus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menus menus)
        {
            if (ModelState.IsValid)
            {

                if (menus.ParentID == null)
                {
                    menus.ParentID = 0;
                }
                if (menus.Order == null)
                {
                    menus.Order = 1;
                }
                else
                {
                    menus.Order += 1;
                }
                //Xy ly cho muc UpdateAt
                menus.CreateAt = DateTime.Now;

                //Xy ly cho muc UpdateBy
                menus.CreateBy = Convert.ToInt32(Session["UserID"]);

                //Xy ly cho muc UpdateAt
                menus.UpdateAt = DateTime.Now;

                //Xy ly cho muc UpdateBy
                menus.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật thành công");

                //Cap nhat du lieu
                menusDAO.Update(menus);

                return RedirectToAction("Index");
            }
            ViewBag.ParentList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");
            return View(menus);
        }

        // GET: Admin/Menu/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            //khi nhap nut thay doi Status cho mot mau tin
            Menus menus = menusDAO.getRow(id);

            //thay doi trang thai Status tu 1,2 thanh 0
            menus.Status = 0;

            //cap nhat gia tri cho UpdateAt/By
            menus.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
            menus.UpdateAt = DateTime.Now;
            menusDAO.Update(menus);
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa Menu thành công");
            return RedirectToAction("Index", "Menu");
        }


        // GET: Admin/Menus/Trash/5
        public ActionResult Trash(int? id)
        {
            return View(menusDAO.getList("Trash"));
        }
        // GET: Admin/Menu/Undo/
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi menu thất bại");
                //chuyen huong trang
                return RedirectToAction("Index", "Page");
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi menu thất bại");
                return RedirectToAction("Index");
            }
            menus.Status = 2;

            //cap nhat gia tri
            menus.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
            menus.UpdateAt = DateTime.Now;
            menusDAO.Update(menus);
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi menu thành công");
            return RedirectToAction("Trash");
        }
        // GET: Admin/Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                return HttpNotFound();
            }
            return View(menus);
        }

        // POST: Admin/Menu/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menus menus = menusDAO.getRow(id);
            menusDAO.Delete(menus);

            TempData["message"] = new XMessage("success", "Xóa menu thành công");
            return RedirectToAction("Trash");
        }


    }
}