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
    public class MobilePhoneController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> um;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: /MobilePhone/ this is what the browser is doing
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            List<MobilePhoneViewModel> mobilePhoneList = new List<MobilePhoneViewModel>();
            var mobiles = db.MobilePhones.ToList();

            foreach (MobilePhone mobile in mobiles)
            {
                MobilePhoneViewModel mobileView = new MobilePhoneViewModel();
                ApplicationUser _user = mobile.ApplicationUser;
                mobileView._mobilePhone = mobile;
                mobileView._username = _user.FirstName + " " + _user.LastName;
                mobileView._lastcheckout = mobile.Checkouts
                    .OrderBy(x => x.dtCheckedOut)
                    .LastOrDefault() == null ? null :
                        mobile.Checkouts.OrderBy(x => x.dtCheckedOut)
                        .Select(x => new CheckoutViewModel
                        {
                            dtCheckedOut = x.dtCheckedOut,
                            dtReturned = x.dtReturned,
                            Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                        }).LastOrDefault();
                mobileView._sold = mobile.Sale ?? null;
                mobilePhoneList.Add(mobileView);
            }

            return View(mobilePhoneList.AsEnumerable());
        }

        // GET: /MobilePhone/Checkout/5
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

        // POST: /MobilePhone/Checkout/5
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
                        //EquipmentType = "MobilePhone"
                    };
                MobilePhone _mobilePhone = db.MobilePhones.Find(model.EquipmentId);
                ApplicationUser _user = _mobilePhone.ApplicationUser;
                _mobilePhone.isCheckedOut = true;
                _mobilePhone.Checkouts.Add(_checkout);
                _mobilePhone.ApplicationUser = _user; // No fucking clue if/why this is necessary.

                db.Entry(_mobilePhone).State = EntityState.Modified;
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
            MobilePhone mobilePhone = db.MobilePhones.Find(id);
            if (mobilePhone == null)
            {
                return HttpNotFound();
            }
            // I think I can, I think I can....
            Checkout checkout = mobilePhone.Checkouts.OrderBy(x => x.dtCheckedOut).Last();
            ApplicationUser MobilePhoneUser = mobilePhone.ApplicationUser;
            ApplicationUser CheckoutUser = checkout.ApplicationUser;
            mobilePhone.isCheckedOut = false;
            checkout.dtReturned = DateTime.Now;
            checkout.ApplicationUser = CheckoutUser;
            mobilePhone.ApplicationUser = MobilePhoneUser; //once again the insanity

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

        // GET: /MobilePhone/Details/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobilePhone mobilePhone = db.MobilePhones.Find(id);
            if (mobilePhone == null)
            {
                return HttpNotFound();
            }
            MobilePhoneViewModel details = new MobilePhoneViewModel();
            details._mobilePhone = mobilePhone;

            details._lastcheckout = mobilePhone.Checkouts.Any() ? mobilePhone.Checkouts.OrderByDescending(x => x.dtCheckedOut).Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                }).First() : null;

            details._sold = mobilePhone.Sale == null ? null : mobilePhone.Sale;

            return View(details);
        }

        // GET: /MobilePhone/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            EquipmentCreateModel model = new EquipmentCreateModel();
            string selectId = User.Identity.GetUserId();
            model.Users = FullNameUserList(db, selectId);
            return View(model);
        }

        // POST: /MobilePhone/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(EquipmentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                MobilePhone _mobilePhone = new MobilePhone
                    {
                        ApplicationUser = um.FindById(model.ApplicationUserId),
                        EquipmentName = model.EquipmentName,
                        SerialNumber = model.SerialNumber,
                        PurchasePrice = model.PurchasePrice,
                        isCheckedOut = false
                    };
                db.MobilePhones.Add(_mobilePhone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // GET: /MobilePhone/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MobilePhone _mobilePhone = db.MobilePhones.Find(id);
            if (_mobilePhone == null)
            {
                return HttpNotFound();
            }

            MobilePhoneEditModel model = new MobilePhoneEditModel(_mobilePhone);

            string selectId = _mobilePhone.ApplicationUser.Id;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // POST: /MobilePhone/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(MobilePhoneEditModel model)
        {
            if (ModelState.IsValid)
            {
                MobilePhone _mobilePhone = db.MobilePhones.Find(model.MobilePhoneId);

                _mobilePhone.ApplicationUser = um.FindById(model.ApplicationUserId);
                _mobilePhone.Discarded = model.Discarded;
                _mobilePhone.EquipmentName = model.EquipmentName;
                _mobilePhone.LostOrStolen = model.LostOrStolen;
                _mobilePhone.PurchasePrice = model.PurchasePrice;
                _mobilePhone.SerialNumber = model.SerialNumber;

                if (model.dtSold != null)
                {
                    Sale _sale = new Sale
                        {
                            dtSold = (DateTime)model.dtSold,
                            SalePrice = model.SalePrice ?? 0.0
                        };
                    db.Sales.Add(_sale);
                    db.SaveChanges();
                    _mobilePhone.Sale = _sale;
                }

                db.Entry(_mobilePhone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // GET: /MobilePhone/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobilePhone mobilePhone = db.MobilePhones.Find(id);
            if (mobilePhone == null)
            {
                return HttpNotFound();
            }
            return View(mobilePhone);
        }

        // POST: /MobilePhone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            MobilePhone mobilePhone = db.MobilePhones.Find(id);
            db.MobilePhones.Remove(mobilePhone);
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