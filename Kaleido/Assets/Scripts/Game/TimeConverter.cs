using System;

public class TimeConverter
{
    public static string ConvertSecondsToTime(float seconds)
    {
        // Calculate hours, minutes, and seconds
        int totalSeconds = (int)seconds;
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int remainingSeconds = totalSeconds % 60;

        // Format the result
        if (hours > 0)
        {
            return $"{hours:D2}:{minutes:D2}:{remainingSeconds:D2}";
        }
        else
        {
            return $"{minutes:D2}:{remainingSeconds:D2}";
        }
    }
}
