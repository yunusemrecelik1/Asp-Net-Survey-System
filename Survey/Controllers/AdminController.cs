using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey.Controllers
{
    public class AdminController : BaseController
    {
        int i = 0;
        //admin sayfasına tüm soruları döndürür. view tarafında foreach ile alır ekrana yazdırırız
        public ActionResult Index()
        {
            if (isAdmin==true)
            {
                var model = db.Question.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index","AnsQues");
            }
            
        }
        //şık sayfasına seçilen sorunun şıklarını yazdırır.viewbag.gonderi toplamın sonucunu yollayıp yüzdelik işlemlerde kullanırız
        public ActionResult OptionScreen(int? id)
        {
            if (isAdmin==true)
            {
                if (id == null || id == 0)
                {
                    return HttpNotFound();
                }
                var model = (from x in db.QuestionOption
                             where x.QuestionOptionID == id 
                             select x).ToList();
                var model2 = Convert.ToInt32((from t in db.QuestionOption
                                              where t.QuestionOptionID == id
                                              select t.Total).Sum());
                ViewBag.gonder = model2;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        //seçilen sorunun güncellemesini yapar güncelleme işlemi yapılırken questionoptionid ile kimin tarafından yapıldığını değiştirmez
        public ActionResult UpdateScreen(int? id)
        {
            if (isAdmin==true)
            {
                if (id == null || id == 0)
                {
                    return HttpNotFound();
                }
                var model = db.Question.Find(id);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        [HttpPost]
        public ActionResult UpdateScreen(Question question)
        {
            if (isAdmin == true)
            {
                db.Entry(question).State = System.Data.Entity.EntityState.Modified;
                db.Entry(question).Property(e => e.QuestionOptionID).IsModified = false;
                db.Entry(question).Property(e => e.CreateBy).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        //view tarafında jquery ile yapılan silme işleminin controller tarafındaki kodları
        public ActionResult Delete(int? id)
        {
            if (isAdmin==true)
            {
                if (id == null || id == 0)
                {
                    return HttpNotFound();
                }                
                var question = db.Question.Find(id);
                var option = (from x in db.QuestionOption
                             where x.QuestionOptionID == question.QuestionOptionID
                             select x).ToList();
                foreach (var item in option)
                {
                    db.QuestionOption.Remove(item);
                }
                db.Question.Remove(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        //seçilen şıkkın güncellemesini yapar
        public ActionResult UpdateOpScreen(int? id)
        {
            if (isAdmin == true)
            {
                if (id == null || id == 0)
                {
                    return HttpNotFound();
                }
                var model = db.QuestionOption.Find(id);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        [HttpPost]
        public ActionResult UpdateOpScreen(QuestionOption option)
        {
            if (isAdmin == true)
            {
                db.Entry(option).State = System.Data.Entity.EntityState.Modified;
                db.Entry(option).Property(e => e.QuestionOptionID).IsModified = false;
                db.Entry(option).Property(e => e.Total).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        //silme işlemi
        public ActionResult DeleteOp(int? id)
        {
            if (isAdmin == true)
            {
                if (id == null || id == 0)
                {
                    return HttpNotFound();
                }
                var option = db.QuestionOption.Find(id);   
                db.QuestionOption.Remove(option);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        //soru ekleme
        public ActionResult AddQuestion(Question question)
        {
            if (isAdmin == true)
            {
                if (question.QuestionLine != null)
                {
                    question.CreateBy = user_name;
                    db.Question.Add(question);
                    db.SaveChanges();
                    i = question.id;
                    question.QuestionOptionID = i;
                    db.SaveChanges();
                    return RedirectToAction("AddOption");
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
        //soru şıkkı ekleme
        [HttpGet]
        public ActionResult AddOption(Question question)
        {
            if (isAdmin == true)
            {
                var v = db.Question.OrderByDescending(t => t.QuestionOptionID).First();
                ViewBag.yolla = v.QuestionOptionID;
                var model = db.QuestionOption.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "AnsQues");
            }
        }
        [HttpPost]
        public ActionResult AddOption(QuestionOption questionOption, Question question)
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