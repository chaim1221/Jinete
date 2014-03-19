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
    public class NotebookViewModel
    {
        public Notebook _notebook { get; set; }
        public string _username { get; set; }
        public Checkout _lastcheckout { get; set; }
        public Sale _sold { get; set; }
    }

    public class NotebookCreateModel : Notebook
    {
        [DisplayName("User")]
        public IEnumerable<SelectListItem> Users { get; set; }
    }

    public class NotebookEditModel
    {
        public Notebook _notebook { get; set; }
        public List<CheckoutViewModel> _checkouts { get; set; }
        public Sale _sale { get; set; }
        
        [DisplayName("User")]
        public IEnumerable<SelectListItem> Users { get; set; }
        public string UserId { get; set; }
    }

    public class NotebookDetailsModel
    {
        public Notebook _notebook { get; set; }
        public ApplicationUser _user { get; set; }
        public List<CheckoutViewModel> _checkouts { get; set; }
        public Sale _sale { get; set; }
    }

    public class CheckoutViewModel : Checkout
    {
        public string Username { get; set; }
    }

    public class CheckoutCreateModel : Checkout
    {
        [DisplayName("User")]
        public IEnumerable<SelectListItem> Users { get; set; }
    }
}