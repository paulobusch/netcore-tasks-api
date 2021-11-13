using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasks.Domain._Common.Results;

namespace Tasks.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class TasksControllerBase : ControllerBase
    {
        protected Result GetResult(Result result)
        {
            Response.StatusCode = (int)result.Status;
            return result;
        }

        protected Result<T> GetResult<T>(Result<T> result) where T : class
        {
            Response.StatusCode = (int)result.Status;
            return result;
        }

        protected Result<T> GetResult<T>(T data) where T : class
        {
            return new Result<T>(data);
        }
    }
}
