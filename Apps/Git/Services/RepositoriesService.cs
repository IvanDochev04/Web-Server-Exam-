using Git.Data;
using Git.Data.Models;
using Git.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Git.Services
{
    public class RepositoriesService : IRepositoriesService
    {
        private readonly ApplicationDbContext db;

        public RepositoriesService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void CreateRepository(string name, bool isPublic, string userId)
        {
            Repository repository = new Repository
            {
                Name = name,
                IsPublic = isPublic,
                CreatedOn = DateTime.UtcNow,
                OwnerId = userId,
                
                
            };
            this.db.Repositories.Add(repository);
            this.db.SaveChanges();
        }

        public List<RepositoryViewModel> GetAllPublic()
        {
            List<RepositoryViewModel> models = new List<RepositoryViewModel>();

            var repos = this.db.Repositories
                .Where(x => x.IsPublic == true)
                .Select(x => new RepositoryViewModel
                {
                    Id = x.Id,
                    Name=x.Name,
                    UserName=x.Owner.Username,
                    CreatedOn = x.CreatedOn.ToString("g",CultureInfo.InvariantCulture),
                    CommitsCount = x.Commits.Count()
                    

                }).ToList();
            models.AddRange(repos);
            return models;
        }
    }
}
