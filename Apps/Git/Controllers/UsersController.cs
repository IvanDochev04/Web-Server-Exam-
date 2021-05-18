using Git.Services;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Git.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService service;

        public UsersController(IUsersService service)
        {
            this.service = service;
        }
        public HttpResponse Login()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            return this.View();
        }
        [HttpPost]
        public HttpResponse Login(string username,string password)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            var userId = this.service.GetUserId(username, password);
            if (userId==null)
            {
                return this.Error("Invalid Username or Password");
            }
            this.SignIn(userId);
            return this.Redirect("/Repositories/All");
        }

        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            return this.View();
        }
        [HttpPost]
        public HttpResponse Register(string username,string email, string password,string confirmPassword)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            if (string.IsNullOrEmpty(username) || username.Length < 5 || username.Length > 20)
            {
                return this.Error("Username must be between 5 and 20 characters.");
            }

            if (string.IsNullOrEmpty(email) || !new EmailAddressAttribute().IsValid(email))
            {
                return this.Error("Invalid email.");
            }

            if (string.IsNullOrEmpty(password) || password.Length < 6 || password.Length > 20)
            {
                return this.Error("Password must be between 6 and 20 characters.");
            }

            if (confirmPassword != password)
            {
                return this.Error("Passwords do not match.");
            }

            if (!this.service.IsEmailAvailable(email))
            {
                return this.Error("Email already taken.");
            }

            if (!this.service.IsUsernameAvailable(username))
            {
                return this.Error("Username already taken.");
            }

            this.service.CreateUser(username, email, password);
            return this.Redirect("/Users/Login");
        }
        public HttpResponse Logout()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.SignOut();
            return this.Redirect("/");
        }

    }
}
