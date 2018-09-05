using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Mercan.Common.Security;
using Mercan.Common.Constants;

namespace Mercan.Common.Certificate
{
    /// <summary>
    /// Validates client certificate in HTTP Header (X-ARR-ClientCert).
    /// When hosting apps in Azure App Services, the client certificates in transport (system.net)
    /// get loaded as HTTP Header (X-ARR-ClientCert)
    /// </summary>
    public class ValidateCertificate : IValidateCertificate
    {
        private readonly IValidateCertificateSettings _settings;
        private readonly IValidateCertificateSettings _alternateSettings;
        private readonly ILogger _logger;
        private readonly ICertificateProvider _certificateProvider;        

		public ValidateCertificate(IValidateCertificateSettings settings, ICertificateProvider certificateProvider, ILogger logger)
            : this(settings, null, certificateProvider, logger)
        {
        }

        public ValidateCertificate(IValidateCertificateSettings settings, IValidateCertificateSettings alternateSettings,
            ICertificateProvider certificateProvider, ILogger logger)
        {
            _settings = settings;
            _alternateSettings = alternateSettings;
            _certificateProvider = certificateProvider;
            _logger = logger;
        }



        public virtual bool HasValidCertificate(HttpRequestMessage request)
        {            
            return IsValidCertificate(GetCertificateFromHeader(request));
        }

        protected X509Certificate2 GetCertificateFromHeader(HttpRequestMessage request)
        {
            IEnumerable<string> values;
            if (request.Headers.TryGetValues(HttpConstants.ARRClientCertificateHeaderName, out values))
            {
                string certHeader = values?.FirstOrDefault();
                if (!string.IsNullOrEmpty(certHeader))
                {
                    try
                    {
                        byte[] clientCertBytes = Convert.FromBase64String(certHeader);
                        return new X509Certificate2(clientCertBytes);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Checks that the client certificate matches the details supplied in configuration
        /// </summary>
        /// <param name="certificate">The certificate to check</param>
        /// <returns>True if the certificate is valid</returns>
        public bool IsValidCertificate(X509Certificate certificate)
        {
            return IsValidCertificate(certificate, null);
        }

        /// <summary>
        /// Checks that the client certificate matches the details supplied in configuration
        /// </summary>
        /// <param name="certificate">The certificate to check</param>
        /// <param name="certificateChain">The certificate's chain to check</param>
        /// <returns>True if the certificate is valid</returns>
        /// <remarks>
        /// The certificate is a valid certificate if the optional conditions below are met:
        /// 1. The certificate is not expired and is active for the current time on server.
        /// 2. The subject name of the certificate matches configuration
        /// 3. The issuer name of the certificate has the common name and organization name matching configuration
        /// 4. The serial number of the certificate matches configuration
        /// 5. Checks if the certificate is chained to a Trusted Root Authority(or revoked) on the server if chain is supplied
        /// </remarks>
        public bool IsValidCertificate(X509Certificate certificate, X509Chain certificateChain)
        {
            if (certificate is X509Certificate2)
                return IsValidCertificate((X509Certificate2)certificate, certificateChain);

            if (!_settings.IsValidationEnabled())
                return true;

            if (certificate == null)
            {
                _logger?.LogError("Expected certificate not supplied");
                return false;
            }

            ValidationResult mainResult = ValidateX509Certificate(certificate, certificateChain, _settings);
            if (mainResult.Status == ValidationStatus.Success)
                return true;

            if (_alternateSettings == null || !_alternateSettings.IsValidationEnabled())
            {
                _logger?.LogError(mainResult.Message);
            }
            else
            {
                ValidationResult alternateStatus = ValidateX509Certificate(certificate, certificateChain, _alternateSettings);
                if (alternateStatus.Status == ValidationStatus.Success)
                    return true;

                _logger?.LogError($"{mainResult.Message} or {alternateStatus.Message}");
            }

            return false;
        }

        private ValidationResult ValidateX509Certificate(X509Certificate certificate, X509Chain certificateChain, IValidateCertificateSettings settings)
        {
            DateTime notBefore;
            DateTime notAfter;

            // Check time validity of certificate
            if (!DateTime.TryParse(certificate.GetEffectiveDateString(), out notBefore)
                || !DateTime.TryParse(certificate.GetExpirationDateString(), out notAfter))
                return new ValidationResult(ValidationStatus.Expiration, "Invalid effective or expiration date");

            ValidationResult result = ValidateExpiration(notBefore, notAfter);
            if (result.Status != ValidationStatus.Success)
                return result;

            result = ValidateSubjectAndIssuer(certificate, settings.Subject, settings.Issuer);
            if (result.Status != ValidationStatus.Success)
                return result;

            result = ValidateSerialNumber(certificate, settings.SerialNumber);
            if (result.Status != ValidationStatus.Success)
                return result;

            // Verify the certificate chains to a Trusted Root Authority should be checked uncomment the code below
            if (certificateChain != null && settings.VerifyChain)
                result = ValidateCertificateChain(certificateChain);

            return result;
        }

        /// <summary>
        /// Checks that the client certificate matches the details supplied in configuration
        /// </summary>
        /// <param name="certificate">The certificate to check</param>
        /// <param name="certificateChain">The certificate's chain to check</param>
        /// <returns>True if the certificate is valid</returns>
        public bool IsValidCertificate(X509Certificate2 certificate)
        {
            return IsValidCertificate(certificate, null);
        }

        /// <summary>
        /// Checks that the client certificate matches the details supplied in configuration
        /// </summary>
        /// <param name="certificate">The certificate to check</param>
        /// <param name="certificateChain">The certificate's chain to check</param>
        /// <returns>True if the certificate is valid</returns>
        /// <remarks>
        /// The certificate is a valid certificate if the optional conditions below are met:
        /// 1. The certificate is not expired and is active for the current time on server.
        /// 2. The subject name of the certificate matches configuration
        /// 3. The issuer name of the certificate has the common name and organization name matching configuration
        /// 4. The thumbprint of the certificate matches configuration
        /// 5. Checks if the certificate is chained to a Trusted Root Authority(or revoked) on the server
        /// </remarks>
        public bool IsValidCertificate(X509Certificate2 certificate, X509Chain certificateChain)
        {
            if (!_settings.IsValidationEnabled())
                return true;

            if (certificate == null)
            {
                _logger?.LogError("Expected certificate not supplied");
                return false;
            }

            ValidationResult mainResult = ValidateX509Certificate(certificate, certificateChain, _settings);
            if (mainResult.Status == ValidationStatus.Success)
                return true;

            if (_alternateSettings == null || !_alternateSettings.IsValidationEnabled())
            {
                _logger?.LogError(mainResult.Message);
            }
            else
            {
                ValidationResult alternateStatus = ValidateX509Certificate(certificate, certificateChain, _alternateSettings);
                if (alternateStatus.Status == ValidationStatus.Success)
                    return true;

                _logger?.LogError($"{mainResult.Message} or {alternateStatus.Message}");
            }

            return false;
        }

        private ValidationResult ValidateX509Certificate(X509Certificate2 certificate, X509Chain certificateChain, IValidateCertificateSettings settings)
        {
            // 1. Check time validity of certificate
            ValidationResult result = ValidateExpiration(certificate.NotBefore, certificate.NotAfter);
            if (result.Status != ValidationStatus.Success)
                return result;

            string thumbprint = settings.Thumbprint;

            if (settings.FindValue == null)
            {
                result = ValidateSubjectAndIssuer(certificate, settings.Subject, settings.Issuer);
                if (result.Status != ValidationStatus.Success)
                    return result;

                result = ValidateSerialNumber(certificate, settings.SerialNumber);
                if (result.Status != ValidationStatus.Success)
                    return result;
            }
            else
            {
                X509Certificate2 matchCertificate = _certificateProvider.FindCertificate(settings);

                if (matchCertificate == null)
                    throw new InvalidOperationException("Match certificate could not be found");

                result = ValidateSubjectAndIssuer(certificate, matchCertificate.Subject, matchCertificate.Issuer);
                if (result.Status != ValidationStatus.Success)
                    return result;

                if (string.IsNullOrEmpty(thumbprint))
                    thumbprint = matchCertificate.Thumbprint.Trim();
                else if (!string.Equals(thumbprint, matchCertificate.Thumbprint, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"Match certificate's thumbprint must match validation thumbprint if supplied");
            }

            // 4. Check thumprint of certificate
            string certificateThumbprint = certificate.Thumbprint.Trim();
            if (!string.Equals(certificateThumbprint, thumbprint, StringComparison.InvariantCultureIgnoreCase))
            {
                result = new ValidationResult(ValidationStatus.Thumbprint, $"Supplied certificate's thumbprint ({certificateThumbprint}) is incorrect");
                if (result.Status != ValidationStatus.Success)
                    return result;
            }

            // 5. Verify the certificate chains to a Trusted Root Authority should be checked uncomment the code below
            if (settings.VerifyChain)
            {
                if (certificateChain == null)
                {
                    certificateChain = new X509Chain();
                    if (!_settings.EnableRevocationListCheck)
                    {
                        // don't use Revocation List Check
                        certificateChain.ChainPolicy = new X509ChainPolicy() { RevocationMode = X509RevocationMode.NoCheck };
                    }
                    certificateChain.Build(certificate);
                }

                result = ValidateCertificateChain(certificateChain);
            }

            return result;
        }

        private ValidationResult ValidateExpiration(DateTime notBefore, DateTime notAfter)
        {
            if (DateTime.UtcNow < notBefore || DateTime.UtcNow > notAfter)
                return new ValidationResult(ValidationStatus.Expiration, $"Certificate is for an invalid time period {notBefore:u} - {notAfter:u}");

            return ValidationResult.Success;
        }

        private ValidationResult ValidateSubjectAndIssuer(X509Certificate certificate, string subject, string issuer)
        {
            if (!string.IsNullOrEmpty(subject) && !string.Equals(certificate.Subject, subject, StringComparison.OrdinalIgnoreCase))
                return new ValidationResult(ValidationStatus.Subject, $"Supplied certificate's subject ({certificate.Subject}) is incorrect");

            if (!string.IsNullOrEmpty(issuer) && !string.Equals(certificate.Issuer, issuer, StringComparison.OrdinalIgnoreCase))
                return new ValidationResult(ValidationStatus.Issuer, $"Supplied certificate's issuer ({certificate.Issuer}) is incorrect");

            return ValidationResult.Success;
        }

        private ValidationResult ValidateSerialNumber(X509Certificate certificate, string serialNumber)
        {
            if (!string.IsNullOrEmpty(serialNumber))
            {
                string certSerialNumber = certificate.GetSerialNumberString();
                if (!string.Equals(certSerialNumber, serialNumber, StringComparison.OrdinalIgnoreCase))
                    return new ValidationResult(ValidationStatus.SerialNumber, $"Supplied client certificate's serial number ({certSerialNumber}) is incorrect");
            }

            return ValidationResult.Success;
        }

        protected virtual ValidationResult ValidateCertificateChain(X509Chain certificateChain)
        {
            if (certificateChain.ChainElements.Cast<X509ChainElement>().Any(e => !e.Certificate.Verify()))
                return new ValidationResult(ValidationStatus.CertificateChain, "Supplied certificate is not trusted");

            return ValidationResult.Success;
        }

        protected enum ValidationStatus
        {
            Success,
            Expiration,
            Issuer,
            Subject,
            Thumbprint,
            SerialNumber,
            CertificateChain
        }
        protected class ValidationResult
        {
            public static readonly ValidationResult Success = new ValidationResult(ValidationStatus.Success, null);

            private ValidationStatus _status;
            private string _message;

            public ValidationResult(ValidationStatus status, string message)
            {
                _status = status;
                _message = message;
            }

            public ValidationStatus Status
            {
                get { return _status; }
                set { _status = value; }
            }

            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
    }
}
