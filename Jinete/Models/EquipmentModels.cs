using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Jinete.Models
{
    public abstract class Equipment // : CheckoutInfo
    {
        [Required]
        [DisplayName("Item Name")]
        public string EquipmentName { get; set; }
        [Required]
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }
        [Required]
        [DisplayName("Purchase Price")]
        public double PurchasePrice { get; set; }
        [DisplayName("Discarded on")]
        public DateTime? Discarded { get; set; }
        [DisplayName("Lost or stolen on")]
        public DateTime? LostOrStolen { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ICollection<int> CheckoutId { get; set; }
        public int? SaleId { get; set; }
    }

    public class Checkout
    {
        public int CheckoutId { get; set; }
        public string ApplicationUserId { get; set; }
        [DisplayName("Checked Out")]
        [DataType(DataType.DateTime)]
        public DateTime dtCheckedOut { get; set; }
        [DisplayName("Checked In")]
        [DataType(DataType.DateTime)]
        public DateTime? dtReturned { get; set; }
        [DisplayName("At Casa Latina")]
        public bool checkedIn { get; set; }
    }

    public class Sale
    {
        public int SaleId { get; set; }

        [Required]
        [DisplayName("Date and Time Sold")]
        [DataType(DataType.DateTime)]
        public DateTime dtSold { get; set; }

        [Required]
        [DisplayName("Sale Price")]
        [DataType(DataType.Currency)]
        public double SalePrice { get; set; }
    }

    public class Desktop : Equipment
    {
        public int DesktopId { get; set; }
        public int MonitorId { get; set; }
    }

    public class Laptop : Equipment
    {
        public int LaptopId { get; set; }
    }

    public class Monitor : Equipment
    {
        public int MonitorId { get; set; }
    }

    public class Notebook : Equipment
    {
        public int NotebookId { get; set; }
    }

    public class Printer : Equipment
    {
        public int PrinterId { get; set; }
    }
    
    public class Tablet : Equipment
    {
        public int TabletId { get; set; }
    }
}