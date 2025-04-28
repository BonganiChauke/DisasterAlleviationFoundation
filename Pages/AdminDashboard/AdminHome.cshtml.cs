using DisasterAlleviationFoundation.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisasterAlleviationFoundation.Pages.AdminDashboard
{
    public class AdminHomeModel : PageModel
    {

        //list variables for donation 
        public List<string> ID { get; set; } = new List<string>();
        public List<string> donationType { get; set; } = new List<string>();
        public List<string> donationAmount { get; set; } = new List<string>();
        public List<string> foodType { get; set; } = new List<string>();
        public List<string> paymentType { get; set; } = new List<string>();
        public List<string> donated { get; set; } = new List<string>();
        public List<string> clothesNumber { get; set; } = new List<string>();
        public List<string> description { get; set; } = new List<string>();
        public List<string> email { get; set; } = new List<string>();

        //list variable for report 
        public List<string> disaterID { get; set; } = new List<string>();
        public List<string> disasterType { get; set; } = new List<string>();
        public List<string> location { get; set; } = new List<string>();
        public List<string> disaterDescription { get; set; } = new List<string>();
        public List<string> date { get; set; } = new List<string>();
        public List<string> disatserEmail { get; set; } = new List<string>();

        public void OnGet()
        {
            //calling method with methods to display
            Information();
        }

        //
        public void Information()
        {
            //calling method to display donations
            DbContext.DonationReport(ID, donationType, donationAmount, foodType, paymentType, donated, clothesNumber, description, email);

            //calling method to display reports
            DbContext.DisaterReport(disaterID, disasterType, location, disaterDescription, date, disatserEmail);
        }
    }
}
