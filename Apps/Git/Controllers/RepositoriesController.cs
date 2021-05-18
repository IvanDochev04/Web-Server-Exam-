using Git.Services;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Git.Controllers
{
    public class RepositoriesController:Controller
    {
        private readonly IRepositoriesService service;

        public RepositoriesController(IRepositoriesService service)
        {
            this.service = service;
        }
        public HttpResponse All()
        {
            var models = this.service.GetAllPublic();
            return this.View(models);
        }
        public HttpResponse Create()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            
            return this.View();
        }
        [HttpPost]
        public HttpResponse Create(string name,string repositoryType)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            
            if (string.IsNullOrEmpty(name)||name.Length<3 ||name.Length>10)
            {
                return this.Error("Name must be between 3 and 10 characters.");
            }
            if (string.IsNullOrEmpty(repositoryType)||repositoryType != "Public" && repositoryType != "Private")
            {
                return this.Error("Invalid repository type. Alowed \"Private\" and \"Public\".");
            }
            bool type = false;
            if (repositoryType == "Public")
            {
                type = true;
            }
            this.service.CreateRepository(name, type, this.GetUserId());
            return this.Redirect("/Repositories/All");
        }
    }
}
