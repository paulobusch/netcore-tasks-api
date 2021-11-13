using System;
using Tasks.Domain._Common.Entities;
using Tasks.Domain.Developers.Entities;

namespace Tasks.Domain.Works.Entities
{
    public class Work : EntityBase
    {
        public Guid DeveloperProjectId  { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public string Comment { get; private set; }
        public float Hours { get; private set; }

        public virtual DeveloperProject DeveloperProject { get; private set; }

        protected Work() : base() { }

        public Work(
            Guid id,
            Guid developerProjectId,
            DateTime startTime,
            DateTime endTime,
            string comment,
            float hours
        ) : base(id)
        {
            this.DeveloperProjectId = developerProjectId;
            SetData(
                startTime: startTime,
                endTime: endTime,
                comment: comment,
                hours: hours
            );
        }

        public void SetData(
            DateTime startTime,
            DateTime endTime,
            string comment,
            float hours
        )
        {
            StartTime = startTime;
            EndTime = endTime;
            Comment = comment;
            Hours = hours;
        }
    }
}
