namespace WorkoutTrackerAPI.Dtos.Common
{
    public class PagedResponse<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage => Page < TotalPages;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
