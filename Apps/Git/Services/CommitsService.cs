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
    public class CommitsService : ICommitsService
    {
        private readonly ApplicationDbContext db;

        public CommitsService(ApplicationDbContext db)
        {
            this.db = db;
        }

       

        public void CreateCommit(string desctription, string creatorId, string repositoryId)
        {
            Commit commit = new Commit
            {
                Description = desctription,
                CreatedOn = DateTime.UtcNow,
                Creator=this.db.Users.FirstOrDefault(x=>x.Id==creatorId),
                RepositoryId = repositoryId,
                
                
            };
            this.db.Commits.Add(commit);
            this.db.SaveChanges();
        }

        public bool Delete(string commitId, string userId)
        {
            var commit = this.db.Commits.FirstOrDefault(x => x.Id == commitId && x.CreatorId == userId);
            if (commit==null)
            {
                return false;
            }
            if (commit.CreatorId != userId)
            {
                return false;
            }
            this.db.Commits.Remove(commit);
            this.db.SaveChanges();
            return true;

        }

        public List<CommitsViewModel> GetAll(string userId)
        {
            List<CommitsViewModel> commits = new List<CommitsViewModel>();

            var comms = this.db.Commits.Where(x=> x.Creator.Id == userId).Select(x => new CommitsViewModel
            {
                Id=x.Id,
                CreatedOn=x.CreatedOn.ToString("g", CultureInfo.InvariantCulture),
                Description = x.Description,
                RepositoryName = x.Repository.Name


            }).ToList();
            commits.AddRange(comms);
            return commits;

        }

        public CommitsCreateViewModel GetRepository(string repositoryId)
        {
            var model = this.db.Repositories
                .Where(x => x.Id == repositoryId)
                .Select(x => new CommitsCreateViewModel 
                {
                    Name =x.Name,
                    Id = x.Id
                }).FirstOrDefault();
            return model;
            
        }
    }
}
