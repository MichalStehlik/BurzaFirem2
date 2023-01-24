namespace BurzaFirem2.ViewModels
{
    public class ListVM<T>
    {
        public int Total { get; set; }
        public int Filtered { get; set; }
        public int Count { get; set; }
        public int Page { get; set; }
        public int Pagesize { get; set; }
        public int Pages { get { return ((Pagesize == 0) ? 0 : (int)Math.Ceiling((double)Filtered / Pagesize)); } }
        public List<T> Data { get; set; } = new List<T>();
    }
}
