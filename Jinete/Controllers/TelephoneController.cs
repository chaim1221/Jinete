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
    public class TelephoneController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Telephone/
        public ActionResult Index()
        {
            return View(db.Telephones.ToList());
        }

        // GET: /Telephone/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telephone telephone = db.Telephones.Find(id);
            if (telephone == null)
            {
                return HttpNotFound();
            }
            return View(telephone);
        }

        // GET: /Telephone/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Telephone/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TelephoneId,EquipmentName,SerialNumber,PurchasePrice,Discarded,LostOrStolen,isCheckedOut")] Telephone telephone)
        {
            if (ModelState.IsValid)
            {
                db.Telephones.Add(telephone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(telephone);
        }

        // GET: /Telephone/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telephone telephone = db.Telephones.Find(id);
            if (telephone == null)
            {
                return HttpNotFound();
            }
            return View(telephone);
        }

        // POST: /Telephone/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TelephoneId,EquipmentName,SerialNumber,PurchasePrice,Discarded,LostOrStolen,isCheckedOut")] Telephone telephone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(telephone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(telephone);
        }

        // GET: /Telephone/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telephone telephone = db.Telephones.Find(id);
            if (telephone == null)
            {
                return HttpNotFound();
            }
            return View(telephone);
        }

        // POST: /Telephone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Telephone telephone = db.Telephones.Find(id);
            db.Telephones.Remove(telephone);
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
