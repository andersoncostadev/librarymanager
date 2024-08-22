using Core.Enums.v1;
using MediatR;

namespace Core.Commands.v1.Book.Create
{
    public class CreateBookCommand : IRequest<CreateBookCommandResponse>
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public int YearPublication { get; set; }
        public  BookStatus? Status { get; set; }
    }
}
