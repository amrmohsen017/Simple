using System;
using System.Security.Cryptography;
using System.Text;

public static class Hash
{
    private const int SaltSize = 32;

    public static byte[] GenerateSalt()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            var randomNumber = new byte[SaltSize];

            rng.GetBytes(randomNumber);
           
            return randomNumber;

        }
    }



    public static string Hash_this(string data)
    {
        //var salt = Hash.GenerateSalt();
        //string result = Encoding.UTF8.GetString(salt);

        string saltString = @"���{�#��#1�@��P��.�\\k!\u001aą��o��";
        var salt = Encoding.UTF8.GetBytes(saltString);



        using (var hmac = new HMACSHA256(salt))
        {
            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(data)));
        }
    }
}

