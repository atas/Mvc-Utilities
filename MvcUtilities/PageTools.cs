using System;
using System.IO;
using System.Web.Mvc;

namespace MvcUtilities
{
    public class PageTools
    {
        /// <summary>
        /// Renders a view to string
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string RenderViewToString(ControllerBase controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            try
            {
                using (var sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, null);
                    var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}