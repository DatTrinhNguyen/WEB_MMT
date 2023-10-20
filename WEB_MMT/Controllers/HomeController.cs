using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WEB_MMT.Models;
using System.Configuration;
using static System.Formats.Asn1.AsnWriter;

namespace WEB_MMT.Controllers
{
    public class HomeController : Controller
    {
        QuizzContext db = new QuizzContext();
        score sc = new score();
        int scoreQuizz = 0;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult tlogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult tlogin(TblAdmin tbl)
        {
            TblAdmin ad = db.TblAdmins.Where(x => x.Adname.Equals(tbl.Adname) && x.Adpass.Equals(tbl.Adpass)).FirstOrDefault();

            if (ad != null)
            {
                HttpContext.Session.SetString("ad_id", ad.Adid.ToString());
                return RedirectToAction("Dashboard");
            }

            else
            {
                ViewBag.msg = "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng";
            }
            return View();
        }
        [HttpGet]
        public IActionResult uSignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult uSignUp(User u)
        {
            try
            {
                bool testname = db.Users.Any(s => s.Email == u.Email);
                if (testname)
                {
                    ViewBag.msg = "Email đã được đăng ký";
                }
                else
                {
                    User s = new User();
                    s.Nameuser = u.Nameuser;
                    s.Email = u.Email;
                    s.Passwords = u.Passwords;
                    db.Users.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("USignIn");
                }
            }
            catch (Exception)
            {
                ViewBag.msg = "Không Thể Tạo Tài Khoản";
            }

            return View();
        }
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("iduser");
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult uSignIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult uSignIn(User u)
        {
            User user = db.Users.FirstOrDefault(x => x.Email.Equals(u.Email) && x.Passwords.Equals(u.Passwords));

            if (user != null)
            {
                HttpContext.Session.SetString("iduser", user.Iduser.ToString());
                return RedirectToAction("ExamDashboard");
            }
            else
            {
                ViewBag.msg = "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng";
                return View();
            }
        }

        [HttpGet]
        public ActionResult ExamDashboard()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ExamDashboard(string room)
        {

            List<TblCategory> list = db.TblCategories.ToList();
            foreach (var item in list)
            {
                if (item.adid.ToString() == room)
                {
                    List<TblQuestion> li = db.TblQuestions.Where(x => x.cat_id == item.CatId).ToList();
                    return RedirectToAction("StartQuiz", new { examid = item.CatId, questions = li });
                }

                else
                {
                    ViewBag.error = "Không Tìm Thấy Bộ Đề";
                }

            }

            return View();
        }
        [HttpGet]
        public IActionResult StartQuiz(int examid, List<TblQuestion> questions)
        {
            if (HttpContext.Session.GetString("iduser") == null)
            {
                return RedirectToAction("uSignIn");
            }

            if (questions != null && questions.Any())
            {
                TblQuestion q = questions.First();
                questions.Remove(q);
                return View(q);
            }
            else
            {
                return RedirectToAction("EndExam");
            }
        }

        [HttpPost]
        public IActionResult StartQuiz(TblQuestion q, List<TblQuestion> questions)
        {
            if (HttpContext.Session.GetString("iduser") == null)
            {
                return RedirectToAction("uSignIn");
            }

            string correctans = null;
            if (q.QuestionA != null)
            {
                correctans = "A";
            }
            else if (q.QuestionB != null)
            {
                correctans = "B";
            }
            else if (q.QuestionC != null)
            {
                correctans = "C";
            }
            else if (q.QuestionD != null)
            {
                correctans = "D";
            }

            if (correctans.Equals(q.QuestionCorrect))
            {
                scoreQuizz += 1;
            }

            sc.scoree[2] = scoreQuizz.ToString();
            TempData.Keep();

            return RedirectToAction("StartQuiz", new { questions = questions });
        }


        public ActionResult ViewAllQuestions(int? id, int page = 1)
        {
            if (HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("tlogin");
            }

            if (id == null)
            {
                return RedirectToAction("Dashboard");
            }

            var questions = db.TblQuestions.Where(x => x.cat_id == id).ToList();
            return View(questions);
        }

        public ActionResult EndExam()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add_Category()
        {
            int ad_id = Convert.ToInt32(HttpContext.Session.GetString("ad_id"));
            List<TblCategory> catLi = db.TblCategories.Where(x => x.adid == ad_id).OrderByDescending(x => x.CatId).ToList();
            ViewData["list"] = catLi;
            return View();
        }

        [HttpPost]
        public ActionResult Add_Category(TblCategory cat)
        {
            int ad_id = Convert.ToInt32(HttpContext.Session.GetString("ad_id"));
            Random random = new Random();
            string encryptedString = crypt.Encrypt(cat.CatName.Trim() + random.Next().ToString(), true);

            TblCategory c = new TblCategory
            {
                CatName = cat.CatName,
                adid = ad_id,
                cat_encrytped_string = encryptedString
            };

            db.TblCategories.Add(c);
            db.SaveChanges();

            return RedirectToAction("Add_Category");
        }



        [HttpGet]
        public ActionResult Add_Questions()
        {
            List<TblCategory> categories = db.TblCategories.ToList();
            var selectListItems = categories.Select(c => new SelectListItem { Value = c.CatId.ToString(), Text = c.CatName }).ToList();
            ViewBag.list = new SelectList(selectListItems, "Value", "Text");
            return View();
        }


        [HttpPost]
        public ActionResult Add_Questions(TblQuestion q)
        {
            List<TblCategory> categories = db.TblCategories.ToList();
            var selectListItems = categories.Select(c => new SelectListItem { Value = c.CatId.ToString(), Text = c.CatName }).ToList();
            ViewBag.list = new SelectList(selectListItems, "Value", "Text");
            
            TblQuestion qa = new TblQuestion();
            qa.Questiontext = q.Questiontext;
            qa.QuestionA = q.QuestionA;

            qa.QuestionB = q.QuestionB;
            qa.QuestionC = q.QuestionC;
            qa.QuestionD = q.QuestionD;
            qa.QuestionCorrect = q.QuestionCorrect;

            qa.cat_id = q.cat_id;
            db.TblQuestions.Add(qa);
            db.SaveChanges();
            TempData["ms"] = "Question successfully Added";
            TempData.Keep();
            return RedirectToAction("Add_Questions");

        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("ad_id") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(errorViewModel);
        }
    }
}
