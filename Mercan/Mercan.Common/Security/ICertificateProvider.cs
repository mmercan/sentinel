using System;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Mercan.Common.Security
{
    public interface ICertificateProvider
    {
        X509Certificate2 GetCertificateByThumbprint(string storeName, string thumbprint, bool validOnly = true);
        X509Certificate2 GetCertificateByThumbprint(string storeName, StoreLocation storeLocation, string thumbprint, bool validOnly = true);
        X509Certificate2 FindCertificate(ICertificateFindCriteria criteria);
        X509Certificate2 FindCertificate(string storeName, X509FindType findType, object findValue, bool validOnly = true);
        X509Certificate2 FindCertificate(string storeName, StoreLocation storeLocation, X509FindType findType,
            object findValue, bool validOnly = true);
        X509Certificate2 GetCertificateFromFile(string fileName);
        X509Certificate2 GetCertificateFromFile(string fileName, string password);
        X509Certificate2 GetCertificateFromFile(string fileName, SecureString password);
        X509Certificate2 GetCertificateFromCertFile(string fileName);
        X509Certificate2 GetCertificateFromSignedFile(string fileName);
        X509Certificate2 ReadCertificate(Stream stream);
        X509Certificate2 ReadCertificate(Stream stream, string password);
        X509Certificate2 ReadCertificate(Stream stream, SecureString password);
        Task<X509Certificate2> ReadCertificateAsync(Func<Task<Stream>> streamFunc, string name);
        Task<X509Certificate2> ReadCertificateAsync(Func<Task<Stream>> streamFunc, string password, string name);
        Task<X509Certificate2> ReadCertificateAsync(Func<Task<Stream>> streamFunc, SecureString password, string name);
        X509Certificate2 FromBase64String(string rawString);
        X509Certificate2 FromBase64String(string rawString, string password);
        X509Certificate2 FromBase64String(string rawString, SecureString password);
        X509Certificate2 FromBase64String(Func<string> rawStringFunc, string name);
        X509Certificate2 FromBase64String(Func<string> rawStringFunc, string password, string name);
        X509Certificate2 FromBase64String(Func<string> rawStringFunc, SecureString password, string name);
        Task<X509Certificate2> FromBase64StringAsync(Func<Task<string>> rawStringAsyncFunc, string name);
        Task<X509Certificate2> FromBase64StringAsync(Func<Task<string>> rawStringAsyncFunc, string password, string name);
        Task<X509Certificate2> FromBase64StringAsync(Func<Task<string>> rawStringAsyncFunc, SecureString password, string name);
        X509Certificate2 FromConfiguration(string key);
        Task<X509Certificate2> FromConfigurationAsync(string key);
        Task<X509Certificate2> FromConfigurationAsync(string key, string password);
        Task<X509Certificate2> FromConfigurationAsync(string key, SecureString password);
    }
}