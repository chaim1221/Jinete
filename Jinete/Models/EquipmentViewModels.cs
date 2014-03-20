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