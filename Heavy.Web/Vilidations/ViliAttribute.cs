using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Heavy.Web.Vilidations
{
    //扩展特性校验model  //类似[MaxLength(18)]的东西
    public class ViliAttribute : Attribute, IModelValidator
    {
        public string Error { get; set; }
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            
            var url = context.Model as string;
            if (url!=null&& Uri.IsWellFormedUriString(url,UriKind.Absolute))
            {
                return Enumerable.Empty<ModelValidationResult>();
            }

            return new List<ModelValidationResult>() {
                new ModelValidationResult(string.Empty,Error)
            };
        }
    }
}
