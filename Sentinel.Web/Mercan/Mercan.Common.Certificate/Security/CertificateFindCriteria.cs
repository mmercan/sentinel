using System.Security.Cryptography.X509Certificates;

namespace Mercan.Common.Security
{
    public class CertificateFindCriteria : ICertificateFindCriteria
    {

        private X509FindType _findType;
        private object _findValue;

        public CertificateFindCriteria()
        {
            _findType = X509FindType.FindByThumbprint;
        }

        public CertificateFindCriteria(string thumbprint)
        {
            SetFindOptions(X509FindType.FindByThumbprint, thumbprint);
        }

        public CertificateFindCriteria(X509FindType findType, object findValue)
        {
            SetFindOptions(findType, findValue);
        }

        public string StoreName { get; set; } = "My";

        public StoreLocation StoreLocation { get; set; } = StoreLocation.CurrentUser;

        public X509FindType FindType
        {
            get { return _findType; }
        }

        public object FindValue
        {
            get { return _findValue; }
        }

        public bool ValidOnly { get; set; } = false;

        public void SetFindOptions(X509FindType findType, object findValue)
        {
            _findType = findType;
            _findValue = findValue;
        }

        public override string ToString()
        {
            return $"{StoreName} StoreLocation, {_findType} {_findValue}";
        }
    }
}
