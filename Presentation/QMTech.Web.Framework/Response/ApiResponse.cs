using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMTech.Web.Framework
{
    /// <summary>
    /// Web API
    /// </summary>
    public class ApiResponseResult
    {
        public string Message { get; set; }

        public int Code { get; set; }
    }

    public class ApiResponseResult<T>:ApiResponseResult
    {
        public T Object { get; set; }
    }
}
