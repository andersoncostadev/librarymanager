using Core.Entities.v1;

namespace Core.Queries.v1.Book.GetAll
{
    public class GetAllBookQueryResponse 
    {
        public GetAllBookQueryResponse(IEnumerable<BookEntity>? books)
        {
            Books = books;
        }

        public IEnumerable<BookEntity>? Books { get; set; }
    }
}
