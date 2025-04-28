using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;
using System.Data.SqlClient;

namespace DisasterAlleviationFoundation.Pages.Accounts
{
    public class userRegistrationModel : PageModel
    {
        //declaring varibales
        public string firstname = string.Empty;
        public string lastName = string.Empty;
        public string email = string.Empty;
        public string password = string.Empty;
        public string role = string.Empty;
        public string message = string.Empty;

        public void OnGet()
        {
        }

        //on post method
        public void OnPost()
        {
            //requesting form values 
            firstname = Request.Form["firstName"];
            lastName = Request.Form["lastName"];
            email = Request.Form["email"];
            password = Request.Form["password"];
            role = "User";

            //calling the method for registration
            //if to check returning string 
            if (DbContext.Registration(firstname, lastName, email, password, role).Equals("Successful"))
            {
                //response redirect
                Response.Redirect("/Accounts/Login");
            }
            else
            {
                //to alert the user
                message = DbContext.Registration(firstname, lastName, email, password, role);

            }



        }
    }
}
