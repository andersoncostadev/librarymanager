namespace Core.Commands.v1.Book.Update
{
    public class UpdateBookCommandResponse
    {
        public UpdateBookCommandResponse(string? title, string? author, string? iSBN, int yearPublication)
        {
            Title = title;
            Author = author;
            ISBN = iSBN;
            YearPublication = yearPublication;
        }

        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public int YearPublication { get; set; }
    }
}
