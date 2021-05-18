

using Git.ViewModels;
using System.Collections.Generic;

namespace Git.Services
{
   public interface ICommitsService
    {
        void CreateCommit(string desctription, string creatorId, string repositoryId);

        List<CommitsViewModel> GetAll(string userId);

        bool Delete(string commitId,string userId);

        CommitsCreateViewModel GetRepository(string repositoryId);

     
    }
}
