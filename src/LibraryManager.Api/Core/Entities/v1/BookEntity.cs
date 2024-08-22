using Core.Enums.v1;

namespace Core.Entities.v1
{
    public class BookEntity : BaseEntity
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public int YearPublication { get; set; }
        public BookStatus? Status { get; set; }
    }
}
