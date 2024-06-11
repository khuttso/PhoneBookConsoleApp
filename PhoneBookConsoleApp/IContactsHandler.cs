namespace PhoneBookConsoleApp;

/// <summary>
///     Interface for handling file operations for objects of type Contact.
///     subclasses will be implemented depended on type of the file
/// 
/// </summary>
/// <typeparam name="T"></typeparam>


public interface IContactsHandler<T> where T : Contact
{
    /// <summary>
    /// void AddToFile(T obj) adds given object to the file
    /// void RemoveFromFile(T obj) removes given object from the file void
    /// AddAllToFile(IEnumerable<T> collection) Default method that adds all the data from the collection to the file
    /// void Display() Displays all data into the console 
    /// void Clear() removes everything from the file
    /// </summary>
    
    void Add(T obj);          
    void Remove(T obj);

    List<T> List();
    void Display();
    void Clear();

    void AddAll(IEnumerable<T> collection)
    {
        foreach (var contact in collection)
        {
            Add(contact);    
        }
    }
    
}