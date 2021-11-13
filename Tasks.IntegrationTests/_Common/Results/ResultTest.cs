using System.Collections.Generic;

namespace Tasks.IntegrationTests._Common.Results
{
    public class ResultTest<T> where T : class
    {
        public int? TotalRows { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
    }

    public class ResultTest : ResultTest<dynamic> { }
}
