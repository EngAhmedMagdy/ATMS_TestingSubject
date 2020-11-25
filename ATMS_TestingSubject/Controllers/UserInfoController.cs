using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ATMS_TestingSubject.Models;

namespace ATMS_TestingSubject.Controllers
{

    public class UserInfoController : Controller
    {
        private ATMS_Model db = new ATMS_Model();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserInfo user)
        {
            if(user.Type != null && user.Name != null && user.Passward != null)
            {
                var UserCheck = db.UserInfoes.Where(x => x.Type == user.Type && x.Name == user.Name && x.Passward == user.Passward).ToArray();
                if (UserCheck.Length > 0)
                {

                    user.Id = UserCheck.First().Id;
                    if (user.Type == "Admin")
                    {
                        return RedirectToAction("AdminDashboard", "UserInfo");
                    }
                    else
                    {
                        return RedirectToAction("Details", "UserInfo", new { id = user.Id });
                    }
                }
                else
                {
                    Response.Write("wrong data");
                    return View();
                }
            }
                Response.Write("fill blanks");
                return View();
            
        }
        // GET: UserInfo
        [NoDirectAccess]
        public async Task<ActionResult> AdminDashboard()
        {
            var userInfoes = db.UserInfoes.Include(u => u.Department);
            return View(await userInfoes.ToListAsync());
        }

        // GET: UserInfo/Details/5
        [NoDirectAccess]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo userInfo = await db.UserInfoes.FindAsync(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // GET: UserInfo/Create
        public ActionResult Create()
        {
            ViewBag.Type = new List<SelectListItem>() {
                new SelectListItem(){Value="Employee",Text="Employee"},
                new SelectListItem(){Value="Head of Department",Text="Head of Department"},
                new SelectListItem(){Value="Admin",Text="Admin"}
            };
            ViewBag.Gender = new List<SelectListItem>() {
                new SelectListItem(){Value="Male",Text="Male"},
                new SelectListItem(){Value="Female",Text="Female"},
            };
            ViewBag.DepId = new SelectList(db.Departments, "DepId", "DepName");
            return View();
        }

        // POST: UserInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Type,Name,Passward,Email,Gender,DepId,Active,Accepted,AbsenceHours")] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                db.UserInfoes.Add(userInfo);
                await db.SaveChangesAsync();
                return RedirectToAction("Details",new { id = userInfo.Id});
            }

            ViewBag.DepId = new SelectList(db.Departments, "DepId", "DepName", userInfo.DepId);
            return View(userInfo);
        }

        // GET: UserInfo/Edit/5
        [NoDirectAccess]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo userInfo = await db.UserInfoes.FindAsync(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepId = new SelectList(db.Departments, "DepId", "DepName", userInfo.DepId);
            return View(userInfo);
        }

        // POST: UserInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Type,Name,Passward,Email,Gender,DepId,Active,Accepted,AbsenceHours")] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userInfo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = userInfo.Id });
            }
            ViewBag.DepId = new SelectList(db.Departments, "DepId", "DepName", userInfo.DepId);
            return View(userInfo);
        }

        // GET: UserInfo/Delete/5
        [NoDirectAccess]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo userInfo = await db.UserInfoes.FindAsync(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // POST: UserInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserInfo userInfo = await db.UserInfoes.FindAsync(id);
            db.UserInfoes.Remove(userInfo);
            await db.SaveChangesAsync();
            return RedirectToAction("AdminDashboard");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
