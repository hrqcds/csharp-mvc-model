namespace Generics
{
    public class DataResponse<T>
    {
        public List<T> Data { get; set; } = new List<T>();

        public int Total { get; set; }
    }
}