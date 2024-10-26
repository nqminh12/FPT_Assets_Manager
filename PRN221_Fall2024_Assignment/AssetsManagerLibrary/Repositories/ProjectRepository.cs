using AssetsManagerLibrary.Management;
using AssetsManagerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagerLibrary.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public Project GetProjectByID(int projectId) => ProjectManagement.Instance.GetProjectByID(projectId);

        public IEnumerable<Project> GetProjects() => ProjectManagement.Instance.GetProjectsList();

        public void InsertProject(Project project) => ProjectManagement.Instance.AddNew(project);

        public void DeleteProject(Project project) => ProjectManagement.Instance.Remove(project);

        public void UpdateProject(Project project) => ProjectManagement.Instance.Update(project);
    }
}
