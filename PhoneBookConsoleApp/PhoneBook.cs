namespace PhoneBookConsoleApp;
using Newtonsoft.Json;

/// <summary>
///     class <c>Phonebook</c>> - class for console application that functions as a phone book,
///     allowing users to add, remove, update, search and list contacts.
///     The application stores contact information in a file to maintain persistence across sessions 
/// </summary>
/// 
public class PhoneBook
{
    private Dictionary<string, Contact> _contacts;  
    private IContactsHandler<Contact> _handler;
    private IValidation _validationHandler;
    
    
    
    /// <summary>
    ///     This class is written for Json file format and Georgian phone numbers,
    ///     Constructor initializes suitable ContactsHandlerForJson and GeorgianNumberValidation
    ///     to the attributes _handlerForFile and _validationHandler respectively     
    /// </summary>
    public PhoneBook(IContactsHandler<Contact> contactsHandler, IValidation validation) 
    {
        _contacts = new Dictionary<string, Contact>();
        _handler = contactsHandler;
        _validationHandler = validation;
    }

    
    
    
    /// <summary>
    ///     AddContact(name, phoneNumber) simply maps name and contact.
    ///     Stores into the dictionary and some file with path - 'path' 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="phoneNumber"></param>
    /// <exception cref="ArgumentException"></exception>
    /// thrown when name does not appear in contacts or phone number is not valid
    public bool AddContact(string name, string phoneNumber)
    {
        if (_contacts.ContainsKey(name) || !_validationHandler.Validate(phoneNumber))
        {
            Console.WriteLine("Invalid name or phone number");
            return false;
        }
        
        _contacts.Add(name, new Contact(name, phoneNumber));
        _handler.Add(new Contact(name, phoneNumber));
        return true;
    }

    
    
    
    /// <summary>
    ///     RemoveContact(name) - Removes Contact with given name from dictionary and from the file as well
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="ArgumentException"></exception>
    /// thrown when name does not appear in contacts
    public bool RemoveContact(string name)
    {
        if (!_contacts.ContainsKey(name))
        {
            Console.WriteLine("Invalid name");
            return false;
        }

        _handler.Remove(_contacts[name]);
        _contacts.Remove(name);
        return true;
    }
    
    
    
    
    /// <summary>
    ///     UpdateContact(name, newPhoneNumber) changes old phone number with new one
    ///     For updating everything in the file at the same time, Remove and Add methods are used here
    /// </summary>
    /// <param name="name"></param>
    /// <param name="newPhoneNumber"></param>
    public bool Update(string name, string newPhoneNumber)
    {
        return RemoveContact(name) && AddContact(name, newPhoneNumber);
    }
    
    
    
    /// <summary>
    ///     Search(name) - searches and returns Contact with given name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Contact Search(string name)
    {
        return _contacts.GetValueOrDefault(name, null);
    }
    
    
    
    
    /// <summary>
    ///     ListContacts() return contacts as a list
    /// </summary>
    /// <returns></returns>
    public List<Contact> ListContacts()
    {
        return _handler.List();
    }

    
    
    
    /// <summary>
    ///     LoadContacts() displays all the contacts in the console
    /// </summary>
    public void DisplayContacts()
    {
        _handler.Display();
    }
}