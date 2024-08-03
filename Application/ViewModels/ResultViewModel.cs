namespace Application.ViewModels
{
    public class ResultViewModel<T>
    {
        public T? Data { get; }
        public bool IsSuccess { get; }
        public string Message { get; }

        public ResultViewModel(T? data, bool isSuccess, string? message)
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message ?? string.Empty;
        }
    }
}
