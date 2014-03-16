using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jinete.Models;

namespace Jinete.ViewModels
{
    public class NotebookViewModel
    {
        public List<string> _firstNames { get; set; }
        public List<string> _lastNames { get; set; }
        public List<string> _userIds { get; set; }
        public Notebook _notebook { get; set; }
    }
}