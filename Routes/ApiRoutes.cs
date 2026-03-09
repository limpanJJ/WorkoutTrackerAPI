namespace WorkoutTrackerAPI.Routes
{
	public static class ApiRoutes
	{
		public const string ApiBase = "api";

		public static class Login
		{
			public const string Base = ApiBase + "/auth/login";
		}

		public static class Register
		{
			public const string Base = ApiBase + "/auth/register";
		}

		public static class ExerciseCategories
		{
			public const string Base = ApiBase + "/exercise-categories";
		}

		public static class MuscleGroups
		{
			public const string Base = ApiBase + "/muscle-groups";
		}

		public static class Exercises
		{
			public const string Base = ApiBase + "/exercises";
		}

		public static class WorkoutSessions
		{
			public const string Base = ApiBase + "/workout-sessions";
		}
	}
}
