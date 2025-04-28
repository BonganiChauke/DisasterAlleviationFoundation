using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Model;
using System.Data;

namespace DisasterAlleviationFoundation.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        //declaring variables
        public string? email = "", password = "", message = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            //requesting form values
            email = Request.Form["email"];
            password = Request.Form["password"];


            // Call the method to get the user role
            string role = DbContext.Login(email, password);

            //if role method returns null
            if (role == null)
            {
                //getting alert from the method 
                message = DbContext.Login(email, password);

            }

            //check user role
            if (!string.IsNullOrEmpty(role))
            {
                //using if to check user roles and redirect user to dashboard pass email to use
                if (role.Equals("Admin"))
                {
                    //redirect user
                    Response.Redirect("/AdminDashboard/AdminDashboard?email=" + email);
                }
                else if (role.Equals("Volunteer"))
                {
                    //redirect user
                    Response.Redirect("/VolunteerDashboard/VolunteerDashboard?email=" + email);
                }
                else if (role.Equals("User"))
                {
                    //redirect user
                    Response.Redirect("/UserDashboard/UserDashboard?email=" + email);
                }
                else
                {
                    //message 
                    message = "Can not login at this time... please contact administration for further assistance.";
                }
            }
            else
            {
                //message 
                message = "Invalid email or password.";
            }

        }
    }
}
