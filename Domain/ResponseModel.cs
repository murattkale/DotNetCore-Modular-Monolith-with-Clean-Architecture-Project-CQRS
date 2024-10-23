namespace dotnetcoreproject.Domain;

public class ResponseModel<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int? TotalRecords { get; set; }

    public static ResponseModel<T> Success(T data, string message, int? totalRecords = null)
    {
        return new ResponseModel<T> { IsSuccess = true, Data = data, Message = message, TotalRecords = totalRecords };
    }

    public static ResponseModel<T> Failure(string message)
    {
        return new ResponseModel<T> { IsSuccess = false, Message = message };
    }
}