using System.Security.Cryptography;
using System.Text;

namespace eSchool.Utils
{
    public static class PasswordGenerator
    {
        public static string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            StringBuilder password = new();
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] byteBuffer = new byte[length];

                rng.GetBytes(byteBuffer);

                for (int i = 0; i < length; i++)
                {
                    password.Append(validChars[byteBuffer[i] % validChars.Length]);
                }
            }
            return password.ToString();
        }
    }
}
