using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Jose;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace TestJWT
{
    class Program
    {
        const string secretKey = "f8a3efeffc5f3ef7db312f72ced5083fcfd0be740174e4ba4aea3c2a5b4ace53";
        const string publicKey = "MIICnTCCAYUCBEReYeAwDQYJKoZIhvcNAQEFBQAwEzERMA8GA1UEAxMIand0LTIwNDgwHhcNMTQwMTI0MTMwOTE2WhcNMzQwMjIzMjAwMDAwWjATMREwDwYDVQQDEwhqd3QtMjA0ODCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKhWb9KXmv45+TKOKhFJkrboZbpbKPJ9Yp12xKLXf8060KfStEStIX+7dCuAYylYWoqiGpuLVVUL5JmHgXmK9TJpzv9Dfe3TAc/+35r8r9IYB2gXUOZkebty05R6PLY0RO/hs2ZhrOozHMo+x216Gwz0CWaajcuiY5Yg1V8VvJ1iQ3rcRgZapk49RNX69kQrGS63gzj0gyHnRtbqc/Ua2kobCA83nnznCom3AGinnlSN65AFPP5jmri0l79+4ZZNIerErSW96mUF8jlJFZI1yJIbzbv73tL+y4i0+BvzsWBs6TkHAp4pinaI8zT+hrVQ2jD4fkJEiRN9lAqLPUd8CNkCAwEAATANBgkqhkiG9w0BAQUFAAOCAQEAnqBw3UHOSSHtU7yMi1+HE+9119tMh7X/fCpcpOnjYmhW8uy9SiPBZBl1z6vQYkMPcURnDMGHdA31kPKICZ6GLWGkBLY3BfIQi064e8vWHW7zX6+2Wi1zFWdJlmgQzBhbr8pYh9xjZe6FjPwbSEuS0uE8dWSWHJLdWsA4xNX9k3pr601R2vPVFCDKs3K1a8P/Xi59kYmKMjaX6vYT879ygWt43yhtGTF48y85+eqLdFRFANTbBFSzdRlPQUYa5d9PZGxeBTcg7UBkK/G+d6D5sd78T2ymwlLYrNi+cSDYD6S4hwZaLeEK6h7p/OoG02RBNuT4VqFRu5DJ6Po+C6JhqQ==";
    
        static void Main(string[] args)
        {
            string token = buildToken();
            Console.WriteLine(token);
        }

        public static X509Certificate2 getX509Cert()
        {
            return new X509Certificate2(Convert.FromBase64String(publicKey));
        }

        public static RSACryptoServiceProvider getRSAFromPublicKey()
        {
            X509Certificate2 cert = getX509Cert();
            return (RSACryptoServiceProvider)cert.PublicKey.Key;
        }

        public static string buildToken()
        {
            var payload = new
            {
                iss = "AR",
                generatedByPatientco = 1
            };

            //hash using sha256
            string hashed = JWT.Encode(payload, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);

            Console.WriteLine(hashed);
            Console.WriteLine("-----------------------");
           
            //encrypt using public key
            string jwe = JWT.Encode(hashed, getRSAFromPublicKey(), JweAlgorithm.RSA1_5, JweEncryption.A128CBC_HS256);
            return jwe;
        }
    }
}
