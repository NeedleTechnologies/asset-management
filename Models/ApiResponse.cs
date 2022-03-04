using System.Diagnostics.CodeAnalysis;

namespace asset_management.Models
{
    public class ApiResponse<T>
    {
        public bool status { get; set; }
        public string message{ get; set; }
        [AllowNull]
        public T data { get; set; }
    }

    public class ApiResponse:ApiResponse<object>{

    }
}