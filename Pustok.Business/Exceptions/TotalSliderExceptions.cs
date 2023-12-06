namespace Pustok.Exceptions
{
    public class TotalSliderExceptions : Exception
    {
        public string Prop { get; set; }
        public TotalSliderExceptions()
        {
        }

        public TotalSliderExceptions(string prop,string? message) : base(message)
        {
            Prop = prop;
        }
    }
}
