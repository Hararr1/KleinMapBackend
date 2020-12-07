using KleinMapLibrary.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace KleinMapLibrary.Helpers
{
    public static class EncryptionHelper
    {
        public static string GetMd5Hash(Subscriber user)
        {
            string input = user.Id.ToString() + user.MailAddress + user.StationId.ToString();

            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public static bool VerifyMd5Hash(Subscriber user, string hash)
        {
            string hashOfInput = GetMd5Hash(user);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
