// See https://aka.ms/new-console-template for more information

using System;

namespace PhoneBookConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        // new ConsoleAppForJsonFile().Run();
        // contactDB
        
        new ConsoleAppForRationalDb().Run();
    }
}