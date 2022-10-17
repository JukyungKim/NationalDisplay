using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Npgsql;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace NationalDisplay.Models;

public class AccountModel
{
    public enum PasswordScore
    {
        Blank = 0,
        VeryWeak = 1,
        Weak = 2,
        Medium = 3,
        Strong = 4,
        VeryStrong = 5
    }

    // public static string Hash(string password)
    // {
        // byte[] salt;
        // new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        // var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        // byte[] hash = pbkdf2.GetBytes(20);


        // var md5 = new MD5CryptoServiceProvider();
        // var md5data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));       
        // return Encoding.Default.GetString(md5data);


        // byte[] salt = new byte[128 / 8];
        // using(var rngCsp =  new RNGCryptoServiceProvider())
        // {
        //     rngCsp.GetNonZeroBytes(salt);
        // }
        // string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //     password: password,
        //     salt: salt,
        //     prf: KeyDerivationPrf.HMACSHA256,
        //     iterationCount: 100000,
        //     numBytesRequested: 256 / 8));

        // return hashed;

        
    // }

    public static void SaveSubAccount(string id, string password)
    {
        Console.WriteLine("Save sub account : {0}, {1}", id, password);
        // password = SecurePasswordHasher.Hash(password);
        password = Sha256encrypt(password);
        using (var conn = new NpgsqlConnection(
                    "host=localhost;username=postgres;password=1234;database=displaydb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = String.Format("INSERT INTO sub_account (id, password) VALUES('{0}', '{1}')", id, password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        while (reader.Read())
                        {
                            Console.Write(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
    static public bool CheckSubAccount(string id)
    {
        using (var conn = new NpgsqlConnection(
                    "host=localhost;username=postgres;password=1234;database=displaydb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = String.Format("select * from sub_account where id='{0}';", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        // while (reader.Read())
                        // {
                        //     Console.Write(reader.GetString(0));
                        // }
                        if(reader.Read()){
                            Console.WriteLine("Exist sub account id");
                            return true;
                        }
                        else{
                            Console.WriteLine("Not exist sub account id");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        return false;
    }

    public static void LoadSubAccount()
    {
        using (var conn = new NpgsqlConnection(
                    "host=localhost;username=postgres;password=1234;database=displaydb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = String.Format("SELECT password FROM sub_account");
                
                    using (var reader = cmd.ExecuteReader())
                    {
                        
                        Console.WriteLine(cmd.CommandText);
                        while (reader.Read())
                        {    
                            if(reader.IsDBNull(0) || reader.GetString(0) == ""){
                                Console.WriteLine("Not exist password");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }

    public static void RemoveSubAccount(string id)
    {
        using (var conn = new NpgsqlConnection(
                    "host=localhost;username=postgres;password=1234;database=displaydb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = String.Format("DELETE FROM sub_account WHERE id='{0}';", id);
                    Console.WriteLine("Remove Sensor : {0}", cmd.CommandText);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        while (reader.Read())
                        {
                            Console.Write(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public static void SaveAccount(string id, string password)
    {
        // password = SecurePasswordHasher.Hash(password);
        password = Sha256encrypt(password);
        Console.WriteLine("Save account : {0}, {1}", id, password);

        using (var conn = new NpgsqlConnection(
                    "host=localhost;username=postgres;password=1234;database=displaydb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = String.Format("INSERT INTO account (id, password) VALUES('{0}', '{1}')", id, password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        while (reader.Read())
                        {
                            Console.Write(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public static void UpdatePassword(string id, string password)
    {
        // password = SecurePasswordHasher.Hash(password);
        password = Sha256encrypt(password);
        Console.WriteLine("Update password : {0}, {1}", id, password);

        using (var conn = new NpgsqlConnection(
                    "host=localhost;username=postgres;password=1234;database=displaydb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    // cmd.CommandText = String.Format("UPDATE account SET password='{0}' WHERE id='{1}'", password, id);
                    if(id == "master"){
                        cmd.CommandText = String.Format("UPDATE account SET password='{0}' WHERE id='{1}'", password, id);
                    }
                    else{
                        cmd.CommandText = String.Format("UPDATE sub_account SET password='{0}' WHERE id='{1}'", password, id);
                    }
                
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        while (reader.Read())
                        {
                            Console.Write(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public static int CheckAccount(string id, string password)
    {
        int ok = 0;
        // password = AccountModel.Hash(password);
        // password = SecurePasswordHasher.Hash(password);
        Console.WriteLine("Check account : {0}, {1}", id, password);


        using (var conn = new NpgsqlConnection(
                    "host=localhost;username=postgres;password=1234;database=displaydb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    if(id == "master"){
                        cmd.CommandText = String.Format("SELECT password FROM account WHERE id='{0}'", id);
                    }
                    else{
                        cmd.CommandText = String.Format("SELECT password FROM sub_account WHERE id='{0}'", id);
                    }
                
                    using (var reader = cmd.ExecuteReader())
                    {
                        
                        Console.WriteLine(cmd.CommandText);
                        while (reader.Read())
                        {    
                            if(reader.IsDBNull(0) || reader.GetString(0) == ""){
                                Console.WriteLine("Not exist password");
                                return 1;
                            }

                            string pass = reader.GetString(0);
                            pass = pass.Replace(" ", String.Empty);
                            Console.WriteLine("Input password : {0},  save password : {1}", password, pass);
                            // var result = SecurePasswordHasher.Verify(password, pass);
                            bool result = false;
                            Console.WriteLine("Encrypt : " + pass + " " + Sha256encrypt(password));
                            Console.WriteLine("Encrypt : " + pass.Length + " " + Sha256encrypt(password).Length);
                            if(pass.Equals(Sha256encrypt(password))){
                                Console.WriteLine("Password OK");
                                result = true;
                            }

                            if(result){
                                ok = 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        return ok;
    }

    public static void SaveLogInfo(string id, int pass)
    {
         // String strHostName = string.Empty;
        // IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
        // IPAddress[] addr = ipEntry.AddressList;

        string ipAddress = "";
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddress = ip.ToString();
                Console.WriteLine("IP Address = " + ip.ToString());
            }
        }
            
        string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
        bool pass2 = false;
        string log;
        // if(pass == 1) pass2 = true;
        if(pass == 1) log = "로그인";
        else if(pass == 10) log = "로그아웃";
        else log = "로그인 실패";
        using (var conn = new NpgsqlConnection(
                    "host=localhost;username=postgres;password=1234;database=displaydb"))
        {
            try
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = String.Format("INSERT INTO log_info (id, time, pass, ip) VALUES('{0}', '{1}', '{2}', '{3}')", 
                        id, time, log, ipAddress);
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine(cmd.CommandText);
                        while (reader.Read())
                        {
                            Console.Write(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }


    public static PasswordScore CheckStrength(string password)
    {

        int score = 0;

        if (password.Length < 1)
            return PasswordScore.Blank;
        if (password.Length < 4)
            return PasswordScore.VeryWeak;

        if (password.Length >= 8)
            score++;
        if (password.Length >= 12)
            score++;
        // if (Regex.Match(password, @"/\d+/", RegexOptions.ECMAScript).Success)
        //     score++;
        if (Regex.Match(password, @"/[a-z]/", RegexOptions.ECMAScript).Success &&
          Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
            score++;
        if (Regex.Match(password, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
            score++;

        return (PasswordScore)score;
    }
    public static string Sha256encrypt(string phrase)
    {
        UTF8Encoding encoder = new UTF8Encoding();
        SHA256Managed sha256hasher = new SHA256Managed();
        byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));
        return Convert.ToBase64String(hashedDataBytes);
    }
}

public static class SecurePasswordHasher
{
    /// <summary>
    /// Size of salt.
    /// </summary>
    private const int SaltSize = 16;

    /// <summary>
    /// Size of hash.
    /// </summary>
    private const int HashSize = 20;

    /// <summary>
    /// Creates a hash from a password.
    /// </summary>
    /// <param name="password">The password.</param>
    /// <param name="iterations">Number of iterations.</param>
    /// <returns>The hash.</returns>
    public static string Hash(string password, int iterations)
    {
        // Create salt
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

        // Create hash
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
        var hash = pbkdf2.GetBytes(HashSize);

        // Combine salt and hash
        var hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        // Convert to base64
        var base64Hash = Convert.ToBase64String(hashBytes);

        // Format hash with extra information
        return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
    }

    /// <summary>
    /// Creates a hash from a password with 10000 iterations
    /// </summary>
    /// <param name="password">The password.</param>
    /// <returns>The hash.</returns>
    public static string Hash(string password)
    {
        return Hash(password, 10000);
    }

    /// <summary>
    /// Checks if hash is supported.
    /// </summary>
    /// <param name="hashString">The hash.</param>
    /// <returns>Is supported?</returns>
    public static bool IsHashSupported(string hashString)
    {
        return hashString.Contains("$MYHASH$V1$");
    }

    /// <summary>
    /// Verifies a password against a hash.
    /// </summary>
    /// <param name="password">The password.</param>
    /// <param name="hashedPassword">The hash.</param>
    /// <returns>Could be verified?</returns>
    public static bool Verify(string password, string hashedPassword)
    {
        // Check hash
        if (!IsHashSupported(hashedPassword))
        {
            throw new NotSupportedException("The hashtype is not supported");
        }

        // Extract iteration and Base64 string
        var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
        var iterations = int.Parse(splittedHashString[0]);
        var base64Hash = splittedHashString[1];

        // Get hash bytes
        var hashBytes = Convert.FromBase64String(base64Hash);

        // Get salt
        var salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        // Create hash with given salt
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Get result
        for (var i = 0; i < HashSize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }
        return true;
    }
}