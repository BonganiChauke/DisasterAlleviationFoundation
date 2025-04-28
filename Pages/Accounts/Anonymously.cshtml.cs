using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;

namespace DisasterAlleviationFoundation.Pages.Accounts
{
    public class AnonymouslyModel : PageModel
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

            // requesting form type to indicate which form beign used 
            var formType = Request.Form["formType"];

            // to check which form beign submitted
            if (formType.Equals("reportDisaster"))
            {
                //requesting form values for report disaster
                disasterType = Request.Form["disasterType"];
                location = Request.Form["location"];
                description = Request.Form["description"];
                disasterDate = Request.Form["disasterDate"];
                // Process image and video files
                image = DbContext.ProcessFileUpload(Request.Form.Files["CaptureImage"]);
                video = DbContext.ProcessFileUpload(Request.Form.Files["CaptureVideo"]);
                email = "Anonymously";


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
            else if (formType.Equals("donate"))
            {
                //requesting form values for donation
                donationType = Request.Form["donationType"];
                donationAmount = Request.Form["donationAmount"];
                foodType = Request.Form["foodType"];
                paymentType = Request.Form["paymentType"];
                donated = DateTime.Now;
                clothesNumber = Request.Form["numberOfClothes"];
                description = Request.Form["message"];
                // Process image and video files
                image = DbContext.ProcessFileUpload(Request.Form.Files["CaptureImage"]);
                video = DbContext.ProcessFileUpload(Request.Form.Files["CaptureVideo"]);
                email = "Anonymously";



                //to check the returning value
                if (DbContext.Donate(donationType, donationAmount, foodType, paymentType, donated, clothesNumber, description, image, video, email).Equals("Success"))
                {
                    //to alert the user
                    error = "Thank you for donate... you have help so many";
                }
                else
                {
                    //alert the user
                    error = DbContext.Donate(donationType, donationAmount, foodType, paymentType, donated, clothesNumber, description, image, video, email);
                }
            }

        }

    }
}
