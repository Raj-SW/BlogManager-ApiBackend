using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Utils
{
    public static class PasswordHashing
    {
        public static byte[] HashPassword(string rawpassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(rawpassword);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                return hashedBytes;
            }
        }
    }
}