public static class TimeFormatter
{
    private const int MINUTE = 60;
    private const int HOUR = MINUTE * 60;
    private const int DAY = HOUR * 24;
    
    public static string GetTimeString(int totalSeconds)
    {
        if (totalSeconds >= DAY)
        {
            int days = totalSeconds / DAY;
            int hours = (totalSeconds - days * DAY) / HOUR;
            return hours > 0 ? $"{days}d {hours}h" : $"{days}d";
        }

        if (totalSeconds >= HOUR)
        {
            int hours = totalSeconds / HOUR;
            int minutes = (totalSeconds - hours * HOUR) / MINUTE;
            return minutes > 0 ? $"{hours}h {minutes}m" : $"{hours}h";
        }

        if (totalSeconds >= MINUTE)
        {
            int minutes = totalSeconds / MINUTE;
            int seconds = totalSeconds - minutes * MINUTE;
            return seconds > 0 ? $"{minutes}m {seconds}s" : $"{minutes}m";
        }

        return $"{totalSeconds}s";
    }
}
