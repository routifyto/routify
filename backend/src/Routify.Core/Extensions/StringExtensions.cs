using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Routify.Core.Extensions;

public static class StringExtensions
{
    public static string ToSha256(
        this string value)
    {
        // Convert the input string to a byte array and compute the hash.
        var data = SHA256.HashData(Encoding.UTF8.GetBytes(value));
    
        // Create a new StringBuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();
    
        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        foreach (var t in data)
        {
            sBuilder.Append(t.ToString("x2"));
        }
    
        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
    
    public static string ToSha512(
        this string value)
    {
        // Convert the input string to a byte array and compute the hash.
        var data = SHA512.HashData(Encoding.UTF8.GetBytes(value));
    
        // Create a new StringBuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();
    
        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        foreach (var t in data)
        {
            sBuilder.Append(t.ToString("x2"));
        }
    
        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
    
    public static string? GetNameFromEmail(
        this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        try
        {
            var address = new MailAddress(value);
            return address.User;
        }
        catch (Exception)
        {
            return null;
        }
    }
}