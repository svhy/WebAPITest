using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using Microsoft.Extensions.Logging;

namespace WebAPITest.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {

        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;
            _logger.LogError("出现未知异常:{0}", ex.Message);

            var requestData = context.HttpContext.Request.Headers.ContainsKey("x-requested-with");
            bool IsAjax = false;
            if (requestData)
            {
                IsAjax = context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
            }

            //测试
            if (!IsAjax)
            {
                context.Result = new JsonResult(new { Code = 500, Msg = "System Error." });
            }
            else
            {
                context.Result = new JsonResult(new { Code = 500, Msg = "System Error." });
            }
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
        }
    }
}
