using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey.Controllers
{
    public class HomeController : BaseController
    {
        
        int i = 0;        
        public ActionResult Index()
        {
            return View();
        }
        //soru ekleme kodları
        public ActionResult AddQues(Question question)
        {
           
            if (question.QuestionLine != null)
            {
                question.CreateBy = user_name;
                db.Question.Add(question);
                db.SaveChanges();
                i = question.id;
                question.QuestionOptionID = i;               
                db.SaveChanges();
                return RedirectToAction("Option");
            }
            else
            {
                return View();
            }
        }
        //şık ekleme kodları
        [HttpGet]
        public ActionResult Option(Question question)
        {
            var v = db.Question.OrderByDescending(t => t.QuestionOptionID).First();
            ViewBag.yolla = v.QuestionOptionID;
            var model = db.QuestionOption.ToList();
            return View(model);
            
        }

        [HttpPost]
        public ActionResult Option(QuestionOption questionOption, Question question)
        {                      
                db.QuestionOption.Add(questionOption);
                var v = db.Question.OrderByDescending(t => t.QuestionOptionID).First();
                ViewBag.yolla = v.QuestionOptionID;
                db.SaveChanges();
            var model = db.QuestionOption.ToList();
            return View(model);
                      
        }
    }
}