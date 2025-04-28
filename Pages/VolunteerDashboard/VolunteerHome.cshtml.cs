using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;

namespace DisasterAlleviationFoundation.Pages.VolunteerDashboard
{
    public class VolunteerHomeModel : PageModel
    {
        //variables
        public string email = "";
        public List<string> description { get; set; } = new List<string>();

        public void OnGet()
        {
            //requesting email that was passed to page
            email = Request.Query["email"];

            //calling method to display
            DbContext.VolunteerMessages(description, email);
        }
    }
}
