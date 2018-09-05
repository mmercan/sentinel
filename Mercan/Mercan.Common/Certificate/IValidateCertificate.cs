using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace Mercan.Common.Certificate
{
    public interface IValidateCertificate
    {
        bool HasValidCertificate(HttpRequestMessage request);
        bool IsValidCertificate(X509Certificate2 certificate);
        bool IsValidCertificate(X509Certificate2 certificate, X509Chain certificateChain);
        bool IsValidCertificate(X509Certificate certificate);
        bool IsValidCertificate(X509Certificate certificate, X509Chain certificateChain);
    }
}
