using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Jinete.Models
{
    public abstract class Equipment // : CheckoutInfo
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
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Checkout> Checkouts { get; set; }

        public virtual Sale Sale { get; set; }
    }

    public class Checkout
    {
        public int CheckoutId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime dtCheckedOut { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? dtReturned { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class Sale
    {
        public int SaleId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime dtSold { get; set; }

        [Required]
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

    public class Telephone : Equipment
    {
        public int TelephoneId { get; set; }
    }

    public class MobilePhone : Equipment
    {
        public int MobilePhoneId { get; set; }
    }

    public class Camera : Equipment
    {
        public int CameraId { get; set; }
    }
}