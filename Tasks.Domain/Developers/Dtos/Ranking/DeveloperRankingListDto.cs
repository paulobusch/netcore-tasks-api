using System;

namespace Tasks.Domain.Developers.Dtos.Ranking
{
    public class DeveloperRankingListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float AvgHours { get; set; }
        public float SumHours { get; set; }
    }
}
