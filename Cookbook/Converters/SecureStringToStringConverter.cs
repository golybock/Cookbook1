using System;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Data;

namespace Cookbook.Converters;

public class SecureStringToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return null;

        var secureString = (SecureString)value;
        var pointer = IntPtr.Zero;

        try
        {
            pointer = Marshal.SecureStringToGlobalAllocUnicode(secureString);
            return Marshal.PtrToStringUni(pointer);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(pointer);
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return null;

        var password = (string)value;
        var secureString = new SecureString();

        foreach (var c in password)
        {
            secureString.AppendChar(c);
        }

        secureString.MakeReadOnly();
        return secureString;
    }
    
    public static string? SecureStringToString(SecureString value)
    {
        return new NetworkCredential(string.Empty, value).Password;
    }
    
    // public static string? SecureStringToString(SecureString value) {
    //     IntPtr valuePtr = IntPtr.Zero;
    //     try {
    //         valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
    //         return Marshal.PtrToStringUni(valuePtr);
    //     } finally {
    //         Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
    //     }
    // }

    public static SecureString? StringToSecureString(string value)
    {
        return new NetworkCredential("", value).SecurePassword;
    } 
    
}