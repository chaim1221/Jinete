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
}