using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;

namespace DisasterAlleviationFoundation.Pages.AdminDashboard
{
    public class AdminVolunteerActivityModel : PageModel
    {
        //declaring list variables for task
        public List<string> taskID { get; set; } = new List<string>();
        public List<string> taskName { get; set; } = new List<string>();
        public List<string> description { get; set; } = new List<string>();
        public List<string> areaName { get; set; } = new List<string>();
        public List<string> taskStatus { get; set; } = new List<string>();
        public List<string> taskEmail { get; set; } = new List<string>();

        //declaring list varibles for schedules
        public List<string> scheduleID { get; set; } = new List<string>();
        public List<string> scheduleArea { get; set; } = new List<string>();
        public List<string> scheduleTime { get; set; } = new List<string>();
        public List<string> scheduleDate { get; set; } = new List<string>();
        public List<string> scheduleStatus { get; set; } = new List<string>();
        public List<string> scheduleEmail { get; set; } = new List<string>();


        public void OnGet()
        {

            //calling method
            Information();
        }

        //method 
        public void Information()
        {
            //calling methods to display tasks
            DbContext.VolunteersTask(taskID, taskName, description, areaName, taskStatus, taskEmail);

            //calling method to display schedules
            DbContext.VolunteersSchedules(scheduleID, scheduleArea, scheduleTime, scheduleDate, scheduleStatus, scheduleEmail);
        }
    }
}
