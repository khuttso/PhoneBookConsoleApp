namespace PhoneBookConsoleApp;

/// <summary>
///      class <c>GeorgianNumberValidation</c>> - This class is implemented for checking if phone number is valid.
///      For Georgia - number must start with '+9955' and 8 digit
/// </summary>
public class GeorgianNumberValidation : IValidation
{
    public bool Validate(string number)
    {
        return number.Length == 13 && number.Substring(0, 5) == "+9955";
    }
}