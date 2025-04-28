using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;

namespace DisasterAlleviationFoundation.Pages.VolunteerDashboard
{
    public class VolunteerTaskModel : PageModel
    {
        //variables
        public string email = "";
        public List<string> taskName { get; set; } = new List<string>();
        public List<string> description { get; set; } = new List<string>();
        public List<string> areaName { get; set; } = new List<string>();
        public List<string> status { get; set; } = new List<string>();

        public void OnGet()
        {
            //requesting email that was passed to page
            email = Request.Query["email"];

            //calling method to view task
            Information();
        }

        //method for details
        public void Information()
        {
            //calling method to view tasks
            DbContext.VolunteerTask(taskName, description, areaName, status, email);
        }
    }
}
