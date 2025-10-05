namespace Web.Models.DTO
{
    public class ResultResponse<T>
    {
        public string Message { get; set; }
        public bool Value { get; set; }
        public T Data { get; set; }

        public ResultResponse() { }

        public ResultResponse(string message, bool value, T data = default)
        {
            Message = message;
            Value = value;
            Data = data;
        }
    }
}
