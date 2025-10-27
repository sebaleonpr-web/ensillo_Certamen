using Microsoft.AspNetCore.Mvc;

namespace enso_Certamen.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}

       /* [HttpPost]

        public IActionResult Login(String email, String password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _logger = Logger;
            }
        }
  
}  }*/
