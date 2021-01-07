using System.Web;
using System.Web.Mvc;

namespace CKEditor_TokenEndPoint
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
