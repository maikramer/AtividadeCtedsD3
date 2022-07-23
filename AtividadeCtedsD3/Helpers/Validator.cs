using System.Net.Mail;

namespace AtividadeCtedsD3.Helpers;

public static class Validator
{
    
    public static bool ValidateEmail(string? emailAddress)
    {
        if (emailAddress == null) return false;
        try
        {
            var unused = new MailAddress(emailAddress);

            return true;
        } catch (FormatException)
        {
            return false;
        }
    }
}