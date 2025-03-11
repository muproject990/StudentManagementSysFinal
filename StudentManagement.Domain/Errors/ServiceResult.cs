namespace StudentManagement.Domain.Errors
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public T Data { get; }

        // Constructor is private to enforce the usage of the two static methods
        private ServiceResult(bool isSuccess, T data, string errorMessage)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static ServiceResult<T> AsSuccess(T data)
        {
            return new ServiceResult<T>(true, data, null);
        }

        public static ServiceResult<T> AsFailure(string errorMessage)
        {
            return new ServiceResult<T>(false, default, errorMessage);
        }
    }
}
