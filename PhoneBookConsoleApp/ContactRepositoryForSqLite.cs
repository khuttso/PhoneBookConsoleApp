using System.Data.SqlClient;
using Exception = System.Exception;

namespace PhoneBookConsoleApp;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SQLite;
using NLog;
using Microsoft.Extensions.Logging;

/// <summary>
/// 
/// </summary>
public class ContactRepositoryForSqLite : IContactRepositoryForDb
{
    private readonly IDbConnection _connection;
    private ILogger<ContactRepositoryForSqLite> _logger;

    public ContactRepositoryForSqLite(ILogger<ContactRepositoryForSqLite> logger)
    {
        _logger = logger;
        string databasePath = "DatabaseForContacts";
        // Create the database file if it doesn't exist
        if (!System.IO.File.Exists(databasePath))
        {
            SQLiteConnection.CreateFile(databasePath);
            Console.WriteLine("Database file created.");
        }
        var connectionStringBuilder = new SQLiteConnectionStringBuilder
        {
            DataSource = databasePath
        };
        string connectionString = connectionStringBuilder.ToString();
        

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            _connection = new SQLiteConnection(connectionString);
            _connection.Open();
            connection.Open();
            string s = connection.ConnectionString;
           
            
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Contact (
                    Name string PRIMARY KEY,
                    PhoneNumber string
                );";

            using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Table created or already exists.");
            }
            

            _connection = new SQLiteConnection(connectionString);
            _logger.LogInformation("Connection is established");
        }
    }

    public IDbConnection Connection { get { return _connection; } }        
    
    public async Task InsertAsync(Contact contact)
    {
        using (var c = new SQLiteConnection(_connection.ConnectionString))
        {
            await c.OpenAsync();
            string sql = "INSERT INTO Contact (Name, PhoneNumber) VALUES (@Name, @PhoneNumber)";
            using (var command = new SQLiteCommand(sql, c))
            {
                command.Parameters.AddWithValue("@Name", contact.Name);
                command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);

                await command.ExecuteNonQueryAsync();
            }
            _logger.LogInformation($"Contact with name:{contact.Name} and phone number:{contact.PhoneNumber} is added into the PhoneBook");
        }
    }
    
    

    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        _logger.LogInformation("Getting all data from database");
        using (var connection = new SQLiteConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();
            string sql = "SELECT * FROM Contact";
            return await connection.QueryAsync<Contact>(sql);
        }
    }
    
    
    public async Task<Contact> GetByNameAsync(string name)
    {
        Contact cont = null;
        using (var c = new SQLiteConnection(_connection.ConnectionString))
        {
            await c.OpenAsync();
            string sql = "SELECT * FROM Contact WHERE Name = @Name";

            using (var command = new SQLiteCommand(sql, c))
            {
                command.Parameters.AddWithValue("@Name", name);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine(reader.GetString(reader.GetOrdinal("Name")));
                        cont = new Contact(reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetString(reader.GetOrdinal("PhoneNumber")));
                    }
                }
            }
        }
        _logger.LogInformation($"Getting contact with name:{name}");

        
        return cont;
    }

    
    
    public async Task UpdateAsync(Contact contact)
    {
        using (var c = new SQLiteConnection(_connection.ConnectionString))
        {
            await c.OpenAsync();
            string sql = "UPDATE Contact SET PhoneNumber = @PhoneNumber Where Name = @Name";

            using (var command = new SQLiteCommand(sql, c))
            {
                command.Parameters.AddWithValue("@Name", contact.Name);
                command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);

                await command.ExecuteNonQueryAsync();
            }
        }
        _logger.LogInformation($"Updating contact with name:{contact.Name} and setting new phone number:{contact.PhoneNumber}");
    }

    
    
    public async Task DeleteAsync(string name)
    {
        
        using (var c = new SQLiteConnection(_connection.ConnectionString))
        {
            await c.OpenAsync();
            string sql = "DELETE FROM Contacts Where Name = @Name";

            using (var command = new SQLiteCommand(sql, c))
            {
                command.Parameters.AddWithValue("@Name", name);
                
                await command.ExecuteNonQueryAsync();
            }
        }
        _logger.LogInformation($"Removing data with name:{name}");
    }

    public async Task DeleteAllAsync()
    {
        using (var c = new SQLiteConnection(_connection.ConnectionString))
        {
            await c.OpenAsync();
            string sql = "DELETE From Contact";

            using (var command = new SQLiteCommand(sql, c))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
        _logger.LogInformation("Removing all data from database");
    }
}