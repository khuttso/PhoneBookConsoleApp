using System.Data.SqlClient;
using Exception = System.Exception;

namespace PhoneBookConsoleApp;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SQLite;

/// <summary>
/// 
/// </summary>
public class ContactRepositoryForSqLite : IContactRepositoryForDb
{
    private readonly IDbConnection _connection;

    public ContactRepositoryForSqLite(string connectionString)
    {
        _connection = new SQLiteConnection(connectionString);
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
        }
    }

    
    
    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        List<Contact> contactsList = new List<Contact>();
        using (var c = new SQLiteConnection(_connection.ConnectionString))
        {
            await c.OpenAsync();
            string sql = "SELECT * FROM Contact";

            using (var command = new SQLiteCommand(sql, c))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Contact cont = new Contact(reader.GetString(reader.GetOrdinal("Name")),
                        reader.GetString(reader.GetOrdinal("PhoneNumber")));
                    
                    contactsList.Add(cont);
                }
            }
        }

        return contactsList;
    }

    
    
    public async Task<Contact> GetByNameAsync(string name)
    {
        Contact cont = null;
        using (var c = new SQLiteConnection(_connection.ConnectionString))
        {
            await c.OpenAsync();
            string sql = "SELECT * FROM Contact WHERE Name = @nName";

            using (var command = new SQLiteCommand(sql, c))
            {
                command.Parameters.AddWithValue("@Name", name);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cont = new Contact(reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetString(reader.GetOrdinal("PhoneNumber")));
                    }
                }
            }
        }

        
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
    }
}