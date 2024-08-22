namespace Core.Entities.v1
{
    public class LoanEntity : BaseEntity
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public BookEntity? Book { get; set; }
        public UserEntity? User { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
