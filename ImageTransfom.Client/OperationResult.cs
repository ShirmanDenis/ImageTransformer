using System;

namespace ImageTransform.Client
{
    public class OperationResult<T>
    {
        private OperationResult()
        { }
        public T Result { get; private set; }

        public string ErrorMsg { get; private set; } = string.Empty;

        public bool IsSuccessful => string.IsNullOrEmpty(ErrorMsg);

        public static OperationResult<T> CreateOk(T result)
        {
            return new OperationResult<T>()
            {
                Result = result,
            };
        }

        public static OperationResult<T> CreateFailed(string errorMsg)
        {
            return new OperationResult<T>()
            {
                Result = default,
                ErrorMsg = errorMsg
            };
        }
    }
}