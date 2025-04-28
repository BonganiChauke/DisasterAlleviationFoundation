using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisasterAlleviationFoundation.Pages.UserDashboard
{
    public class UserDashboardModel : PageModel
    {
        //variables
        public string email = "";

        public void OnGet()
        {
            //requesting email that was passed to page
            email = Request.Query["email"];
        }
    }
}
