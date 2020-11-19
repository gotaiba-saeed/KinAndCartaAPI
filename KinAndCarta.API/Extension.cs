using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace KinAndCarta.API
{
    public class Extension
    {
        public class ValidateModelAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(HttpActionContext actionContext)
            {
                if (actionContext.ModelState.IsValid == false)
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest, actionContext.ModelState);
                }
            }
        }
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class ValidatePhone : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string workPhone = (string)validationContext.ObjectType.GetProperty("workPhone").GetValue(validationContext.ObjectInstance, null);

                string personalPhone = (string)validationContext.ObjectType.GetProperty("personalPhone").GetValue(validationContext.ObjectInstance, null);

                //check at least one has a value
                if (string.IsNullOrEmpty(workPhone) && string.IsNullOrEmpty(personalPhone))
                    return new ValidationResult("Work phone or personal phone must be set");

                return ValidationResult.Success;
            }
        }
    }
}