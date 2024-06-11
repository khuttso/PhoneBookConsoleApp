using System.Collections;

namespace PhoneBookConsoleApp;
using Newtonsoft.Json;

/// <summary>
///     class <c>ContactsHandlerForJson</c>> - class based on IContactsHandledForFile.
///     interface methods are implemented for Json file.
///
///     JsonConvert.SerializeObject(...); and JsonConvert.DeserializeObject(...); methods of the package
///     Newtonsoft.Json are used to do standard serialization of the data
///
///     Class is written for Json file and all the cases(other file type) that may cause error are handled
/// </summary>

public class ContactsHandlerForJson : IContactsHandler<Contact>
{
    private readonly string path;
    private int numberOfElemets = 0;
    
    /// <summary>
    ///     Constructor receives string path, which is supposed to be Json file location
    ///     Here is simply checked if the path is incompatible for format Json      
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="ArgumentException"></exception>
    ///    thrown when given path is incompatible with format json
    /// <exception cref="FileNotFoundException"></exception>
    ///    thrown when the file with given path does not exist 
    public ContactsHandlerForJson(string path)
    {
        if (path.Substring(path.Length-5) != ".json")
        {
            throw new ArgumentException("Incompatible File Path");
        }
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("File does not exist");
        }

        this.path = path;
    }
    
    public string Path
    {
        get { return this.path; }
    }
    public int NumberOfElements
    {
        get { return numberOfElemets; }
    }


    /// <summary>
    ///     By calling AddToFile(obj) - Based on task description Serialized data of obj is added into the file
    /// </summary>
    /// <param name="obj"></param>
    public void Add(Contact obj)
    {
        numberOfElemets++;
        string readJsonData = File.ReadAllText(path);
        List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(readJsonData);
        if (contacts == null) contacts = new List<Contact>();
        
        contacts.Add(obj);
        string serializedContacts = JsonConvert.SerializeObject(contacts);
        Clear();
        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine(serializedContacts);                        
        }
    }


    
    
    /// <summary>
    ///     RemoveFromFile(obj) removes object from the file by using following steps:
    ///         1. Reads data from the file
    ///         2. Deserializes read data and stores in collection
    ///         3. Removes data from the collection
    ///         4. Writes updated data into the file
    /// </summary>
    /// <param name="obj"></param>
    public void Remove(Contact obj)
    {
        if (numberOfElemets == 0) 
        numberOfElemets--;
        // making list of contact objects from Json file
        string jsonData = File.ReadAllText(path);
        List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonData);
        contacts.RemoveAll(c => c.Equals(obj));
        
        this.Clear();

        using (StreamWriter sw = new StreamWriter(path, true)) 
        {
            sw.WriteLine(JsonConvert.SerializeObject(contacts));
        }
    }

    
    
    /// <summary>
    ///        List() - deserializes json data and returns it as list 
    /// </summary>
    /// <returns></returns>
    public List<Contact> List()
    {
        string jsonData = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<List<Contact>>(jsonData);
    }

    
    
    /// <summary>
    ///     Display() - iterates over deserialized data and print
    /// </summary>
    public void Display()
    {
        foreach (Contact c in List()) 
        {
            Console.WriteLine(c.ToString());
        }
    }

        
    /// <summary>
    ///     Clear() - simply clears the file
    /// </summary>
    public void Clear()
    {
        File.WriteAllText(path, "");
    }
    
}