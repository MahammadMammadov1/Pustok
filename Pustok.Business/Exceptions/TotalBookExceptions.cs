namespace Pustok.Exceptions
{
    public class TotalBookExceptions : Exception
    {
        public string Prop { get; set; }
        public TotalBookExceptions()
        {
        }

        public TotalBookExceptions(string prop,string? message) : base(message)
        {
            Prop = prop;
        }
    }
}
