namespace ExcelAnalyst.Domain.Global
{
    public record Error(string Code, string Message)
    {
        public record General
        {
            public static Error None = new(string.Empty, string.Empty);
            public static Error NullValue = new("Error.NullValue", "ادخل قيم صالحة");
            public static Error NoResult = new("Error.NoResult", "لاتوجد نتائج");
            public static Error FailedOperation = new("Error.FailedOperation", "خطأ في تنفيذ العملية.");
        }

        public record EFCore
        {
            public static Error NoChanges = new("EFCore.NoChanges", "لم يتم عمل اي تعديل");
            public static Error FailedTransaction = new("EFCore.FailedTransaction", "خطا في السيرفر");
        }

        public record User
        {
            public static Error EmailAlreadyExists = new("User.EmailAlreadyExists", "اسم المستخدم او الايميل موجود مسبقا");
            public static Error CreationFailed = new("User.CreationFailed", "فشل في اتمام العملية");
            public static Error InvalidCredentials = new("User.InvalidCredentials", "اسم المستخدم او كلمة المرور غير صحيحة");
            public static Error AcountLocked = new("User.AccountLocked", "تم غلق حسابك رجاء تواصل مع الدعم");
            public static Error AttempExceeded = new("User.AttemptExceeded", "لقد تم ادخال كلمة مرور فوق الحد, رجاء حاول لاحقا");
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
