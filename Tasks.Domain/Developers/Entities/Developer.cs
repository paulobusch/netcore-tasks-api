using System;
using System.Collections.Generic;
using System.Linq;
using Tasks.Domain._Common.Crypto;
using Tasks.Domain._Common.Entities;

namespace Tasks.Domain.Developers.Entities
{
    public class Developer : EntityBase
    {
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string CPF { get; private set; }
        public string PasswordHash { get; private set; }

        public virtual IReadOnlyCollection<DeveloperProject> DeveloperProjects => _developerProjects.AsReadOnly();
        private readonly List<DeveloperProject> _developerProjects;

        protected Developer() : base() {
            _developerProjects = new List<DeveloperProject>();
        }

        public Developer(
            Guid id,
            string name,
            string login,
            string cpf,
            string password, 
            IEnumerable<DeveloperProject> developerProjects = null
        ) : base(id)
        {
            this.CPF = cpf;
            this.PasswordHash = MD5Crypto.Encode(password);
            _developerProjects = new List<DeveloperProject>();
            SetData(
                name: name,
                login: login,
                developerProjects: developerProjects
            );
        }

        public void SetData(
            string name,
            string login,
            IEnumerable<DeveloperProject> developerProjects = null
        )
        {
            this.Name = name;
            this.Login = login;
            this.UpdateDeveloperProjects(developerProjects);
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            var hash = MD5Crypto.Encode(password);
            return hash.Equals(PasswordHash);
        }

        private void UpdateDeveloperProjects(IEnumerable<DeveloperProject> developerProjects)
        {
            if (developerProjects == null) return;
            _developerProjects.RemoveAll(dp => !developerProjects.Any(a => a.ProjectId == dp.ProjectId));
            _developerProjects.AddRange(developerProjects.Where(dp => !_developerProjects.Any(a => a.ProjectId == dp.ProjectId)));
        }
    }
}
