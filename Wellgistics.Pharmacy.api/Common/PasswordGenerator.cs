using System.Security.Cryptography;
using System.Text;

namespace Wellgistics.Pharmacy.api.Common
{
    public static class PasswordGenerator
    {
        // Private method to generate the password based on the email hash
        private static string GenerateComplexPasswordFromHash(string hash)
        {
            // Define sets of characters for complexity
            const string upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()_-+=<>?";

            // Create a StringBuilder to build the password
            StringBuilder password = new StringBuilder();

            // Combine all sets of characters into one
            string allChars = upperCaseChars + lowerCaseChars + digits + specialChars;

            // Use the hash to determine how many characters to pick from each set
            int hashLength = hash.Length;

            // Use the hash to generate different parts of the password
            password.Append(hash.Substring(0, 2)); // First 2 characters for some complexity
            password.Append(upperCaseChars[hashLength % upperCaseChars.Length]);
            password.Append(lowerCaseChars[hashLength % lowerCaseChars.Length]);
            password.Append(digits[hashLength % digits.Length]);
            password.Append(specialChars[hashLength % specialChars.Length]);
            password.Append(hash.Substring(hashLength - 2)); // Last 2 characters for some complexity

            // Step 5: Return the generated password (no randomization)
            return password.ToString();
        }

        // Public method to generate a password from the email
        public static string GeneratePasswordFromEmail(string email)
        {
            // Compute the hash of the email using SHA-256
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] emailBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(email));

                // Convert the hash to a string (hex format)
                StringBuilder hashString = new StringBuilder();
                foreach (byte b in emailBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }

                // Generate a complex password from the hash using the private method
                return GenerateComplexPasswordFromHash(hashString.ToString());
            }
        }
    }
}
