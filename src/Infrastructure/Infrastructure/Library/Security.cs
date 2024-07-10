using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Library;

public static class Security
{
    public static string errors = "";
    private static string cryptoPassword = "";
    private static string Salt = "";
    private static string HashAlgorithm = "SHA1";
    private static int PasswordIterations = 5;
    private static string InitialVector = "";
    private static int KeySize = 256;

    private const int SizeOfBuffer = 1024 * 8;

    public static string crypto(string SourceString, Boolean Encrypt)
    {
        try
        {
            if (cryptoPassword.Length == 0) cryptoPassword = @";{L6aT)i.n2q/U0S1xHcGmA$4xR[D0{Q@IkFoT2Ze/S9jN!:(B7K2t]Vf3}e/{r?b?;CwV8So0@D1#,^I_2{N(:NwP/C1(dO0QfJ5Bx#U:z";
            if (Salt.Length == 0)
            {
                for (int x = 0; x < 50; x++) Salt += (char)x;
                Salt += cryptoPassword.Substring(2, 10);
            }
            if (InitialVector.Length == 0)
            {
                for (int x = 195; x < 211; x++) InitialVector += (char)x;
            }

            if (Encrypt == true) return encrypt(SourceString);
            else return decrypt(SourceString);
        }
        catch (Exception ex)
        {
            errors = ex.Message;
            return "";
        }
    }

    private static string encrypt(string SourceString)
    {
        try
        {
            if (string.IsNullOrEmpty(SourceString)) return "";

            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(SourceString);

            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(cryptoPassword, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;

            byte[] CipherTextBytes = null;

            using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream())
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                    {
                        CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                        CryptoStream.FlushFinalBlock();
                        CipherTextBytes = MemStream.ToArray();
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }

            SymmetricKey.Clear();

            return Convert.ToBase64String(CipherTextBytes);
        }
        catch (Exception ex)
        {
            errors = ex.Message;
            return "";
        }
    }

    private static string decrypt(string guponSourceString)
    {
        try
        {
            if (string.IsNullOrEmpty(guponSourceString)) return "";

            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] CipherTextBytes = Convert.FromBase64String(guponSourceString);

            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(cryptoPassword, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);

            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            byte[] bornoBytes = new byte[guponSourceString.Length];

            int ByteCount = 0;

            using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream(CipherTextBytes))
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
                    {
                        ByteCount = CryptoStream.Read(bornoBytes, 0, bornoBytes.Length);
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }

            SymmetricKey.Clear();
            return Encoding.UTF8.GetString(bornoBytes, 0, ByteCount);
        }
        catch (Exception ex)
        {
            errors = ex.Message;
            return "";
        }
    }

    public static void EncryptFile(string inputPath, string outputPath)
    {
        try
        {
            if (cryptoPassword.Length == 0) cryptoPassword = @"m1aK|f?)ZD8Rm0aK|e?)YCt7!Rl0aJ{e>(YCt7!Ql/`J{d>(YBs6 /`Izd>'XBs6 Qk.";

            if (Salt.Length == 0)
            {
                for (int x = 0; x < 50; x++) Salt += (char)x;
                Salt += cryptoPassword.Substring(2, 10);
            }

            var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            var output = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);

            // Essentially, if you want to use RijndaelManaged as AES you need to make sure that:
            // 1.The block size is set to 128 bits
            // 2.You are not using CFB mode, or if you are the feedback size is also 128 bits

            var algorithm = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
            var key = new Rfc2898DeriveBytes(cryptoPassword, Encoding.ASCII.GetBytes(Salt));

            algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

            using (var encryptedStream = new CryptoStream(output, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
            {
                CopyStream(input, encryptedStream);
            }
        }
        catch (Exception ex)
        {
        }
    }

    public static void DecryptFile(string inputPath, string outputPath)
    {
        try
        {
            if (cryptoPassword.Length == 0) cryptoPassword = @"m1aK|f?)ZD8Rm0aK|e?)YCt7!Rl0aJ{e>(YCt7!Ql/`J{d>(YBs6 /`Izd>'XBs6 Qk.";

            var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            var output = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write);

            // Essentially, if you want to use RijndaelManaged as AES you need to make sure that:
            // 1.The block size is set to 128 bits
            // 2.You are not using CFB mode, or if you are the feedback size is also 128 bits
            var algorithm = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
            var key = new Rfc2898DeriveBytes(cryptoPassword, Encoding.ASCII.GetBytes(Salt));

            algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

            try
            {
                using (var decryptedStream = new CryptoStream(output, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    CopyStream(input, decryptedStream);
                }
            }
            catch
            { }
        }
        catch { }
    }

    private static void CopyStream(Stream input, Stream output)
    {
        try
        {
            using (output)
            using (input)
            {
                byte[] buffer = new byte[SizeOfBuffer];
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, read);
                }
            }
        }
        catch
        { }
    }

    public static string CreateMD5Hash(string input)
    {
        // Step 1, calculate MD5 hash from input
        MD5 md5 = MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }
        return sb.ToString();
    }
}