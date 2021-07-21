namespace ConditionalLinq.Models
{
    public class Pager
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsValid => PageIndex >= 0 && PageSize > 0;
    }
}
