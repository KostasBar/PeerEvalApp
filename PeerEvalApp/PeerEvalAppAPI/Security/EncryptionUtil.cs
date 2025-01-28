namespace PeerEvalAppAPI.Security
{
    public class EncryptionUtil
    {
        /// <summary>
        /// Encrypts a plain text password using BCrypt hashing.
        /// </summary>
        /// <param name="plainText">The plain text password that needs to be encrypted.</param>
        /// <returns>The encrypted password as a string.</returns>
        public static string Encrypt(string plainText)
        {
            var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(plainText);
            return encryptedPassword;
        }

        /// <summary>
        /// Verifies if the provided plain text password matches the encrypted password.
        /// </summary>
        /// <param name="plainText">The plain text password to verify.</param>
        /// <param name="cipherText">The encrypted password to compare against.</param>
        /// <returns>True if the plain text password matches the encrypted password, otherwise false.</returns>
        public static bool IsValidPassword(string plainText, string cipherText)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, cipherText);
        }
    }
}
