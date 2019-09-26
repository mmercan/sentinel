using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Polly;
using Polly.Extensions.Http;

namespace Sentinel.Common
{
    public static class HttpClientHelpers
    {
        public static X509Certificate2 GetCert()
        {
            // //          "-----BEGIN CERTIFICATE-----"
            // var cert64 = "MIICYzCCAcygAwIBAgIBADANBgkqhkiG9w0BAQUFADAuMQswCQYDVQQGEwJVUzEMMAoGA1UEChMDSUJNMREwDwYDVQQLEwhMb2NhbCBDQTAeFw05OTEyMjIwNTAwMDBaFw0wMDEyMjMwNDU5NTlaMC4xCzAJBgNVBAYTAlVTMQwwCgYDVQQKEwNJQk0xETAPBgNVBAsTCExvY2FsIENBMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQD2bZEo7xGaX2/0GHkrNFZvlxBou9v1Jmt/PDiTMPve8r9FeJAQ0QdvFST/0JPQYD20rH0bimdDLgNdNynmyRoS2S/IInfpmf69iyc2G0TPyRvmHIiOZbdCd+YBHQi1adkj17NDcWj6S14tVurFX73zx0sNoMS79q3tuXKrDsxeuwIDAQABo4GQMIGNMEsGCVUdDwGG+EIBDQQ+EzxHZW5lcmF0ZWQgYnkgdGhlIFNlY3VyZVdheSBTZWN1cml0eSBTZXJ2ZXIgZm9yIE9TLzM5MCAoUkFDRikwDgYDVR0PAQH/BAQDAgAGMA8GA1UdEwEB/wQFMAMBAf8wHQYDVR0OBBYEFJ3+ocRyCTJw067dLSwr/nalx6YMMA0GCSqGSIb3DQEBBQUAA4GBAMaQzt+zaj1GU77yzlr8iiMBXgdQrwsZZWJo5exnAucJAEYQZmOfyLiMD6oYq+ZnfvM0n8G/Y79q8nhwvuxpYOnRSAXFp6xSkrIOeZtJMY1h00LKp/JX3Ng1svZ2agE126JHsQ0bhzN5TKsYfbwfTwfjdWAGy6Vf1nYi/rO+ryMO";
            // //"-----END CERTIFICATE----- "
            // var certbyte = Convert.FromBase64String(cert64);
            // X509Certificate2 cert = new X509Certificate2(certbyte);
            // var certsubject = cert.Subject;
            // return cert;

            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            //for some reason the cert collection .Find(...) method can't find a cert by thumprint so we created our own.
            var clientCert = FindCert(store, "53e0a006198c8939cd4f8469381d3cdcd2f93c2f");
            store.Dispose();
            return clientCert;
        }

        public static X509Certificate2 FindCert(X509Store store, string thumbprint)
        {
            foreach (var cert in store.Certificates)
                if (cert.Thumbprint.Equals(thumbprint,
                    StringComparison.CurrentCultureIgnoreCase))
                    return cert;
            return null;
        }

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
    public class CertMessageHandler : HttpClientHandler
    {
        public CertMessageHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual;
            ClientCertificates.Add(HttpClientHelpers.GetCert());
        }
    }
}