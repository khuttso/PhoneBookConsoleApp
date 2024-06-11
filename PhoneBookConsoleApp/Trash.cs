namespace PhoneBookConsoleApp;
using Dapper;
using System.Data.SQLite;

public class Trash
{
    private const string _connectionString = "Data Source=C:\\Users\\asusVivo\\DataGripProjects\\PhoneBookApp\\identifier.sqlite";

    private static ContactsHandlerForRelationalDb _contactsHandlerForSQLite =
        new ContactsHandlerForRelationalDb(_connectionString);
    
    private PhoneBook _phoneBook = new PhoneBook(_contactsHandlerForSQLite, new GeorgianNumberValidation());

    // / / / / / // 
    public void AssertString(string s, string s1)
    { 
        if (s != s1) Console.WriteLine("Failed");
        else Console.WriteLine("Passed");
    }
    public void AssertInt(int x, int y)
    {
        if (x != y) Console.WriteLine("Failed");
        else Console.WriteLine("Passed");
    }
    // / / // / // // 
    
    
    /// <summary>
    ///     TestAddContact() - Todo="Tomorrow";
    /// </summary>
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
        AssertString(phoneNumberNew, phoneNumber);
    }
    
    
    
    
    /// <summary>
    ///     TestDeleteContact() - Todo - "Tomorrow"
    /// </summary>
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
        AssertInt(count, 0); 
    }
    
    
    
    
    
    /// <summary>
    ///     TestUpdateContact() - Todo - "Tomorrow"
    /// </summary>
    public async Task TestUpdateContact()
    {   
        string name = "Luka";
        string phoneNumber = "+995599310860";
        string newPhoneNumber = "+995511223344";
        string currentDbData = "";
        _phoneBook.AddContact(name, phoneNumber);
        _phoneBook.Update(name, newPhoneNumber);

        using (var connection = new SQLiteConnection(_connectionString))
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
        AssertString(currentDbData, newPhoneNumber);
    }
}