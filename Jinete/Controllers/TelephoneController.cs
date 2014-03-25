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
    public class TelephoneController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> um;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: /Telephone/ this is what the browser is doing
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            List<TelephoneViewModel> telephoneList = new List<TelephoneViewModel>();
            var phones = db.Telephones.ToList();

            foreach (Telephone phone in phones)
            {
                TelephoneViewModel phoneView = new TelephoneViewModel();
                ApplicationUser _user = phone.ApplicationUser;
                phoneView._telephone = phone;
                phoneView._username = _user.FirstName + " " + _user.LastName;
                phoneView._lastcheckout = phone.Checkouts
                    .OrderBy(x => x.dtCheckedOut)
                    .LastOrDefault() == null ? null :
                        phone.Checkouts.OrderBy(x => x.dtCheckedOut)
                        .Select(x => new CheckoutViewModel
                        {
                            dtCheckedOut = x.dtCheckedOut,
                            dtReturned = x.dtReturned,
                            Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                        }).LastOrDefault();
                phoneView._sold = phone.Sale ?? null;
                telephoneList.Add(phoneView);
            }

            return View(telephoneList.AsEnumerable());
        }

        // GET: /Telephone/Checkout/5
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

        // POST: /Telephone/Checkout/5
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
                    //EquipmentType = "Telephone"
                };
                Telephone _telephone = db.Telephones.Find(model.EquipmentId);
                ApplicationUser _user = _telephone.ApplicationUser;
                _telephone.isCheckedOut = true;
                _telephone.Checkouts.Add(_checkout);
                _telephone.ApplicationUser = _user; // No fucking clue if/why this is necessary.

                db.Entry(_telephone).State = EntityState.Modified;
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
            Telephone telephone = db.Telephones.Find(id);
            if (telephone == null)
            {
                return HttpNotFound();
            }
            // I think I can, I think I can....
            Checkout checkout = telephone.Checkouts.OrderBy(x => x.dtCheckedOut).Last();
            ApplicationUser TelephoneUser = telephone.ApplicationUser;
            ApplicationUser CheckoutUser = checkout.ApplicationUser;
            telephone.isCheckedOut = false;
            checkout.dtReturned = DateTime.Now;
            checkout.ApplicationUser = CheckoutUser;
            telephone.ApplicationUser = TelephoneUser; //once again the insanity

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

        // GET: /Telephone/Details/5
        [Authorize(Roles = "Administrator, Manager")]
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
            TelephoneViewModel details = new TelephoneViewModel();
            details._telephone = telephone;

            details._lastcheckout = telephone.Checkouts.Any() ? telephone.Checkouts.OrderByDescending(x => x.dtCheckedOut).Select(x => new CheckoutViewModel
            {
                dtCheckedOut = x.dtCheckedOut,
                dtReturned = x.dtReturned,
                Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
            }).First() : null;

            details._sold = telephone.Sale == null ? null : telephone.Sale;

            return View(details);
        }

        // GET: /Telephone/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            EquipmentCreateModel model = new EquipmentCreateModel();
            string selectId = User.Identity.GetUserId();
            model.Users = FullNameUserList(db, selectId);
            return View(model);
        }

        // POST: /Telephone/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(EquipmentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                Telephone _telephone = new Telephone
                {
                    ApplicationUser = um.FindById(model.ApplicationUserId),
                    EquipmentName = model.EquipmentName,
                    SerialNumber = model.SerialNumber,
                    PurchasePrice = model.PurchasePrice,
                    isCheckedOut = false
                };
                db.Telephones.Add(_telephone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // GET: /Telephone/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Telephone _telephone = db.Telephones.Find(id);
            if (_telephone == null)
            {
                return HttpNotFound();
            }

            TelephoneEditModel model = new TelephoneEditModel(_telephone);

            string selectId = _telephone.ApplicationUser.Id;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // POST: /Telephone/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(TelephoneEditModel model)
        {
            if (ModelState.IsValid)
            {
                Telephone _telephone = db.Telephones.Find(model.TelephoneId);

                _telephone.ApplicationUser = um.FindById(model.ApplicationUserId);
                _telephone.Discarded = model.Discarded;
                _telephone.EquipmentName = model.EquipmentName;
                _telephone.LostOrStolen = model.LostOrStolen;
                _telephone.PurchasePrice = model.PurchasePrice;
                _telephone.SerialNumber = model.SerialNumber;

                if (model.dtSold != null)
                {
                    Sale _sale = new Sale
                    {
                        dtSold = (DateTime)model.dtSold,
                        SalePrice = model.SalePrice ?? 0.0
                    };
                    db.Sales.Add(_sale);
                    db.SaveChanges();
                    _telephone.Sale = _sale;
                }

                db.Entry(_telephone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // GET: /Telephone/Delete/5
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Telephone telephone = db.Telephones.Find(id);
            db.Telephones.Remove(telephone);
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
