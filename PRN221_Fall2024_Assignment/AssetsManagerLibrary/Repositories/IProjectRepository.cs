using AssetsManagerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagerLibrary.Repositories
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetProjects();
        Project GetProjectByID(int projectId);
        void InsertProject(Project project);
        void DeleteProject(Project project);
        void UpdateProject(Project project);
    }
}
