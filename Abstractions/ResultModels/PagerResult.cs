namespace Timeoff.ResultModels
{
    public record PagerResult
    {
        public int TotalRows { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int PageSize { get; init; } = 10;

        public int TotalPages => (int)Math.Ceiling(((decimal)TotalRows) / PageSize);

        public int PreviousPage => CurrentPage > 1 ? CurrentPage - 1 : 0;

        public int NextPage => CurrentPage < TotalPages ? CurrentPage + 1 : 0;

        public object QueryParameters { get; init; }
    }
}