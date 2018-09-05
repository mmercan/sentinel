using System.Security.Cryptography.X509Certificates;

namespace Mercan.Common.Security
{
    public interface ICertificateFindCriteria
    {
        string StoreName { get; set; }
        StoreLocation StoreLocation { get; set; }
        X509FindType FindType { get; }
        object FindValue { get; }
        bool ValidOnly { get; set; }
        void SetFindOptions(X509FindType findType, object findValue);
    }
}
