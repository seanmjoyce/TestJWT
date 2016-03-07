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
        const string publicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDQunccB2bGy6L95JEteXlUHqP2c50xTbQBaR3TeAIoUZA4A63qpjcQ2WmL5wEq4bmkQqdNpjYF1xYDyAGwtxlwOA+tpqTXcz7+vEeR1S/WCepjbhe/yNg5wsV3wy1NPsKVDcdAXM09Gw7xuvsotS8NuVGYpDwd2QY7BwN+EZZ2vwIDAQAB";

    
        static void Main(string[] args)
        {
           string token = buildToken();
           Console.WriteLine(token);
        }

        public static RSACryptoServiceProvider getRSAFromPublicKey()
        {
            RSACryptoServiceProvider RSA = PEMKeyLoader.CryptoServiceProviderFromPublicKeyInfo(publicKey);
            return RSA;
        }

        public static string buildToken()
        {
            var payload = new
            {
                iss = "AR",
                generatedByPatientco = 0,
                testField = "ABC"
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
