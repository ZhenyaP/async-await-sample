using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AsyncAwaitSample.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        #region Private

        private async Task<string> GetHelloWorldAsync()
        {
            var message = $@"Hello World! <br/>
                Before await: ManagedThreadId = {Thread.CurrentThread.ManagedThreadId}";
            await Task.Delay(10000);
            message += $@"<br/>
                After await: ManagedThreadId = {Thread.CurrentThread.ManagedThreadId}";

            return message;
        }

        #endregion

        #region Public

        [HttpGet("HelloWorld")]
        public Task<string> HelloWorldAsync()
        {
            return GetHelloWorldAsync();
        }

        #endregion
    }
}
