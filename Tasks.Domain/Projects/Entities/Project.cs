using System;
using System.Collections.Generic;
using System.Linq;
using Tasks.Domain._Common.Entities;
using Tasks.Domain.Developers.Entities;

namespace Tasks.Domain.Projects.Entities
{
    public class Project : EntityBase
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        public virtual IReadOnlyCollection<DeveloperProject> DeveloperProjects => _developerProjects.AsReadOnly();

        private readonly List<DeveloperProject> _developerProjects;

        protected Project() : base()
        {
            _developerProjects = new List<DeveloperProject>();
        }

        public Project(
            Guid id,
            string title,
            string description,
            IEnumerable<DeveloperProject> developerProjects = null
        ) : base(id)
        {
            _developerProjects = new List<DeveloperProject>();
            SetData(
                title: title,
                description: description,
                developerProjects: developerProjects
            );
        }

        public void SetData(
            string title,
            string description,
            IEnumerable<DeveloperProject> developerProjects = null
        )
        {
            this.Title = title;
            this.Description = description;
            this.UpdateDeveloperProjects(developerProjects);
        }

        private void UpdateDeveloperProjects(IEnumerable<DeveloperProject> developerProjects)
        {
            if (developerProjects == null) return;
            _developerProjects.RemoveAll(dp => !developerProjects.Any(a => a.DeveloperId == dp.DeveloperId));
            _developerProjects.AddRange(developerProjects.Where(dp => !_developerProjects.Any(a => a.DeveloperId == dp.DeveloperId)));
        }
    }
}
