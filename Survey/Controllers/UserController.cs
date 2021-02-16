using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey.Controllers
{
    public class UserController : BaseController
    {
        //admin tarafında kullanıcı tabloları için yapılan işlemler

        // GET: User
        public ActionResult Index()
        {

            if (isAdmin == true)
            {
                var model = db.User.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        public ActionResult AddUser(User user)
        {
            if (isAdmin == true)
            {     
                if(user.user_name != null) {   
                    db.User.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        public ActionResult AuthAdmin(int? id)
        {
            if (isAdmin == true)
            {
                if (id == null || id == 0)
                {
                    return HttpNotFound();
                }
                var user = db.User.Find(id);
                user.isAdmin = !user.isAdmin;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        public ActionResult DeleteUser(int? id)
        {
            if (isAdmin == true)
            {
                if (id == null || id == 0)
                {
                    return HttpNotFound();
                }
                var user = db.User.Find(id);         
                db.User.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        public ActionResult UserUpdate(int? id)
        {
            if (isAdmin == true)
            {
                if (id == null || id == 0)
                {
                    return HttpNotFound();
                }
                var model = db.User.Find(id);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        [HttpPost]
        public ActionResult UserUpdate(User user)
        {
            if (isAdmin == true)
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.Entry(user).Property(e => e.isAdmin).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
    }
}