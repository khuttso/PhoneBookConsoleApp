# Phone Book Application with File/Database Storage

## Table of Contents
- [Description](#Description)
- [Features](#Features)
- [Classes and Methods](#classes-and-methods)
- [Instructions For Users](#instructions-for-users)

## Description
The Phone Book Application is a console app designed to manage contacts efficiently. It allows users to add, remove, update, list, load and save contacts. The application stores all the data a the file. Users interact with the program via the console

## Features
- **Add Contact:** Allows users to add a new contact with a name and phone number to the phone book.
- **Remove Contact:** Allows users to removes given existing contact from the phone book
- **Update Contact:** Allows users to update contact.
- **Display Contacts:** Allows users to load and see contacts in the console.
- **Persistent Storage:** Contacts are saved in a JSON file or in a database.

## Instructions For Users:
It's necessary to run new `ConsoleAppForJsonFile()` or `new ConsoleAppForRelationalDb()` in the `Main(String[] args)` method for running phone book application.
- Every operation that user may need to do will be described in the console after running the `Main()` method
- User will need to type some inputs into the console, when adding, removing or updating contacts
- User will be able to see the format of the input and every invalid trying will cause the repetition of the process.
- After doing any operation user will be able to close the application by typing the command provided by the program.


## Classes and methods
### Contact class
Represents Data class for Phonebook

### Attributes
- `name` - The name of the contact
- `phoneNumber` - The phone number of the contact

### Methods
`bool Equals()`, `int GetHashCode()`, `string ToString()` are override

### IContactsHandler interface
    public interface IContactsHandler<T> where T : Contact
Handler class for contact objects. Subclasses can be implemented for anything that can store data.
For example: Text file, Json File, Database, ...

### Methods:
- `void Add(T obj)` - Adds contact object to the storage
- `void Remove(T obj)` - Removes contact object from the storage
- `List<T> List()` - Returns storage data as a List<T> object
- `void Display()` - Displays all the contacts into the console
- `void Clear();` - Removes everything from the storage

### ContactsHandlerForJsonFile class
```csharp
    public class ContactsHandlerForJson : IContactsHandler<Contact>
```    
Handler class for contact objects for Json files. Here is implemented every method of the interface
IContactsHandler. Every method has its own comment about how it works.
For storing into the Json file, here is used Serialization method.
`JsonConvert.SerializeObject(...)` method of the package `Newtonsoft.Json;`

### Methods:
- `void Add(T obj)` - Adds contact object to the storage
- `void Remove(T obj)` - Removes contact object from the storage
- `List<Contact> List()` - Returns storage data as a List<Contact> object
- `void Display()` - Displays all the contacts into the console
- `void Clear();` - Removes everything from the storage

These methods using Json serialization and deserialization algorithm mentioned above.
Methods working details and possible thrown exceptions are attached to the methods as a comments.

### ContactsHandlerForRelationalDb class
////
////
////
////

////
////




### IValidation interface
```csharp
public interface IValidation
{
    bool Validate(string number);
}
```
    This interface has single bool method thats receives one string argument and checks if is it valid or not.
    Subclasses should be implemented depended on some rules about phone numbers validations.


### GeorgianNumberValidation class
```csharp
public class GeorgianNumberValidation : IValidation {
    ...
}

```
This class is implemented for checking if phone number is valid.
For Georgia - number must start with '+9955' and 8 digit

