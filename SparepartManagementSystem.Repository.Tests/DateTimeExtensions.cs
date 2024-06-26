namespace SparepartManagementSystem.Repository.Tests;

public static class DateTimeExtensions
{
    internal static DateTime TrimMiliseconds(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
    }
}