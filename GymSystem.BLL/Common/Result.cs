using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Common
{
    public sealed record Result(bool Success, string? Error = null , ResultType Type = ResultType.Ok)
    {
        public static Result Ok() => new(true);
        public static Result Fail (string error , ResultType type = ResultType.Conflict) => new(false, error, type);
        public static Result NotFound(string error = "Not Found") => new(false, error, ResultType.NotFound);
        public static Result Validation(string error) => new(false, error, ResultType.ValidationFailed);
    }

    public sealed record Result<T>(bool Success, T? Value,string? Error = null, ResultType Type = ResultType.Ok)
    {
        public static Result<T> Ok(T value) => new(true, value);
        public static Result<T> Fail(string error, ResultType type = ResultType.Conflict) => new(false, default, error, type);
        public static Result<T> NotFound(string error = "Not Found") => new(false, default, error, ResultType.NotFound);
    }


    public enum ResultType
    {
       Ok,
       NotFound,
       Conflict,
       ValidationFailed,
       Forbidden,
    }
}
