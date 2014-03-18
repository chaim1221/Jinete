using Jinete.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jinete.ModelExtensions
{
    static class Extensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<ApplicationUser> users, string selectedId)
        {
            return users.OrderBy(user => user.UserName).Select(user => 
                new SelectListItem
                {
                    Selected = (user.Id == selectedId),
                    Text = user.FirstName + " " + user.LastName,
                    Value = user.Id.ToString()
                });
        }
    }
}