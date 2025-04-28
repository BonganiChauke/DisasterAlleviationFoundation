using DisasterAlleviationFoundation.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisasterAlleviationFoundation.Pages.Accounts
{
    public class ResetPasswordModel : PageModel
    {
        //declaring varibales 
        public string email = "", password = "", message = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            //requesting form variables
            email = Request.Form["email"];
            password = Request.Form["password"];

            if (DbContext.ResetPassword(email, password).Equals("Success"))
            {
                message = "Success";
                Response.Redirect("/Accounts/Login");
            }
            else
            {
                message = DbContext.ResetPassword(email, password);
            }
        }
    }
}
