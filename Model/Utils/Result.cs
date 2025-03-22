namespace Model.Utils
{
    public class Result
    {
        public bool IsSuccess { get; set; } = true;
        public IList<string> ErrorMessage { get; set; } = [];
        public object? ResultObject { get; set; }
    }
}
