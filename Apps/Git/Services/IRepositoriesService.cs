
using Git.ViewModels;
using System.Collections.Generic;

namespace Git.Services
{
   public interface IRepositoriesService
    {
        void CreateRepository(string name, bool isPublic,string userId);

        List<RepositoryViewModel> GetAllPublic();

        

    }
}
