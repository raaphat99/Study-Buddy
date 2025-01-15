namespace Presentation.Utilities
{
    public class TimeSinceFormatter
    {
        public static string GetTimeSince(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{timeSpan.Minutes} mins ago";
            if (timeSpan.TotalHours < 24)
                return $"{timeSpan.Hours} hours ago";
            if (timeSpan.TotalDays < 30)
                return $"{timeSpan.Days} days ago";
            if (timeSpan.TotalDays < 365)
                return $"{timeSpan.Days / 30} months ago";

            return $"{timeSpan.Days / 365} years ago";

        }
    }
}
