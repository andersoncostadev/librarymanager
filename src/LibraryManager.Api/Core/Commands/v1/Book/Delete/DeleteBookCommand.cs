using MediatR;

namespace Core.Commands.v1.Book.Delete
{
    public class DeleteBookCommand : IRequest<DeleteBookCommandResponse>
    {
        public DeleteBookCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
