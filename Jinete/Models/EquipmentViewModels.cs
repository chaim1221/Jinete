using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jinete.Models;

namespace Jinete.ViewModels
{
    public class DesktopViewModel : EquipmentViewModel
    {
        public Desktop _desktop { get; set; }
    }

    public class DesktopEditModel : EquipmentEditModel
    {
        public DesktopEditModel() { }
        public DesktopEditModel(Desktop _desktop)
        {
            this.DesktopId = _desktop.DesktopId;
            this.EquipmentName = _desktop.EquipmentName;
            this.SerialNumber = _desktop.SerialNumber;
            this.PurchasePrice = _desktop.PurchasePrice;
            this.Discarded = _desktop.Discarded;
            this.LostOrStolen = _desktop.LostOrStolen;
            this.isCheckedOut = _desktop.isCheckedOut;
            this.ApplicationUserId = _desktop.ApplicationUser.Id;
            this.Checkouts = _desktop.Checkouts == null ? new List<CheckoutViewModel>() { } : _desktop.Checkouts
                .Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                })
                .ToList();
            if (_desktop.Sale != null)
            {
                this.dtSold = _desktop.Sale.dtSold;
                this.SalePrice = _desktop.Sale.SalePrice;
            }
        }

        [Required]
        public int DesktopId { get; set; }
    }
    public class LaptopViewModel : EquipmentViewModel
    {
        public Laptop _laptop { get; set; }
    }

    public class LaptopEditModel : EquipmentEditModel
    {
        public LaptopEditModel() { }
        public LaptopEditModel(Laptop _laptop)
        {
            this.LaptopId = _laptop.LaptopId;
            this.EquipmentName = _laptop.EquipmentName;
            this.SerialNumber = _laptop.SerialNumber;
            this.PurchasePrice = _laptop.PurchasePrice;
            this.Discarded = _laptop.Discarded;
            this.LostOrStolen = _laptop.LostOrStolen;
            this.isCheckedOut = _laptop.isCheckedOut;
            this.ApplicationUserId = _laptop.ApplicationUser.Id;
            this.Checkouts = _laptop.Checkouts == null ? new List<CheckoutViewModel>() { } : _laptop.Checkouts
                .Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                })
                .ToList();
            if (_laptop.Sale != null)
            {
                this.dtSold = _laptop.Sale.dtSold;
                this.SalePrice = _laptop.Sale.SalePrice;
            }
        }

        [Required]
        public int LaptopId { get; set; }
    }
    public class MonitorViewModel : EquipmentViewModel
    {
        public Monitor _monitor { get; set; }
    }

    public class MonitorEditModel : EquipmentEditModel
    {
        public MonitorEditModel() { }
        public MonitorEditModel(Monitor _monitor)
        {
            this.MonitorId = _monitor.MonitorId;
            this.EquipmentName = _monitor.EquipmentName;
            this.SerialNumber = _monitor.SerialNumber;
            this.PurchasePrice = _monitor.PurchasePrice;
            this.Discarded = _monitor.Discarded;
            this.LostOrStolen = _monitor.LostOrStolen;
            this.isCheckedOut = _monitor.isCheckedOut;
            this.ApplicationUserId = _monitor.ApplicationUser.Id;
            this.Checkouts = _monitor.Checkouts == null ? new List<CheckoutViewModel>() { } : _monitor.Checkouts
                .Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                })
                .ToList();
            if (_monitor.Sale != null)
            {
                this.dtSold = _monitor.Sale.dtSold;
                this.SalePrice = _monitor.Sale.SalePrice;
            }
        }

        [Required]
        public int MonitorId { get; set; }
    }
    public class TabletViewModel : EquipmentViewModel
    {
        public Tablet _tablet { get; set; }
    }

    public class TabletEditModel : EquipmentEditModel
    {
        public TabletEditModel() { }
        public TabletEditModel(Tablet _tablet)
        {
            this.TabletId = _tablet.TabletId;
            this.EquipmentName = _tablet.EquipmentName;
            this.SerialNumber = _tablet.SerialNumber;
            this.PurchasePrice = _tablet.PurchasePrice;
            this.Discarded = _tablet.Discarded;
            this.LostOrStolen = _tablet.LostOrStolen;
            this.isCheckedOut = _tablet.isCheckedOut;
            this.ApplicationUserId = _tablet.ApplicationUser.Id;
            this.Checkouts = _tablet.Checkouts == null ? new List<CheckoutViewModel>() { } : _tablet.Checkouts
                .Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                })
                .ToList();
            if (_tablet.Sale != null)
            {
                this.dtSold = _tablet.Sale.dtSold;
                this.SalePrice = _tablet.Sale.SalePrice;
            }
        }

        [Required]
        public int TabletId { get; set; }
    }
    public class MobilePhoneViewModel : EquipmentViewModel
    {
        public MobilePhone _mobilePhone { get; set; }
    }

    public class MobilePhoneEditModel : EquipmentEditModel
    {
        public MobilePhoneEditModel() { }
        public MobilePhoneEditModel(MobilePhone _mobilePhone)
        {
            this.MobilePhoneId = _mobilePhone.MobilePhoneId;
            this.EquipmentName = _mobilePhone.EquipmentName;
            this.SerialNumber = _mobilePhone.SerialNumber;
            this.PurchasePrice = _mobilePhone.PurchasePrice;
            this.Discarded = _mobilePhone.Discarded;
            this.LostOrStolen = _mobilePhone.LostOrStolen;
            this.isCheckedOut = _mobilePhone.isCheckedOut;
            this.ApplicationUserId = _mobilePhone.ApplicationUser.Id;
            this.Checkouts = _mobilePhone.Checkouts == null ? new List<CheckoutViewModel>() { } : _mobilePhone.Checkouts
                .Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                })
                .ToList();
            if (_mobilePhone.Sale != null)
            {
                this.dtSold = _mobilePhone.Sale.dtSold;
                this.SalePrice = _mobilePhone.Sale.SalePrice;
            }
        }

        [Required]
        public int MobilePhoneId { get; set; }
    }
    public class CameraViewModel : EquipmentViewModel
    {
        public Camera _camera { get; set; }
    }

    public class CameraEditModel : EquipmentEditModel
    {
        public CameraEditModel() { }
        public CameraEditModel(Camera _camera)
        {
            this.CameraId = _camera.CameraId;
            this.EquipmentName = _camera.EquipmentName;
            this.SerialNumber = _camera.SerialNumber;
            this.PurchasePrice = _camera.PurchasePrice;
            this.Discarded = _camera.Discarded;
            this.LostOrStolen = _camera.LostOrStolen;
            this.isCheckedOut = _camera.isCheckedOut;
            this.ApplicationUserId = _camera.ApplicationUser.Id;
            this.Checkouts = _camera.Checkouts == null ? new List<CheckoutViewModel>() { } : _camera.Checkouts
                .Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                })
                .ToList();
            if (_camera.Sale != null)
            {
                this.dtSold = _camera.Sale.dtSold;
                this.SalePrice = _camera.Sale.SalePrice;
            }
        }

        [Required]
        public int CameraId { get; set; }
    }
    public class TelephoneViewModel : EquipmentViewModel
    {
        public Telephone _telephone { get; set; }
    }

    public class TelephoneEditModel : EquipmentEditModel
    {
        public TelephoneEditModel() { }
        public TelephoneEditModel(Telephone _telephone)
        {
            this.TelephoneId = _telephone.TelephoneId;
            this.EquipmentName = _telephone.EquipmentName;
            this.SerialNumber = _telephone.SerialNumber;
            this.PurchasePrice = _telephone.PurchasePrice;
            this.Discarded = _telephone.Discarded;
            this.LostOrStolen = _telephone.LostOrStolen;
            this.isCheckedOut = _telephone.isCheckedOut;
            this.ApplicationUserId = _telephone.ApplicationUser.Id;
            this.Checkouts = _telephone.Checkouts == null ? new List<CheckoutViewModel>() { } : _telephone.Checkouts
                .Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                })
                .ToList();
            if (_telephone.Sale != null)
            {
                this.dtSold = _telephone.Sale.dtSold;
                this.SalePrice = _telephone.Sale.SalePrice;
            }
        }

        [Required]
        public int TelephoneId { get; set; }
    }
     public class PrinterViewModel : EquipmentViewModel
    {
        public Printer _printer { get; set; }
    }

    public class PrinterEditModel : EquipmentEditModel
    {
        public PrinterEditModel() { }
        public PrinterEditModel(Printer _printer)
        {
            this.PrinterId = _printer.PrinterId;
            this.EquipmentName = _printer.EquipmentName;
            this.SerialNumber = _printer.SerialNumber;
            this.PurchasePrice = _printer.PurchasePrice;
            this.Discarded = _printer.Discarded;
            this.LostOrStolen = _printer.LostOrStolen;
            this.isCheckedOut = _printer.isCheckedOut;
            this.ApplicationUserId = _printer.ApplicationUser.Id;
            this.Checkouts = _printer.Checkouts == null ? new List<CheckoutViewModel>() { } : _printer.Checkouts
                .Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                })
                .ToList();
            if (_printer.Sale != null)
            {
                this.dtSold = _printer.Sale.dtSold;
                this.SalePrice = _printer.Sale.SalePrice;
            }
        }

        [Required]
        public int PrinterId { get; set; }
    }


    public class NotebookViewModel : EquipmentViewModel
    {
        public Notebook _notebook { get; set; }
    }

    public class NotebookEditModel : EquipmentEditModel
    {
        public NotebookEditModel() { }
        public NotebookEditModel(Notebook _notebook)
        {
            this.NotebookId = _notebook.NotebookId;
            this.EquipmentName = _notebook.EquipmentName;
            this.SerialNumber = _notebook.SerialNumber;
            this.PurchasePrice = _notebook.PurchasePrice;
            this.Discarded = _notebook.Discarded;
            this.LostOrStolen = _notebook.LostOrStolen;
            this.isCheckedOut = _notebook.isCheckedOut;
            this.ApplicationUserId = _notebook.ApplicationUser.Id;
            this.Checkouts = _notebook.Checkouts == null ? new List<CheckoutViewModel>() { } : _notebook.Checkouts
                .Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                })
                .ToList();
            if (_notebook.Sale != null)
            {
                this.dtSold = _notebook.Sale.dtSold;
                this.SalePrice = _notebook.Sale.SalePrice;
            }
        }

        [Required]
        public int NotebookId { get; set; }
    }

    public abstract class EquipmentViewModel
    {
        public string _username { get; set; }
        public CheckoutViewModel _lastcheckout { get; set; }
        public Sale _sold { get; set; }
    }

    public class EquipmentCreateModel
    {
        [Required]
        [DisplayName("Item Name")]
        public string EquipmentName { get; set; }
        [Required]
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [DisplayName("Purchase Price")]
        public double PurchasePrice { get; set; }
        [Required]
        [DisplayName("User")]
        public string ApplicationUserId { get; set; }
        [DisplayName("User")]
        public IEnumerable<SelectListItem> Users { get; set; }
    }

    public class EquipmentEditModel
    {
        [Required]
        public string EquipmentName { get; set; }
        [Required]
        public string SerialNumber { get; set; }
        [Required]
        public double PurchasePrice { get; set; }
        [DisplayName("Discarded on")]
        public DateTime? Discarded { get; set; }
        [DisplayName("Lost or stolen on")]
        public DateTime? LostOrStolen { get; set; }
        [Required]
        public bool isCheckedOut { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }

        public List<CheckoutViewModel> Checkouts { get; set; }
        public DateTime? dtSold { get; set; }
        public double? SalePrice { get; set; }
        
        [DisplayName("User")]
        public IEnumerable<SelectListItem> Users { get; set; }
    }

    public class CheckoutViewModel
    {
        [DisplayName("Checked Out")]
        [DataType(DataType.DateTime)]
        public DateTime dtCheckedOut { get; set; }
        [DisplayName("Checked In")]
        [DataType(DataType.DateTime)]
        public DateTime? dtReturned { get; set; }
        [DisplayName("User")]
        public string Username { get; set; }
    }

    public class CheckoutCreateModel
    {
        [Required]
        [DisplayName("Checked Out")]
        [DataType(DataType.DateTime)]
        public DateTime dtCheckedOut { get; set; }
        [Required]
        public int EquipmentId { get; set; }
        [Required]
        [DisplayName("User")]
        public string ApplicationUserId { get; set; }
        [DisplayName("User")]
        public IEnumerable<SelectListItem> Users { get; set; }
    }

    public class SaleViewModel
    {
        [DisplayName("Date and Time Sold")]
        [DataType(DataType.DateTime)]
        public DateTime dtSold { get; set; }

        [DisplayName("Sale Price")]
        [DataType(DataType.Currency)]
        public double SalePrice { get; set; }
    }
}