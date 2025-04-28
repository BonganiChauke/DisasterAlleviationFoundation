using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;

namespace DisasterAlleviationFoundation.Pages.UserDashboard
{
    public class UserHomeModel : PageModel
    {
        //declaring variables 
        public string email = "";
        public List<string> firstName { get; set; } = new List<string>();
        public List<string> lastName { get; set; } = new List<string>();
        public string title = string.Empty, location = string.Empty, description = string.Empty, descriptionFeedBack = string.Empty, alertFeedBack = string.Empty, alertEmergncy = string.Empty;


        public void OnGet()
        {
            //requesting email that was passed to page
            email = Request.Query["email"];

            //call method for full user names
            Information();

        }

        public void Onpost()
        {
            // requesting form type to indicate which form beign used 
            var formType = Request.Form["formType"];

            //if 
            if (formType.Equals("feedBack"))
            {

                // Retrieve form values for feedback
                title = Request.Form["titleMessage"];
                descriptionFeedBack = Request.Form["feedBackMessage"];
                email = Request.Query["email"];

                // Save feedback to the database
                if (DbContext.FeedBack(title, descriptionFeedBack, email).Equals("Success"))
                {
                    alertFeedBack = "Feedback received. Thank you!";
                }
                else
                {
                    alertFeedBack = DbContext.FeedBack(title, descriptionFeedBack, email);
                }
                //call method for full user names
                DbContext.fullNames(firstName, lastName, email);
            }
            else if (formType.Equals("help"))
            {
                //requesting form values
                location = Request.Form["emergencyArea"];
                description = Request.Form["emergencyDescription"];
                //requesting email that was passed to page
                email = Request.Query["email"];

                //calling method to send message
                if (DbContext.Emergencies(location, description, email).Equals("Success"))
                {
                    //alert the user
                    alertEmergncy = "Succssful Receive help is on the way";
                }
                else
                {
                    //alert the user
                    alertEmergncy = DbContext.Emergencies(location, description, email);
                }

                //call method for full user names
                DbContext.fullNames(firstName, lastName, email);
            }


        }


        //method to retrieve usernames
        public void Information()
        {
            //call method for full user names
            DbContext.fullNames(firstName, lastName, email);
        }
    }
}
