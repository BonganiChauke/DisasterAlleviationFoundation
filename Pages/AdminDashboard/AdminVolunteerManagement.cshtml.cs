using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;

namespace DisasterAlleviationFoundation.Pages.AdminDashboard
{
    public class AdminVolunteerManagementModel : PageModel
    {
        //declaring variables
        public string email = string.Empty, message = string.Empty, status = string.Empty, scheduleAlert = string.Empty;
        public string assignTasksAlert = string.Empty, assignSchedulesAlert = string.Empty, sendMessagesAlert = string.Empty;
        public DateTime date;
        public TimeOnly time;

        //
        public List<string> emailVolunteer { get; set; } = new List<string>();
        public List<string> taskNames { get; set; } = new List<string>();
        public List<string> taskArea { get; set; } = new List<string>();

        public void OnGet()
        {
            //call method 
            DataInformation();
        }

        public void OnPostAddTaskAsync()
        {
            //call method 
            DataInformation();

            // Handle Add Task form submission
            string taskName = Request.Form["taskName"];
            string taskDescription = Request.Form["taskDescription"];
            string areaName = Request.Form["areaName"];
            status = "Incomplete";
            email = "Unassigned";

            if (DbContext.Tasks(taskName, taskDescription, areaName, status, email).Equals("Success"))
            {
                message = "Task has been added successful";
            }
            else
            {
                message = DbContext.Tasks(taskName, taskDescription, areaName, status, email);
            }
        }//end

        //
        public void OnPostAddSchedulesAsync()
        {
            //call method 
            DataInformation();

            // Handle Add Schedules form submission
            string areaName = Request.Form["areaName"];
            string time = Request.Form["scheduleTime"];
            string scheduleDate = Request.Form["scheduleDate"];
            status = "Not Available";
            email = "Unassigned";

            if (DbContext.Schedules(areaName, time, scheduleDate, status, email).Equals("Success"))
            {
                scheduleAlert = "Schedule has been added successful";
            }
            else
            {
                scheduleAlert = DbContext.Schedules(areaName, time, scheduleDate, status, email);
            }
        }//end

        //
        public void OnPostAssignTasksAsync()
        {
            //call method 
            DataInformation();

            //request form values
            string emailVolunteer = Request.Form["volunteerAssignEmail"];
            string taskName = Request.Form["taskNameAssign"];

            //calling the method
            if (DbContext.AssignVolunteer(emailVolunteer, taskName).Equals("Success"))
            {
                //alert user
                assignTasksAlert = "Successful assigned task to volunteer";
            }
            else
            {
                //alert user
                assignTasksAlert = DbContext.AssignVolunteer(emailVolunteer, taskName);
            }

        }//end

        //
        public void OnPostAssignSchedulesAsync()
        {
            //call method 
            DataInformation();

            //request form values 
            string emailVolunteers = Request.Form["volunteerEmailSchedules"];
            string scheduleArea = Request.Form["scheduleArea"];

            //calling method
            if (DbContext.AssignSchedules(emailVolunteers, scheduleArea).Equals("Success"))
            {
                //alert the user 
                assignSchedulesAlert = "Successful scheduled volunteer";

            }
            else
            {
                //alert the 
                assignSchedulesAlert = DbContext.AssignSchedules(emailVolunteers, scheduleArea);
            }

        }//end

        //
        public void OnPostSendMessagesAsync()
        {
            //call method 
            DataInformation();

            //request form values
            string emailVolunteer = Request.Form["volunteer"];
            string messageDescription = Request.Form["messageDescription"];

            //calling method
            if (DbContext.SendMessages(emailVolunteer, messageDescription).Equals("Success"))
            {
                //alert the user
                sendMessagesAlert = "Successful sent message";
            }
            else
            {
                //alert the user
                sendMessagesAlert = DbContext.SendMessages(emailVolunteer, messageDescription);
            }

        }//end

        //this method will reload the drop fields each time the form is submitted
        public void DataInformation()
        {
            //get emails
            DbContext.VolunteerEmails(emailVolunteer);

            //get task Names
            DbContext.TaskNames(taskNames);

            //get task area
            DbContext.TaskAreas(taskArea);

        }//end
    }
}
