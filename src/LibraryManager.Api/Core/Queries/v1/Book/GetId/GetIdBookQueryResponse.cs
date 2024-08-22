using Core.Enums.v1;

namespace Core.Queries.v1.Book.GetId
{
    public class GetIdBookQueryResponse
    {
        public GetIdBookQueryResponse(string? title, string? author, string? iSBN, int yearPublication, BookStatus? status)
        {
            Title = title;
            Author = author;
            ISBN = iSBN;
            YearPublication = yearPublication;
            Status = status;
        }

        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public int YearPublication { get; set; }
        public BookStatus? Status { get; set; }
    }
}
