using Mercan.Common.Security;

namespace Mercan.Common.Certificate
{
    public interface IValidateCertificateSettings : ICertificateFindCriteria
    {
        string Thumbprint { get; }
        string Subject { get; }
        string Issuer { get; }
        string SerialNumber { get; }
        bool VerifyChain { get; }
        bool IsValidationEnabled();
        bool EnableRevocationListCheck { get; }
    }
}
