using Git.Services;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Git.Controllers
{
    public class CommitsController : Controller
    {
        private readonly ICommitsService service;

        public CommitsController(ICommitsService service)
        {
            this.service = service;
        }
        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
           
            var models= service.GetAll(this.GetUserId());
            return this.View(models);
        }
        public HttpResponse Create(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            
            var model = service.GetRepository(id);
            if (model == null)
            {
                return this.Error("No such repository exsist.");
            }
            return this.View(model);
        }
        [HttpPost]
        public HttpResponse Create(string description,string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            
            if (string.IsNullOrEmpty( description)||description.Length<5)
            {
                return this.Error("Description must be at least 5 characters.");
            }
            service.CreateCommit(description,  this.GetUserId(),id);
            return this.Redirect("/Repositories/All");
        }
        public HttpResponse Delete(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            
            bool isDeleted = service.Delete(id, this.GetUserId());
                if (!isDeleted)
            {
                return this.Error("Could not delete !");
            }
            return this.Redirect("/Commits/All");
        }
    }
}
