using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.Models;
using Microsoft.AspNetCore.Http;
using Login.Filter;
using System.Net.Mail;
using System.Net;

namespace Login.Controllers
{

    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private string code = null;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }


        public IActionResult ForgotPassword()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }


        public IActionResult ResetPassword()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public IActionResult SendCode(string email)
        {
            var user = _context.User.FirstOrDefault(w => w.Email.Equals(email));
            if (user != null)
            {
                _context.Add(new PasswordCode { UserId = user.Id, Code = getCode() });
                _context.SaveChanges();
                string text = "sıfırlama işlemi: " + getCode();

                MailMessage msg = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("sevvale685@gmail.com", "sevval.22");
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                msg.To.Add(email);
                msg.From = new MailAddress("sevvale685@gmail.com");
                msg.Subject = "Email doğrulaması";
                msg.Body = text;
                client.Send(msg);
                return Redirect("ResetPassword");


            }
            return Redirect("Index");
        }


        public IActionResult ResetPasswordCode(string code, string newPassword)
        {
            var passwordcode = _context.PasswordCode.FirstOrDefault(w => w.Code.Equals(code));
            if (passwordcode != null)
            {
                var user = _context.User.Find(passwordcode.UserId);
                user.Password = newPassword;
                _context.Update(user);
                _context.Remove(passwordcode);
                _context.SaveChanges();
                return Redirect("Index");
            }
            return Redirect("Index");

        }


        public IActionResult Login(string email, string pass)
        {
            var user = _context.User.FirstOrDefault(w => w.Email.Equals(email) && w.Password.Equals(pass));

            if (user != null) {
                HttpContext.Session.SetInt32("id", user.Id);
                HttpContext.Session.SetString("fullname", user.Name + " " + user.Surname);
                return Redirect("/Home/Index");
            }

            return Redirect("Index");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("Index");
        }

       public IActionResult ChangesPass(string currentpass, string newpass)
     
        {
            var user = _context.User.FirstOrDefault(w => w.Password.Equals(currentpass));
            if (user != null )
            {
                HttpContext.Session.SetInt32("id", user.Id);
                user.Password = newpass;
                _context.Update(user);
                _context.Remove(newpass);
                _context.SaveChanges();
                return Redirect("/Home/Index");
            }
            return Redirect("/Home/Index");

        }
        public IActionResult changes()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("ChangesPassword");
            }
            return  View();
        }
        public IActionResult SignUp()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }


        public async Task<IActionResult> Register(User model)
        {
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();

            return Redirect("Index");
        }
        public string getCode()
        {
            if(code == null)
            {
                Random rand = new Random();
                code = "";
                for(int i = 0; i < 7; i++)
                {
                    char tmp = Convert.ToChar(rand.Next(48, 58)); //ascii değer elde ederiz 0-9 arasında bir değer
                    code +=tmp;
                }
            }
            return code;
        }
    }
}
