using MediatR;

namespace Core.Commands.v1.Book.Update
{
    public class UpdateBookCommand : IRequest<UpdateBookCommandResponse>
    {
        public UpdateBookCommand(string? title, string? author, string? iSBN, int yearPublication)
        {
            Title = title;
            Author = author;
            ISBN = iSBN;
            YearPublication = yearPublication;
        }

        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public int YearPublication { get; set; }
    }
}
