using System;

namespace Tasks.Domain.Developers.Dtos.Ranking
{
    public class DeveloperRankingSearchDto
    {
        public Guid? ProjectId { get; set; }
        public DateTime? StartTime { get; set; }

        public DeveloperRankingSearchDto()
        {
            StartTime = DayOfLastWeekDay(DayOfWeek.Monday);
        }

        private DateTime DayOfLastWeekDay(DayOfWeek startOfWeek)
        {
            var now = DateTime.Now;
            var diff = (7 + (now.DayOfWeek - startOfWeek)) % 7;
            return now.AddDays(-1 * diff).Date;
        }
    }
}
