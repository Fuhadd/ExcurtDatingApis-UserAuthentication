using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthenticationService.Core.Models.Response
{
    public class ApiResponse<T>
    {
        public bool? success { get; set; }
        public string? message { get; set; }
#nullable enable
        public object? errors { get; set; }
#nullable disable
        [MaybeNull, AllowNull]
        public T data { get; set; }
    }

}
