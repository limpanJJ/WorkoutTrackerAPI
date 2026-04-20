namespace WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests
{
    public class WorkoutSessionQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        // Filter by name (contains, case-insensitive)
        public string? Name { get; set; }

        // Time period – filters on StartedAt
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Sorting: "date" | "name" | "duration"
        public string SortBy { get; set; } = "date";

        // Direction: "desc" | "asc"
        public string SortOrder { get; set; } = "desc";
    }
}
