using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wskh.Web.Helper
{
    public static class HtmlToJsonHelper
    {
        public static string RenderPartialView(Controller thisController, string viewName, object model)
        {
            thisController.ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(thisController.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(thisController.ControllerContext, viewResult.View, thisController.ViewData, thisController.TempData, sw);

                viewResult.View.Render(viewContext, sw);

                return sw.ToString();
            }
        }
    }
}