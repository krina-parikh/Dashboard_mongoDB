using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dashboard.Controllers;

namespace Dashboard.Helpers
{
    public static class HtmlHelpers
    {

        public static void setValue(this HtmlHelper helper)
        {
            HomeController.companyId = 0;
            HomeController.salesOption = 1;
            HomeController.startDate = new DateTime(DateTime.Now.Year, 5 , 1);
            HomeController.endDate = DateTime.Now;
            HomeController.catgryId = 0;
            
        }

    
    }
}