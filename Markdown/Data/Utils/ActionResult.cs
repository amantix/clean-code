using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Utils
{
    public class ActionResult<T>(bool isSuccess, T data, string? errorMessage)
    {
        public bool IsSuccess { get; set; } = isSuccess;
        public string? ErrorMessage { get; set; } = errorMessage;
        public T Data { get; set; } = data;

        public static ActionResult<T> Ok(T data) => new(true, data, null);
        public static ActionResult<T?> Fail(string error) =>
            new(false, default, error);
        public ActionResult<F> withData<F>(Func<T, F> func) => new(IsSuccess, func(Data), ErrorMessage);
    }

    public class ActionResult(bool isSuccess, string? errorMessage)
    {
        public bool IsSuccess { get; set; } = isSuccess;
        public string? ErrorMessage { get; set; } = errorMessage;

        public static ActionResult Ok() => new(true, null);
        public static ActionResult Fail(string error) =>
            new(false, error);
    }
}
