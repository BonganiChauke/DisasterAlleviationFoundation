using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;

namespace DisasterAlleviationFoundation.Pages.VolunteerDashboard
{
    public class VolunteerSchedulesModel : PageModel
    {
        //variables
        public string email = "";
        public List<string> scheduleArea { get; set; } = new List<string>();
        public List<string> scheduleTime { get; set; } = new List<string>();
        public List<string> scheduleDate { get; set; } = new List<string>();
        public List<string> status { get; set; } = new List<string>();

        public void OnGet()
        {
            //requesting email that was passed to page
            email = Request.Query["email"];

            //
            Information();
        }

        //method to view information
        public void Information()
        {
            //calling method to view schedules
            DbContext.VolunteerSchedules(scheduleArea, scheduleTime, scheduleDate, status, email);
        }
    }
}
