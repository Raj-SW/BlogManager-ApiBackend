namespace Model.Utils
{
    public class GenericResult<T>
    {
        public bool IsSuccess { get; set; } = true;
        public IList<string> ErrorMessage { get; set; } = [];
        public T? ResultObject { get; set; }
    }
}
