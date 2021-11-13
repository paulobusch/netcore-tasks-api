using System;

namespace Tasks.Domain._Common.Session
{
    public interface IAutenticationContext
    {
        public Guid Id { get; }
        public string Login { get; }
    }
}
