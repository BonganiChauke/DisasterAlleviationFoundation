using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;

namespace DisasterAlleviationFoundation.Pages.UserDashboard
{
    public class UserReportModel : PageModel
    {
        //declaring form variables
        public string donationAmount = string.Empty, clothesNumber = string.Empty, disasterType = string.Empty, location = string.Empty, foodType = string.Empty, paymentType = string.Empty, description = string.Empty, disasterDate = string.Empty, email = string.Empty, message = string.Empty, donationType = string.Empty, messages = "", error = "";
        public byte[]? image;
        public byte[]? video;
        public DateTime donated;

        public void OnGet()
        {
        }

        public void OnPost()
        {
            //requesting form values for report disaster
            disasterType = Request.Form["disasterType"];
            location = Request.Form["location"];
            description = Request.Form["description"];
            disasterDate = Request.Form["disasterDate"];
            // Process image and video files
            image = DbContext.ProcessFileUpload(Request.Form.Files["CaptureImage"]);
            video = DbContext.ProcessFileUpload(Request.Form.Files["CaptureVideo"]);
            //requesting email that was passed to page
            email = Request.Query["email"];


            //to check value of method
            if (DbContext.ReportDisaster(disasterType, location, description, disasterDate, image, video, email).Equals("Success"))
            {
                //to alert the user
                message = "Thank you for reporting... help is on the way";
            }
            else
            {
                // to alert the user
                message = DbContext.ReportDisaster(disasterType, location, description, disasterDate, image, video, email);
            }
        }
    }
}
