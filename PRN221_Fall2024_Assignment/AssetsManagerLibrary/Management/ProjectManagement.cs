using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetsManagerLibrary.Models;
using System.Numerics;

namespace AssetsManagerLibrary.Management
{
    public class ProjectManagement
    {
        private static ProjectManagement instance = null;
        private static readonly object instanceLock = new object();
        private ProjectManagement() { }

        public static ProjectManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProjectManagement();
                    }
                    return instance;
                }
            }
        }

        //------------------------------------------------------------------------------
        public IEnumerable<Project> GetProjectsList()
        {
            List<Project> projects;
            try
            {
                var myDB = new AssetManagerContext();
                projects = myDB.Projects.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return projects;
        }

        //------------------------------------------------------------------------------
        public Project GetProjectByID(int projectID)
        {
            Project project = null;
            try
            {
                var myDB = new AssetManagerContext();
                project = myDB.Projects.SingleOrDefault(p => p.ProjectId == projectID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return project;
        }

        //------------------------------------------------------------------------------
        public void AddNew(Project project)
        {
            try
            {
                Project _project = GetProjectByID(project.ProjectId);
                if (_project == null)
                {
                    var myDB = new AssetManagerContext();
                    myDB.Projects.Add(project);
                    myDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The Project already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //------------------------------------------------------------------------------
        public void Update(Project project)
        {
            try
            {
                Project existingProject = GetProjectByID(project.ProjectId);
                if (existingProject != null)
                {
                    var myDB = new AssetManagerContext();
                    myDB.Entry<Project>(project).State = EntityState.Modified;
                    myDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The project does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //------------------------------------------------------------------------------
        public void Remove(Project project)
        {
            try
            {
                Project existingProject = GetProjectByID(project.ProjectId);
                if (existingProject != null)
                {
                    var myDB = new AssetManagerContext();
                    myDB.Projects.Remove(project);
                    myDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The project does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
