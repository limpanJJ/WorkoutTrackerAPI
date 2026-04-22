namespace WorkoutTrackerAPI.Dtos.Common
{
    public class WorkoutStatsResponse
    {
        public int TotalSessions { get; set; }
        public int TotalExercises { get; set; }
        public int TotalSets { get; set; }
        public int TotalReps { get; set; }
        public decimal TotalWeight { get; set; }
    }

}
