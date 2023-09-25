using System.Web;
using System.Web.Mvc;

namespace WEB_TH_63CNTT_4
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
