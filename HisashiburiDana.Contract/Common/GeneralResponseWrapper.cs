using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.Common
{
    public class GeneralResponseWrapper<T>
    {
        private readonly ILogger _logger;


        public GeneralResponseWrapper(T data)
        {
            Data = data;
            HasError = false;
            Errors = null;
            TimeGenerated = DateTime.UtcNow.AddHours(1);
        }

        public GeneralResponseWrapper(List<string> errors)
        {
            HasError = true;
            Errors = errors;
            TimeGenerated = DateTime.UtcNow.AddHours(1);
        }

        public GeneralResponseWrapper(ILogger logger)
        {
            _logger = logger;
        }

        public T? Data { get; set; }

        public bool HasError { get; set; }

        public List<string>? Errors {get; set;} 

        public DateTime TimeGenerated { get; set; }

        
        
        public GeneralResponseWrapper<T> BuildSuccessResponse(T data, [CallerMemberName] string caller = "")
        {
            _logger.LogInformation($"{caller} Success Response Sent");
            return new(data);
        } 

        public GeneralResponseWrapper<T> BuildFailureResponse(List<string> errors, [CallerMemberName] string caller = "")
        {
            _logger.LogInformation($"{caller} Failure response {string.Join(" , ",errors)}");
            return new(errors);
        }
    }
}
