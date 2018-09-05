using Mercan.Common.Security;
using Microsoft.Extensions.Configuration;

namespace Mercan.Common.Certificate
{
    public class ValidateCertificateConfigSettings : CertificateConfigFindCriteria, IValidateCertificateSettings
    {
        private IConfiguration _configurationManager;
        private readonly string _thumbprintKey;
        private readonly string _subjectKey;
        private readonly string _issuerKey;
        private readonly string _verifyChainKey;
        private readonly string _serialNumberKey;
        private readonly string _enableRevocationListCheckKey;

        private const string _defaultKeyPrefix = "ValidCert";
        private const string _thumbprintKeySuffix = "Thumbprint";
        private const string _subjectKeySuffix = "Subject";
        private const string _issuerKeySuffix = "Issuer";
        private const string _verifyChainKeySuffix = "VerifyChain";
        private const string _serialNumberSuffix = "SerialNumber";
        private const string _enableRevocationListCheckSuffix = "EnableRevocationListCheck";

        public ValidateCertificateConfigSettings(IConfiguration configurationManager)
            : this(configurationManager, _defaultKeyPrefix)
        {
        }

        public ValidateCertificateConfigSettings(IConfiguration configurationManager, string keyPrefix)
            : base(configurationManager, keyPrefix)
        {
            _configurationManager = configurationManager;
            _thumbprintKey = keyPrefix + _thumbprintKeySuffix;
            _subjectKey = keyPrefix + _subjectKeySuffix;
            _issuerKey = keyPrefix + _issuerKeySuffix;
            _verifyChainKey = keyPrefix + _verifyChainKeySuffix;
            _serialNumberKey = keyPrefix + _serialNumberSuffix;
            _enableRevocationListCheckKey = keyPrefix + _enableRevocationListCheckSuffix;
        }

        public string Thumbprint
        {
            get { return _configurationManager[_thumbprintKey]; }
        }

        public string Subject
        {
            get { return _configurationManager[_subjectKey]; }
        }

        public string Issuer
        {
            get { return _configurationManager[_issuerKey]; }
        }

        public bool VerifyChain
        {
            get
            {
                bool verifyChain;
                string verifyChainSetting = _configurationManager[_verifyChainKey];
                return !string.IsNullOrEmpty(verifyChainSetting) && bool.TryParse(verifyChainSetting, out verifyChain) && verifyChain;
            }
        }

        public string SerialNumber
        {
            get { return _configurationManager[_serialNumberKey]; }
        }

        public bool IsValidationEnabled()
        {
            return !string.IsNullOrEmpty(Thumbprint) || FindValue != null || (!string.IsNullOrEmpty(Issuer) && !string.IsNullOrEmpty(SerialNumber));
        }

        public bool EnableRevocationListCheck
        {
            get
            {
                bool enableRevocationListCheck;
                string enableRevocationListSetting = _configurationManager[_enableRevocationListCheckKey];
                return !string.IsNullOrEmpty(enableRevocationListSetting) && bool.TryParse(enableRevocationListSetting, out enableRevocationListCheck) && enableRevocationListCheck;
            }
        }
    }
}
