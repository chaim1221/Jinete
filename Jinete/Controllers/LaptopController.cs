using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Jinete.Models;
using Jinete.ModelExtensions;
using Jinete.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Jinete.Controllers
{
    public class LaptopController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> um;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: /Laptop/ this is what the browser is doing
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            List<LaptopViewModel> laptopList = new List<LaptopViewModel>();
            var laps = db.Laptops.ToList();

            foreach (Laptop lap in laps)
            {
                LaptopViewModel lapView = new LaptopViewModel();
                ApplicationUser _user = lap.ApplicationUser;
                lapView._laptop = lap;
                lapView._username = _user.FirstName + " " + _user.LastName;
                lapView._lastcheckout = lap.Checkouts
                    .OrderBy(x => x.dtCheckedOut)
                    .LastOrDefault() == null ? null :
                        lap.Checkouts.OrderBy(x => x.dtCheckedOut)
                        .Select(x => new CheckoutViewModel
                        {
                            dtCheckedOut = x.dtCheckedOut,
                            dtReturned = x.dtReturned,
                            Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                        }).LastOrDefault();
                lapView._sold = lap.Sale ?? null;
                laptopList.Add(lapView);
            }

            return View(laptopList.AsEnumerable());
        }

        // GET: /Laptop/Checkout/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Checkout(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new CheckoutCreateModel();
            model.EquipmentId = (int)id;
            model.dtCheckedOut = DateTime.Now;

            string selectId = User.Identity.GetUserId();
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // POST: /Laptop/Checkout/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Checkout(CheckoutCreateModel model)
        {
            if (ModelState.IsValid)
            {
                Checkout _checkout = new Checkout
                    {
                        ApplicationUser = um.FindById(model.ApplicationUserId),
                        dtCheckedOut = model.dtCheckedOut//,
                        // Early attempt to create a *-1 rel.
                        //EquipmentId = model.EquipmentId,
                        //EquipmentType = "Laptop"
                    };
                Laptop _laptop = db.Laptops.Find(model.EquipmentId);
                ApplicationUser _user = _laptop.ApplicationUser;
                _laptop.isCheckedOut = true;
                _laptop.Checkouts.Add(_checkout);
                _laptop.ApplicationUser = _user; // No fucking clue if/why this is necessary.

                db.Entry(_laptop).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    GetDbErrorState(dbEx);
                }

                return RedirectToAction("Index");
            }

            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            // I think I can, I think I can....
            Checkout checkout = laptop.Checkouts.OrderBy(x => x.dtCheckedOut).Last();
            ApplicationUser LaptopUser = laptop.ApplicationUser;
            ApplicationUser CheckoutUser = checkout.ApplicationUser;
            laptop.isCheckedOut = false;
            checkout.dtReturned = DateTime.Now;
            checkout.ApplicationUser = CheckoutUser;
            laptop.ApplicationUser = LaptopUser; //once again the insanity

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                GetDbErrorState(dbEx);
            }

            return RedirectToAction("Index");
        }

        // GET: /Laptop/Details/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            LaptopViewModel details = new LaptopViewModel();
            details._laptop = laptop;

            details._lastcheckout = laptop.Checkouts.Any() ? laptop.Checkouts.OrderByDescending(x => x.dtCheckedOut).Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                }).First() : null;

            details._sold = laptop.Sale == null ? null : laptop.Sale;

            return View(details);
        }

        // GET: /Laptop/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            EquipmentCreateModel model = new EquipmentCreateModel();
            string selectId = User.Identity.GetUserId();
            model.Users = FullNameUserList(db, selectId);
            return View(model);
        }

        // POST: /Laptop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(EquipmentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                Laptop _laptop = new Laptop
                    {
                        ApplicationUser = um.FindById(model.ApplicationUserId),
                        EquipmentName = model.EquipmentName,
                        SerialNumber = model.SerialNumber,
                        PurchasePrice = model.PurchasePrice,
                        isCheckedOut = false
                    };
                db.Laptops.Add(_laptop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // GET: /Laptop/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Laptop _laptop = db.Laptops.Find(id);
            if (_laptop == null)
            {
                return HttpNotFound();
            }

            LaptopEditModel model = new LaptopEditModel(_laptop);

            string selectId = _laptop.ApplicationUser.Id;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // POST: /Laptop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(LaptopEditModel model)
        {
            if (ModelState.IsValid)
            {
                Laptop _laptop = db.Laptops.Find(model.LaptopId);

                _laptop.ApplicationUser = um.FindById(model.ApplicationUserId);
                _laptop.Discarded = model.Discarded;
                _laptop.EquipmentName = model.EquipmentName;
                _laptop.LostOrStolen = model.LostOrStolen;
                _laptop.PurchasePrice = model.PurchasePrice;
                _laptop.SerialNumber = model.SerialNumber;

                if (model.dtSold != null)
                {
                    Sale _sale = new Sale
                        {
                            dtSold = (DateTime)model.dtSold,
                            SalePrice = model.SalePrice ?? 0.0
                        };
                    db.Sales.Add(_sale);
                    db.SaveChanges();
                    _laptop.Sale = _sale;
                }

                db.Entry(_laptop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // GET: /Laptop/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            return View(laptop);
        }

        // POST: /Laptop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Laptop laptop = db.Laptops.Find(id);
            db.Laptops.Remove(laptop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public static SelectList FullNameUserList(ApplicationDbContext db, string id)
        {
            List<ApplicationUser> users = db.Users.ToList();
            IEnumerable<SelectListItem> selectList = users.AsEnumerable()
                .ToSelectUserList(id);
            return new SelectList(selectList, "Value", "Text", id);
        }

        public static void GetDbErrorState(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    Trace.TraceInformation("Property: {0} Error: {1} Entity: {2}", validationError.PropertyName, validationError.ErrorMessage, validationErrors.Entry.Entity.GetType().FullName);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                um.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}