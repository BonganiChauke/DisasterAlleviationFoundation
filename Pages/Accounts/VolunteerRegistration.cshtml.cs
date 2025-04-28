using DisasterAlleviationFoundation.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisasterAlleviationFoundation.Pages.Accounts
{
    public class VolunteerRegistrationModel : PageModel
    {
        //declaring varibales
        public string? firstname = "", lastName = "", email = "", password = "", role = "", message = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            //requesting form values 
            firstname = Request.Form["firstName"];
            lastName = Request.Form["lastName"];
            email = Request.Form["email"];
            password = Request.Form["password"];
            role = "Volunteer";


#pragma warning disable CS8604 // Possible null reference argument.
            //if to check returning string 
            if (DbContext.Registration(firstname, lastName, email, password, role).Equals("Successful"))
            {
                //response redirect
                Response.Redirect("/Accounts/Login");
            }
            else
            {
#pragma warning disable CS8604 // Possible null reference argument.
                message = DbContext.Registration(firstname, lastName, email, password, role);


            }
        }
    }
}
