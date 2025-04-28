using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisasterAlleviationFoundation.Pages.VolunteerDashboard
{
    public class VolunteerDashboardModel : PageModel
    {
        //email variable
        public string email = "";

        public void OnGet()
        {
            //requesting email that was passed to page
            email = Request.Query["email"];
        }
    }
}
