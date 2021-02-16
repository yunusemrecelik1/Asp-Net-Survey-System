using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey.Controllers
{
    public class BaseController : Controller
    {
        //giriş yapan kişinin admin olup olmadığını kontrol edip kısıtlamalar dahilinde girebileceği tablolara karar veren kodlar

        public SurveySystemEntities2 db = new SurveySystemEntities2();
        public string user_name { get; set; }
        public bool isAdmin { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["user_name"] == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
            
            else
            {
                if (Session["Admin"] != null)
                {                    
                    isAdmin = true;
                }
                else
                {                 
                    isAdmin = false;
                }
                user_name = Session["user_name"].ToString();
            }
            base.OnActionExecuting(filterContext);
        }
    }
}