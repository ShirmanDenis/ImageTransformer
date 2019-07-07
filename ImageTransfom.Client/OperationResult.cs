using System;

namespace ImageTransform.Client
{
    public class OperationResult<T>
    {
        private T _result;
        private OperationResult()
        { }
        public T Result => EnsureSuccess()._result;
        public string ErrorMsg { get; private set; } = string.Empty;

        public OperationResult<T> EnsureSuccess()
        {
            if (!string.IsNullOrEmpty(ErrorMsg))
                throw new Exception(ErrorMsg);
            return this;
        }

        public static OperationResult<T> CreateOk(T result)
        {
            return new OperationResult<T>()
            {
                _result = result,
            };
        }

        public static OperationResult<T> CreateFailed(string errorMsg)
        {
            return new OperationResult<T>()
            {
                _result = default,
                ErrorMsg = errorMsg
            };
        }
    }
}