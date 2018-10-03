using Mercan.Common.Security;

namespace Mercan.Common.Certificate
{
    public class ValidateCertificateSettings : CertificateFindCriteria, IValidateCertificateSettings
    {
        public string Issuer { get; set; }

        public string SerialNumber { get; set; }

        public string Subject { get; set; }

        public string Thumbprint { get; set; }

        public bool VerifyChain { get; set; }

        public bool IsValidationEnabled()
        {
            return !string.IsNullOrEmpty(Thumbprint) || FindValue != null || (!string.IsNullOrEmpty(Issuer) && !string.IsNullOrEmpty(SerialNumber));
        }

        public bool EnableRevocationListCheck { get; set; }
    }
}
