using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;

namespace Mercan.Common.Security
{
    public class CertificateConfigFindCriteria : CertificateFindCriteria
    {
        protected const string DefaultKeyPrefix = "Cert";

        private const string _storeNameKeySuffix = "Store";
        private const string _storeLocationKeySuffix = "Location";
        private const string _findTypeKeySuffix = "FindType";
        private const string _findValueKeySuffix = "FindValue";
        private const string _validOnlyKeySuffix = "ValidOnly";

        public CertificateConfigFindCriteria(IConfiguration configurationManager, string keyPrefix = DefaultKeyPrefix)
        {
            string storeName = configurationManager[keyPrefix + _storeNameKeySuffix];
            if (!string.IsNullOrWhiteSpace(storeName))
                StoreName = storeName;

            string storeLocationSetting = configurationManager[keyPrefix + _storeLocationKeySuffix];
            if (!string.IsNullOrWhiteSpace(storeLocationSetting))
            {
                StoreLocation storeLocation;

                if (Enum.TryParse(storeLocationSetting, true, out storeLocation))
                    StoreLocation = storeLocation;
                else
                    StoreLocation = StoreLocation.LocalMachine;
            }

            SetFindOptions(configurationManager, keyPrefix);

            string validOnlySetting = configurationManager[keyPrefix + _validOnlyKeySuffix];
            if (!string.IsNullOrWhiteSpace(validOnlySetting))
            {
                bool validOnly;
                bool.TryParse(validOnlySetting, out validOnly);
                ValidOnly = validOnly;
            }
        }

        private void SetFindOptions(IConfiguration configurationManager, string keyPrefix)
        {
            const string findTypePrefix = "FindBy";

            string findTypeSetting = configurationManager[keyPrefix + _findTypeKeySuffix];
            X509FindType findType;
            if (!string.IsNullOrWhiteSpace(findTypeSetting))
            {
                if (!Enum.TryParse(findTypePrefix + findTypeSetting, true, out findType))
                    findType = X509FindType.FindByThumbprint;
            }
            else
            {
                findType = X509FindType.FindByThumbprint;
            }

            string findValue = configurationManager[keyPrefix + _findValueKeySuffix];

            if (findType == X509FindType.FindByTimeExpired || findType == X509FindType.FindByTimeNotYetValid || findType == X509FindType.FindByTimeValid)
            {
                DateTime findTime;
                SetFindOptions(findType, DateTime.TryParse(findValue, out findTime) ? (object)findTime : null);
            }
            else
            {
                SetFindOptions(findType, string.IsNullOrEmpty(findValue) ? null : findValue);
            }
        }
    }
}
