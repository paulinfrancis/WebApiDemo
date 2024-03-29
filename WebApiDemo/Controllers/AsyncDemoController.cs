﻿using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiDemo.Controllers
{
    public class AsyncDemoController : ApiController
    {
        // GET /api/AsyncDemo
        public Task<string> Get()
        {
            return AReallyslowAsyncMethod();
        }

        private async Task<string> AReallyslowAsyncMethod()
        {
            var slowTask = Task.Factory.StartNew(
              () =>
                  {
                      Thread.Sleep(10000);
                      return "Hello sleepy world - yawn...";
                  }
            );

            return await slowTask;
        }
    }
}
