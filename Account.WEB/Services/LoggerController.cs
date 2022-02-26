using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Account.Services
{
    public class ExceptionService : IExceptionFilter
    {
      
        public  void OnException(ExceptionContext context)
        {
             context.Result = new RedirectResult("~/Home/Error/");
            context.ExceptionHandled = true;
        }
    }

}
