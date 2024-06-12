namespace PhoneBookConsoleApp;

/// <summary>
///     class <c>ContactsHandlerForRelationalDb</c>
/// </summary>
public class ContactsHandlerForRelationalDb : IContactsHandler<Contact>
{
    private readonly ContactRepositoryForSqLite _contactRepositoryForSqLite;
    private int numberOfElements = 0;

    /// <summary>
    ///     Constructor takes connectionString as an argument which creates ContactRepositoryForSqLite object   
    /// </summary>
    /// <param name="connectionString"></param>
    public ContactsHandlerForRelationalDb()
    {
        _contactRepositoryForSqLite = new ContactRepositoryForSqLite();
    }

    public int NumberOfElements
    {
        get { return numberOfElements; }
    }
    
    
    /// <summary>
    ///     Add(Contact obj) - Asynchronous method that manages contact object insertion into the database
    /// </summary>
    /// <param name="obj"></param>
    public async void Add(Contact obj)
    {
        numberOfElements++;
        await _contactRepositoryForSqLite.InsertAsync(obj);
    }

    
    
    /// <summary>
    ///     Remove(Contact obj) - Asynchronous method that manages removing of the contact object from the database
    /// </summary>
    /// <param name="obj"></param>
    public async void Remove(Contact obj)
    {
        numberOfElements--;
        await _contactRepositoryForSqLite.DeleteAsync(obj.Name);
    }

    
    
    /// <summary>
    ///     List() - Asynchronous method that returns a list of contact objects from database
    /// </summary>
    /// <returns></returns>
    public List<Contact> List()
    {
        IEnumerable<Contact> contactsEnumerable = _contactRepositoryForSqLite.GetAllAsync().Result;
        return contactsEnumerable.ToList();
    }

    
    
    /// <summary>
    ///     Display() - Asynchronous method that manages displaying all the data into the console
    /// </summary>
    public void Display()
    {
        foreach (var contact in List())
        {
            Console.WriteLine(contact.ToString());
        }
    }

    
    
    
    /// <summary>
    ///     Clear() - Asynchronous method that clears all the data in database
    /// </summary>
    public async void Clear()
    {
        await _contactRepositoryForSqLite.DeleteAllAsync();
    }
    
    /// <summary>
    ///     Update(Contact contact) - Asynchronous method that updates contact data in the database
    /// </summary>
    /// <param name="contact"></param>
    public async void Update(Contact contact)
    {
        await _contactRepositoryForSqLite.UpdateAsync(contact);
    }
}