using ScreenNavigation;
using UnityEngine;

namespace Utils
{
    public static class StringFormatUtils
    {
        public static string ToLabel(this SceneID value)
        {
            return value.ToString().Replace("_", " ");
        }

        public static string ToPercentage(this float value, int max = 0)
        {
            int percentage = (int)(value * 100);
            if (max > 0 && percentage > max)
            {
                percentage = max;
            }

            return $"{percentage}%";
        }

        public static string ToTimeElapsed(this float value)
        {
            int hours = Mathf.FloorToInt(value / 3600);
            int minutes = Mathf.FloorToInt(value % 3600 / 60);
            int remainingSeconds = Mathf.FloorToInt(value % 60);

            return hours == 0 ? $"{minutes:00}:{remainingSeconds:00}" :
                                $"{hours:00}:{minutes:00}:{remainingSeconds:00}";
        }
    }
}