using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Contract.Common
{
    public class GeneralResponseWrapper<T>
    {
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

        public GeneralResponseWrapper()
        {

        }

        public T? Data { get; set; }

        public bool HasError { get; set; }

        public List<string>? Errors {get; set;} 

        public DateTime TimeGenerated { get; set; }

        
        
        public GeneralResponseWrapper<T> BuildSuccessResponse(T data)
        {
            return new(data);
        } 

        public GeneralResponseWrapper<T> BuildFailureResponse(List<string> errors)
        {
            return new(errors);
        }
    }
}
