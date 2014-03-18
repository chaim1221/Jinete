using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Jinete.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
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
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<Jinete.Models.Checkout> Checkouts { get; set; }
        public System.Data.Entity.DbSet<Jinete.Models.Sale> Sales { get; set; }
        public System.Data.Entity.DbSet<Jinete.Models.Desktop> Desktops { get; set; }
        public System.Data.Entity.DbSet<Jinete.Models.Laptop> Laptops { get; set; }
        public System.Data.Entity.DbSet<Jinete.Models.Monitor> Monitors { get; set; }
        public System.Data.Entity.DbSet<Jinete.Models.Notebook> Notebooks { get; set; }
        public System.Data.Entity.DbSet<Jinete.Models.Printer> Printers { get; set; }
        public System.Data.Entity.DbSet<Jinete.Models.Tablet> Tablets { get; set; }
    }
}