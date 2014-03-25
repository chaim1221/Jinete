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
    public class PrinterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> um;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: /Printer/ this is what the browser is doing
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            List<PrinterViewModel> printerList = new List<PrinterViewModel>();
            var prints = db.Printers.ToList();

            foreach(Printer print in prints)
            {
                PrinterViewModel printView = new PrinterViewModel();
                ApplicationUser _user = print.ApplicationUser;
                printView._printer = print;
                printView._username = _user.FirstName + " " + _user.LastName;
                printView._lastcheckout = print.Checkouts
                    .OrderBy(x => x.dtCheckedOut)
                    .LastOrDefault() == null ? null :
                        print.Checkouts.OrderBy(x => x.dtCheckedOut)
                        .Select(x => new CheckoutViewModel
                        {
                            dtCheckedOut = x.dtCheckedOut,
                            dtReturned = x.dtReturned,
                            Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                        }).LastOrDefault();
                printView._sold = print.Sale ?? null;
                printerList.Add(printView);
            }

            return View(printerList.AsEnumerable());
        }

        // GET: /Printer/Checkout/5
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

        // POST: /Printer/Checkout/5
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
                        //EquipmentType = "Printer"
                    };
                Printer _printer = db.Printers.Find(model.EquipmentId);
                ApplicationUser _user = _printer.ApplicationUser;
                _printer.isCheckedOut = true;
                _printer.Checkouts.Add(_checkout);
                _printer.ApplicationUser = _user; // No fucking clue if/why this is necessary.

                db.Entry(_printer).State = EntityState.Modified;
                try { 
                        db.SaveChanges();
                    }
                catch (DbEntityValidationException dbEx) {
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
            Printer printer = db.Printers.Find(id);
            if (printer == null)
            {
                return HttpNotFound();
            }
            // I think I can, I think I can....
            Checkout checkout = printer.Checkouts.OrderBy(x => x.dtCheckedOut).Last();
            ApplicationUser PrinterUser = printer.ApplicationUser;
            ApplicationUser CheckoutUser = checkout.ApplicationUser;
            printer.isCheckedOut = false;
            checkout.dtReturned = DateTime.Now;
            checkout.ApplicationUser = CheckoutUser;
            printer.ApplicationUser = PrinterUser; //once again the insanity

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

        // GET: /Printer/Details/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Printer printer = db.Printers.Find(id);
            if (printer == null)
            {
                return HttpNotFound();
            }
            PrinterViewModel details = new PrinterViewModel();
            details._printer = printer;

            details._lastcheckout = printer.Checkouts.Any() ? printer.Checkouts.OrderByDescending(x => x.dtCheckedOut).Select(x => new CheckoutViewModel 
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                }).First() : null;

            details._sold = printer.Sale == null ? null : printer.Sale;

            return View(details);
        }

        // GET: /Printer/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            EquipmentCreateModel model = new EquipmentCreateModel();
            string selectId = User.Identity.GetUserId();
            model.Users = FullNameUserList(db, selectId);
            return View(model);
        }

        // POST: /Printer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(EquipmentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                Printer _printer = new Printer
                    {
                        ApplicationUser = um.FindById(model.ApplicationUserId),
                        EquipmentName = model.EquipmentName,
                        SerialNumber = model.SerialNumber,
                        PurchasePrice = model.PurchasePrice,
                        isCheckedOut = false
                    };
                db.Printers.Add(_printer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // GET: /Printer/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Printer _printer = db.Printers.Find(id);
            if (_printer == null)
            {
                return HttpNotFound();
            }
            
            PrinterEditModel model = new PrinterEditModel(_printer);
            
            string selectId = _printer.ApplicationUser.Id;
            model.Users = FullNameUserList(db, selectId);
            
            return View(model);
        }

        // POST: /Printer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(PrinterEditModel model)
        {
            if (ModelState.IsValid)
            {
                Printer _printer = db.Printers.Find(model.PrinterId);

                _printer.ApplicationUser = um.FindById(model.ApplicationUserId);
                _printer.Discarded = model.Discarded;
                _printer.EquipmentName = model.EquipmentName;
                _printer.LostOrStolen = model.LostOrStolen;
                _printer.PurchasePrice = model.PurchasePrice;
                _printer.SerialNumber = model.SerialNumber;
                
                if (model.dtSold != null)
                {
                    Sale _sale = new Sale
                        {
                            dtSold = (DateTime)model.dtSold,
                            SalePrice = model.SalePrice ?? 0.0
                        };
                    db.Sales.Add(_sale);
                    db.SaveChanges();
                    _printer.Sale = _sale;
                }

                db.Entry(_printer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // GET: /Printer/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Printer Printer = db.Printers.Find(id);
            if (Printer == null)
            {
                return HttpNotFound();
            }
            return View(Printer);
        }

        // POST: /Printer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Printer Printer = db.Printers.Find(id);
            db.Printers.Remove(Printer);
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
