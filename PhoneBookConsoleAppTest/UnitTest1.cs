namespace PhoneBookConsoleAppTest;
using PhoneBookConsoleApp;
using Dapper;
using System.Data.SQLite;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Config;
using NLog.Targets;

using Xunit;

public class UnitTest1
{
    private const string _connectionString = "Data Source=./Data/identifier.sqlite";

    private static ContactsHandlerForRelationalDb _contactsHandlerForSQLite;

    private PhoneBook _phoneBook = new PhoneBook(_contactsHandlerForSQLite, new GeorgianNumberValidation());

    public UnitTest1()
    {
        var config = new LoggingConfiguration();

        var logfile = new FileTarget("logfile")
        {
            FileName = "logfile.txt",
            Layout = "${longdate} ${level:uppercase=true} ${message} ${exception:format=toString,StackTrace}"
        };
        var logconsole = new ConsoleTarget("logconsole");


        LogManager.Configuration = config;

        // Create a LoggerFactory and configure it to use NLog
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddNLog(config);
        });

        // Create a logger for the HandlerForRationalDatabase class
        var logger = loggerFactory.CreateLogger<ContactRepositoryForSqLite>();

        _contactsHandlerForSQLite = new ContactsHandlerForRelationalDb(logger);
    }


    /// <summary>
    ///     TestAddContact() - Todo="Tomorrow";
    /// </summary>
    [Fact]
    public async Task TestAddContact()
    {
        string name = "Luka";
        string phoneNumber = "+995599310860";
        string phoneNumberNew = "+995599310860";
        _phoneBook.AddContact(name, phoneNumber);

        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT * PhoneNumber FROM Contact WHERE Name = @Name";

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("Name", name);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        phoneNumberNew = reader.GetString(reader.GetOrdinal("Name"));
                    }
                }
            }
        }

        Assert.Equal(phoneNumber, phoneNumberNew);
    }
    
    
    
    
    /// <summary>
    ///     TestDeleteContact() - Todo - "Tomorrow"
    /// </summary>
    [Fact]
    public async Task TestDeleteContact()
    {
        string name = "Luka";
        string phoneNumber = "+995599310860";
        _phoneBook.AddContact(name, phoneNumber);
        
        int count = 0;
        _phoneBook.RemoveContact(name);

        using (var connection = new SQLiteConnection(_connectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT COUNT(*) PhoneNumber FROM Contact WHERE Name = @Name";

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("Name", name);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        count = int.Parse(reader.GetString(reader.GetOrdinal("Name")));
                    }
                }
            }
        }
        Assert.Equal(0, count); 
    }
    
    
    
    
    
    /// <summary>
    ///     TestUpdateContact() - Todo - "Tomorrow"
    /// </summary>
    [Fact]
    public async Task TestUpdateContact()
    {   
        string name = "Luka";
        string phoneNumber = "+995599310860";
        string newPhoneNumber = "+995511223344";
        string currentDbData = "";
        _phoneBook.AddContact(name, phoneNumber);
        _phoneBook.Update(name, newPhoneNumber);

        using (var connection = new SQLiteConnection())
        {
            await connection.OpenAsync();
            string sql = "SELECT PhoneNumber PhoneNumber FROM Contact WHERE Name = @Name";

            using (var command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("Name", name);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        currentDbData = reader.GetString(reader.GetOrdinal("Name"));
                    }
                }
            }
        }
        Assert.Equal(currentDbData, newPhoneNumber);
    }
}