using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Helpers
{
    public class DateTimeObject
    {
        public static DateTime ToDateTime(double timeInSeconds)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(timeInSeconds);
        }
    }
}
