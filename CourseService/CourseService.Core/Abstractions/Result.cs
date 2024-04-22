using CourseService.Application.Abstractions.Errors;

namespace CourseService.Application.Abstractions {
    public class Result<TValue, TError> {
        private readonly TValue? _value;
        private readonly TError? _error;
        public TValue? Value => IsSuccess ? _value : throw new Exception();
        public TError? Error => _error;

        private bool _success;

        public bool IsSuccess => _success;

        public Result(TValue? value) {
            _success = true;
            _value = value;
            _error = default;
        }

        public Result(TError error) {
            _success = false;
            _value = default;
            _error = error;
        }

        //happy path
        public static implicit operator Result<TValue, TError>(TValue? value) => new Result<TValue, TError>(value);
        //error path
        public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);

        public Result<TValue, TError> Match(Func<TValue, Result<TValue, TError>> success, Func<TError, Result<TValue, TError>> failure) {
            if (_success) {
                return success(_value!);
            }
            return failure(_error!);
        }
    }

    public class Result<TValue> : Result<TValue, Error> {
        public Result(TValue? value) : base(value) {}
        public Result(Error error) : base(error) { }

        //happy path
        public static implicit operator Result<TValue>(TValue? value) => new Result<TValue>(value);
        //error path
        public static implicit operator Result<TValue>(Error error) => new Result<TValue>(error);

        public static Result<TValue> Ok(TValue value) => new(value);
        public static Result<TValue> Failure(Error error) => new(error);
    }

    public class Result : Result<Object> {
        public Result() : base(value: null) { }
        public Result(Error error) : base(error) { }
        public static implicit operator Result(Error error) => new Result(error);
        //public static implicit operator Result() => new Result();
        public static Result Ok() => new();
    }

    //public static class Result {
    //    public static Result<TValue> Ok<TValue>(TValue value) => Result<TValue>.Ok(value);
    //    public static Result<Error> Failure(Error error) => Result<TValue>.Ok(error);
    //}
    //public class TEST {
    //    public void yui() {
    //        var t = Result<int>.Ok(56);
    //    }
    //}
}
