using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Jinete.Models
{
    public abstract class Equipment : CheckoutInfo
    {
        [Required]
        [DisplayName("Name")]
        public string EquipmentName { get; set; }
        [Required]
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }
        [Required]
        [DisplayName("Checked Out")]
        [DataType(DataType.DateTime)]
        public DateTime dtCheckedOut { get; set; }
        [DisplayName("Checked In")]
        [DataType(DataType.DateTime)]
        public DateTime? dtReturned { get; set; }
        [DisplayName("[X]")]
        public bool checkedIn { get; set; }
    }

    public class CheckoutInfo
    {
        [Required]
        [DisplayName("First Name")]
        public string PersonFirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string PersonLastName { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [MaxLength(2, ErrorMessage = "Please enter a state code, such as \"WA\".")]
        public string State { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        public string Zip { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }

    public class Notebook : Equipment 
    {
        public int NotebookId { get; set; }
    }
}