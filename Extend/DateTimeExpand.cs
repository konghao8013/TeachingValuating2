using System;


public static class DateTimeExpand
{
    /// <summary>
    /// 将DayOfWeek转化为星期几
    /// </summary>
    /// <param name="week"></param>
    /// <returns></returns>
    public static string ToWeek(this DayOfWeek week)
    {
        string weekString = "未判断到星期";
        switch (week)
        {
            case DayOfWeek.Friday: weekString = "星期五"; break;
            case DayOfWeek.Monday: weekString = "星期一"; break;
            case DayOfWeek.Saturday: weekString = "星期六"; break;
            case DayOfWeek.Sunday: weekString = "星期日"; break;
            case DayOfWeek.Thursday: weekString = "星期四"; break;
            case DayOfWeek.Tuesday: weekString = "星期二"; break;
            case DayOfWeek.Wednesday: weekString = "星期三"; break;
        }
        return weekString;
    }
    /// <summary>
    /// 获得一个日期 当前周的开始时间
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime WeekStartDate(this DateTime date)
    {
            var week = date.DayOfWeek;
            var timeDate = date.AddDays(-(int)week);
        timeDate = timeDate.AddHours(-timeDate.Hour);
        timeDate = timeDate.AddMinutes(-timeDate.Minute);
        return timeDate;
    }

    //// 摘要:
    ////     表示星期日。
    //Sunday = 0,
    ////
    //// 摘要:
    ////     表示星期一。
    //Monday = 1,
    ////
    //// 摘要:
    ////     表示星期二。
    //Tuesday = 2,
    ////
    //// 摘要:
    ////     表示星期三。
    //Wednesday = 3,
    ////
    //// 摘要:
    ////     表示星期四。
    //Thursday = 4,
    ////
    //// 摘要:
    ////     表示星期五。
    //Friday = 5,
    ////
    //// 摘要:
    ////     表示星期六。
    //Saturday = 6,
}
