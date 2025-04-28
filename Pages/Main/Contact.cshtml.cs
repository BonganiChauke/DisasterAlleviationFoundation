using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;

namespace DisasterAlleviationFoundation.Pages.Main
{
    public class ContactModel : PageModel
    {
        //declaring variables
        public string firstName = string.Empty;
        public string lastName = string.Empty;
        public string email = string.Empty;
        public string phone = string.Empty;
        public string description = string.Empty;
        public string alertContact = string.Empty;
        public string subEmail = string.Empty;
        public string alertSub = string.Empty;

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // requesting form type to indicate which form beign used 
            var formType = Request.Form["formType"];

            //if to check form submitted
            if (formType.Equals("contact"))
            {
                //requesting form values
                firstName = Request.Form["firstName"];
                lastName = Request.Form["lastName"];
                email = Request.Form["email"];
                phone = Request.Form["phone"];
                description = Request.Form["message"];

                //calling method to store to database
                if (DbContext.ContactForm(firstName, lastName, email, phone, description).Equals("Success"))
                {
                    //alert the user
                    alertContact = "Successful sent message you will contacted soon";
                }
                else
                {
                    //alert the user
                    alertContact = DbContext.ContactForm(firstName, lastName, email, phone, description);
                }

            }
            else if (formType.Equals("subscribe"))
            {
                //requesting form values
                subEmail = Request.Form["emailSubscribe"];

                //calling method to store to database
                if (DbContext.Subscribers(subEmail).Equals("Success"))
                {
                    //alert the user
                    alertSub = "Successful subscribe to our newsletter";
                }
                else
                {
                    //alert the user
                    alertSub = DbContext.Subscribers(subEmail);
                }
            }
        }
    }
}
