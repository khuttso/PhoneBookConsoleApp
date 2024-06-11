using System.Data;

namespace PhoneBookConsoleApp;

public interface IContactRepositoryForDb
{ 
    Task InsertAsync(Contact contact);
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<Contact> GetByNameAsync(string name);
    Task UpdateAsync(Contact contact);
    Task DeleteAsync(string name);
    Task DeleteAllAsync();
}