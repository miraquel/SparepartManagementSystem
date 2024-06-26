using MySqlConnector;
using Dapper;

namespace SparepartManagementSystem.Console;

public static class MySqlConnectionTest
{
    public static async void TestMultiThreadedMySqlConnection()
    {
        const string connectionString = "Server=localhost;Database=sparepart_management_system;Uid=root;Pwd=root;AllowLoadLocalInfile=true;Allow User Variables=true;";
        await using var firstConnection = new MySqlConnection(connectionString);
        await using var secondConnection = new MySqlConnection(connectionString);
        await using var thirdConnection = new MySqlConnection(connectionString);
        
        firstConnection.Open();
        secondConnection.Open();
        thirdConnection.Open();
        
        var tasks = new List<Task<int>>
        {
            firstConnection.ExecuteAsync("SELECT 1"),
            secondConnection.ExecuteAsync("SELECT 1"),
            thirdConnection.ExecuteAsync("SELECT 1")
        };
        
        var result = await Task.WhenAll(tasks);
        
        System.Console.WriteLine(result[0]);
        System.Console.WriteLine(result[1]);
        System.Console.WriteLine(result[2]);
    }
}