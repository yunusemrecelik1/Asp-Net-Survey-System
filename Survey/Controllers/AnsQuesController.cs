using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey.Controllers
{
    public class AnsQuesController : BaseController
    {
        /* her kullanıcının bir ankete bir oy verebilmesini istedim. köprü görevi görmesi için oluşturduğum user_vote tablosu sayesinde
         seçilen şıkkın hangi tabloya ait olduğunu section ile user_vote tablosuna kaydettim. daha sonrasında contains ile user_vota
         tablosunda eklenenler arasında
         olup olmadığı kontrolünü yapıp olmayanları ekrana yazdırdım.
             */
        public ActionResult Index()
        {
            var mainmodel = (from s in db.User_Vote
                            where s.isSelect == true && s.user_name == user_name
                            select s.QuestionOptionID).ToList();
            int[] wrong = new int[mainmodel.Count];
            var post = (from i in db.QuestionOption
                        where i.QuestionOptionID != null && !mainmodel.Contains(i.QuestionOptionID)
                        select i.QuestionOptionID).Distinct().ToList();
            
            int[] id = new int[post.Count];
            for (int i = 0; i < post.Count; i++)
            {
                id[i] = post[i].Value;
            }
            var model = from p in db.Question
                        orderby p.QuestionOptionID descending
                        where id.Contains(p.id)  
                        select p;
            return View(model);  
        }
        //seçilen anketin şıklarını getiren kod
        public ActionResult Ans(int? id)
        {
            if (id == null || id == 0)
            {
                return HttpNotFound();
            }
            var model = (from x in db.QuestionOption
                         where x.QuestionOptionID == id
                         select x).ToList();
            if (model.Count == 0)
            {
                return RedirectToAction("Index", "User");
            }
            
            var model2 = Convert.ToInt32((from t in db.QuestionOption
                                          where t.QuestionOptionID == id
                                          select t.Total).Sum());
            ViewBag.gonder = model2;      
            return View(model);
        }
        //seçilen şıkkın kaydını yapan kod
        [HttpPost]
        public ActionResult Ans(QuestionOption queop, int? id,User_Vote user_vt)
        {

            if (id == null || id == 0)
            {
                return HttpNotFound();
            }
            var upt = db.QuestionOption.Find(queop.id);
            upt.Total++;
            user_vt.QuestionOptionID = upt.QuestionOptionID;
            user_vt.user_name = user_name;
            user_vt.isSelect = true;
            db.User_Vote.Add(user_vt);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //kullanıcının tüm anketleri görebilmesini sağlayan kod
        public ActionResult AnketView()
        {
            var model4 = db.Question.ToList(); 
            return View(model4);
        }
        public ActionResult AnsView(int? id)
        {
            if (id == null || id == 0)
            {
                return HttpNotFound();
            }
            var model = (from x in db.QuestionOption
                         where x.QuestionOptionID == id
                         select x).ToList();
            if (model.Count == 0)
            {
                return RedirectToAction("Index", "User");
            }

            var model2 = Convert.ToInt32((from t in db.QuestionOption
                                          where t.QuestionOptionID == id
                                          select t.Total).Sum());
            ViewBag.gonder = model2;
            return View(model);
        }
    }
}