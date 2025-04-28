using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;

namespace DisasterAlleviationFoundation.Pages.UserDashboard
{
    public class UserHistoryModel : PageModel
    {
        //variables 
        public string email = "";


        //list variables for donation 
        public List<string> ID { get; set; } = new List<string>();
        public List<string> donationType { get; set; } = new List<string>();
        public List<string> donationAmount { get; set; } = new List<string>();
        public List<string> foodType { get; set; } = new List<string>();
        public List<string> paymentType { get; set; } = new List<string>();
        public List<string> donated { get; set; } = new List<string>();
        public List<string> clothesNumber { get; set; } = new List<string>();
        public List<string> description { get; set; } = new List<string>();


        //list variable for report 
        public List<string> disaterID { get; set; } = new List<string>();
        public List<string> disasterType { get; set; } = new List<string>();
        public List<string> location { get; set; } = new List<string>();
        public List<string> disaterDescription { get; set; } = new List<string>();
        public List<string> date { get; set; } = new List<string>();

        //emerncy
        public List<string> emergencyID { get; set; } = new List<string>();
        public List<string> emergencyLocation { get; set; } = new List<string>();
        public List<string> emergencyDescription { get; set; } = new List<string>();


        //feedback
        public List<string> feedBackID { get; set; } = new List<string>();
        public List<string> feedBackTitle { get; set; } = new List<string>();
        public List<string> feedBackDescription { get; set; } = new List<string>();

        public void OnGet()
        {
            Information();
        }

        public void Information()
        {
            //requesting email that was passed to page
            email = Request.Query["email"];

            //calling method to display donations
            DbContext.UserDonation(ID, donationType, donationAmount, foodType, paymentType, donated, clothesNumber, description, email);

            //calling method to display reports
            DbContext.UsersReport(disaterID, disasterType, location, disaterDescription, date, email);

            //calling method to emergncy
            DbContext.UsersEmergency(emergencyID, emergencyLocation, emergencyDescription, email);

            //calling method to display reports
            DbContext.UsersFeedback(feedBackID, feedBackTitle, feedBackDescription, email);
        }
    }
}
