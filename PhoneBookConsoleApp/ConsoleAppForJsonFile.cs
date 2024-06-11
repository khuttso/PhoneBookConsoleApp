using System.Threading.Channels;

namespace PhoneBookConsoleApp;

/// <summary>
///     class <c>ConsoleApp</c> - class for communication between user and program.
///     contains single phonebook object as an attribute and the method Run() runs the console application 
/// </summary>
public class ConsoleAppForJsonFile : IConsoleApp
{
    private PhoneBook _phoneBook;
    private const string path = "contacts.json";
    private ContactsHandlerForJson _handlerForJson = new ContactsHandlerForJson(path);
    
    public ConsoleAppForJsonFile()
    {
        _phoneBook = new PhoneBook(_handlerForJson, new GeorgianNumberValidation());
    }

    public void Welcome()
    {
        Console.WriteLine("Hello!");
        Console.WriteLine("This is The Phone Book Application is a console app designed to manage contacts efficiently. \nIt allows users to add, remove, update, list, load and save contacts. The application stores all the data a the file. \nUsers interact with the program via the console");
        Console.WriteLine("_____________________________________________________________________________________________________");
    }

    
    
    
    /// <summary>
    ///     Implementation() - performs conversation between user and program. Conversation includes only one operation.
    ///             add, remove, update, search or load.
    ///     For working PhoneBook correctly, this implementation should be repeated until user wants.
    /// </summary>
    public void Implementation()
    {
        Console.WriteLine("You can perform the following operations: Add Contact, Remove Contact, Update contact, Find phone number by name, Load phone book data");
        Console.WriteLine("Choose one of the following operations(1-5):");
        Console.WriteLine("1. Add contact");
        Console.WriteLine("2. Remove contact");
        Console.WriteLine("3. Update contact");
        Console.WriteLine("4. Search contact");
        Console.WriteLine("5. Display contacts");
        
        Console.WriteLine("Type number from 1 to 5");
        int input = int.Parse(Console.ReadLine());
        while (input < 1 || input > 5)
        {
            Console.WriteLine("Invalid operation. Type number in range [1,5]");
            input = Convert.ToInt32(Console.Read());
        }

        if (input == 1)
        {
            Console.WriteLine("Please enter a contact name:");
            string name = Console.ReadLine();
            Console.WriteLine("Please enter a contact phone number in valid format (+9955 and 8 digits), otherwise program will close:");
            string phoneNumber = Console.ReadLine();

            bool test = _phoneBook.AddContact(name, phoneNumber);
            while (!test)
            {
                Console.WriteLine("Please enter a contact name:");
                name = Console.ReadLine();
                Console.WriteLine("Please enter a contact phone number in valid format (+9955 and 8 digits), otherwise program will close:");
                phoneNumber = Console.ReadLine();
                test = _phoneBook.AddContact(name, phoneNumber);
            }
            Console.WriteLine("contact is added successfully");
        }
        else if (input == 2)
        {
            if (_handlerForJson.NumberOfElements == 0)
            {
                Console.WriteLine("Invalid Operation. You can not remove when contacts is empty");
                return;
            }
            Console.WriteLine("Please enter a contact name to remove:");
            string name = Console.ReadLine();

            bool test = _phoneBook.RemoveContact(name);
            while (!test)
            {
                Console.WriteLine("Please enter a contact name to remove:");
                name = Console.ReadLine();
                test = _phoneBook.RemoveContact(name);
            }
            Console.WriteLine("Contact is removed successfully");
        }
        else if (input == 3)
        {
            Console.WriteLine("Please enter a contact name to update:");
            string name = Console.ReadLine();
            Console.WriteLine("Please enter a contact phone number in valid format (+9955 and 8 digits), otherwise program will close:");
            string phoneNumber = Console.ReadLine();
            
            bool test = _phoneBook.Update(name, phoneNumber);
            while (!test)
            {
                Console.WriteLine("Please enter a contact name to update:");
                name = Console.ReadLine();
                Console.WriteLine("Please enter a contact phone number in valid format (+9955 and 8 digits), otherwise program will close:");
                phoneNumber = Console.ReadLine();
                test = _phoneBook.Update(name, phoneNumber);
            }
            Console.WriteLine("Phone number is updated successfully");
        }
        else if (input == 4)
        {
            Console.WriteLine("Please enter a name: ");
            string name = Console.ReadLine();

            Contact contact = _phoneBook.Search(name);
            while (contact == null)
            {
                Console.WriteLine("Name not found. Try again");
                Console.WriteLine("Please enter a name: ");
                name = Console.ReadLine();
            }
            Console.WriteLine($"The contact with a name {name} is found successfully");
            Console.WriteLine(contact.ToString());
        }
        else
        {
            if (_handlerForJson.NumberOfElements == 0)
            {
                Console.WriteLine("The phone book is empty");
                return;
            }
            Console.WriteLine("Current Contacts in phone book: ");
            _phoneBook.DisplayContacts();
        }
    }

    
    
    
    /// <summary>
    ///     Run() - Figures out if user want to continue doing operations in phone book and does implementation repeatedly.
    /// </summary>
    public void Run()
    {
        _handlerForJson.Clear();
        Welcome();
        while (true)
        {
            Console.WriteLine("If u want to close app, type: '-', otherwise type anything");
            string input = Console.ReadLine();
            if (input == "-")
            {
                break;
            }
            
            Implementation();
        }
    }
}