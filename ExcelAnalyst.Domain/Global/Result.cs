namespace ExcelAnalyst.Domain.Global
{
    public record Error(string Code, string Message)
    {
        public record General
        {
            public static Error None = new(string.Empty, string.Empty);
            public static Error NullValue = new("Error.NullValue", "Invalid Input");
            public static Error NoResult = new("Error.NoResult", "No Result");
            public static Error FailedOperation = new("Error.FailedOperation", "Error Aqured In The Operation.");
        }

        public record EFCore
        {
            public static Error NoChanges = new("EFCore.NoChanges", "No Chage Made");
            public static Error FailedTransaction = new("EFCore.FailedTransaction", "Error in server");
        }

        public record User
        {
            public static Error EmailAlreadyExists = new("User.EmailAlreadyExists", "Username Already Exists");
            public static Error CreationFailed = new("User.CreationFailed", "Failed To Complate The Operation");
            public static Error InvalidCredentials = new("User.InvalidCredentials", "Username Or Password Invalid");
            public static Error AcountLocked = new("User.AccountLocked", "Your Acount Has been locked");
            public static Error AttempExceeded = new("User.AttemptExceeded", "Your Trails Has Been Exceeded Try Later");
        }

    }

    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            switch (isSuccess)
            {
                case true when error != Error.General.None:
                    throw new InvalidOperationException();

                case false when error == Error.General.None:
                    throw new InvalidOperationException();

                default:
                    IsSuccess = isSuccess;
                    Error = error;
                    break;
            }
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result Success() => new(true, Error.General.None);
        public static Result Failure(Error error) => new(false, error);

        public static Result<T> Success<T>(T value) => new(value, true, Error.General.None);
        public static Result<T> Failure<T>(Error error) => new(default, false, error);

        public override string ToString()
        {
            return IsSuccess ? "Success" : $"Failure: {Error.Code} - {Error.Message}";
        }

    }

    public class Result<T> : Result
    {
        private readonly T? _value;

        protected internal Result(T? value, bool isSuccess, Error error) : base(isSuccess, error)
            => _value = value;

        public T Value => _value! ?? throw new InvalidOperationException("Result has no value");
    }
}
