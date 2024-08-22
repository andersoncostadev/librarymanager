using Core.Enums.v1;

namespace Core.Commands.v1.Book.Create
{
    public class CreateBookCommandResponse
    {
        public CreateBookCommandResponse(Guid id, string? title, string? author, string? iSBN, int yearPublication, BookStatus? status)
        {
            Id = id;
            Title = title;
            Author = author;
            ISBN = iSBN;
            YearPublication = yearPublication;
            Status = status;
        }

        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public int YearPublication { get; set; }
        public BookStatus? Status { get; set; }
    }
}
