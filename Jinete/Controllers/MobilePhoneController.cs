using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jinete.Models;

namespace Jinete.Controllers
{
    public class MobilePhoneController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /MobilePhone/
        public ActionResult Index()
        {
            return View(db.MobilePhones.ToList());
        }

        // GET: /MobilePhone/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobilePhone mobilephone = db.MobilePhones.Find(id);
            if (mobilephone == null)
            {
                return HttpNotFound();
            }
            return View(mobilephone);
        }

        // GET: /MobilePhone/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MobilePhone/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MobilePhoneId,EquipmentName,SerialNumber,PurchasePrice,Discarded,LostOrStolen,isCheckedOut")] MobilePhone mobilephone)
        {
            if (ModelState.IsValid)
            {
                db.MobilePhones.Add(mobilephone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mobilephone);
        }

        // GET: /MobilePhone/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobilePhone mobilephone = db.MobilePhones.Find(id);
            if (mobilephone == null)
            {
                return HttpNotFound();
            }
            return View(mobilephone);
        }

        // POST: /MobilePhone/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MobilePhoneId,EquipmentName,SerialNumber,PurchasePrice,Discarded,LostOrStolen,isCheckedOut")] MobilePhone mobilephone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mobilephone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mobilephone);
        }

        // GET: /MobilePhone/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobilePhone mobilephone = db.MobilePhones.Find(id);
            if (mobilephone == null)
            {
                return HttpNotFound();
            }
            return View(mobilephone);
        }

        // POST: /MobilePhone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MobilePhone mobilephone = db.MobilePhones.Find(id);
            db.MobilePhones.Remove(mobilephone);
            db.SaveChanges();
            return RedirectToAction("Index");
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
