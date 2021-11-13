using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Tasks.Domain._Common.Enums;

namespace Tasks.Domain._Common.Results
{
    public class Result<T>
    {
        [JsonIgnore] public Status Status { get; protected set; }
        public int? TotalRows { get; protected set; }
        public T Data { get; protected set; }
        public bool Success => Status == Status.Success && !_errorMessages.Any();
        public IEnumerable<string> ErrorMessages => _errorMessages.Any() ? _errorMessages.AsReadOnly() : null;

        protected readonly List<string> _errorMessages;

        public Result()
        {
            Status = Status.Success;
            _errorMessages = new List<string>();
        }

        public Result(T data, int? total = null) : this()
        {
            Data = data;
            TotalRows = total;
        }

        public Result(Status status, string error) : this()
        {
            _errorMessages.Add(error);
            Status = status;
        }

        public Result(Status status, IEnumerable<string> errors) : this()
        {
            if (errors != null) _errorMessages.AddRange(errors);
            Status = status;
        }

        public Result(Status status, IEnumerable<string> errors, T data) : this()
        {
            if (errors != null) _errorMessages.AddRange(errors);
            Status = status;
            Data = data;
        }
    }

    public class Result : Result<dynamic>
    {
        public Result() : base() { }

        public Result(dynamic data) : base()
        {
            Data = data;
        }

        public Result(Status status, string error) : base(status, error) { }

        public Result(Status status, IEnumerable<string> errors) : base(status, errors) { }
    }
}
