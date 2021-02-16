using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey.Controllers
{
    public class LoginController : Controller
    {
        SurveySystemEntities2 db = new SurveySystemEntities2();
        //girilen bilgileri dbden kontrol edip doğruysa girişe izin veren kodlar
        public ActionResult Index(string user_name, string user_pass)
        {
            if (user_name == null)
            {
                return View();
            }
            else
            {
                var user = db.User.FirstOrDefault(m => m.user_name == user_name && m.user_pass == user_pass);
                if (user != null)
                {
                    Session["user_name"] = user.user_name;
                    if (user.isAdmin == true)
                    {
                        Session["Admin"] = user.isAdmin;
                        return RedirectToAction("Index", "Admin");
                    }
                    return RedirectToAction("Index", "AnsQues");
                }
                else
                {
                    ViewBag.Error = "Kullanıcı adı veya şifre hatalı";
                    return View();
                }
            }
            
        }
        //kullanıcı kayıt işlemi için gerekli kodlar
        public ActionResult Register(string user_name,string user_pass, User user)        
        {
            if (user_name == null)
            {
                return View();
            }
            else
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
        }
    }
}