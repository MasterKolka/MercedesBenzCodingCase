using System.Text.RegularExpressions;

namespace UrlShortener.DataProvider.Helpers;

public static class HashAlphabetHelper
{
    public const int MaxHashLength = 6;
    public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    public static readonly Regex ShortenedUrlRegex = new Regex($"^[a-zA-Z0-9]{{1,{MaxHashLength}}}$", RegexOptions.Compiled);
    public static long MaxHashItems => (long)Math.Pow(Alphabet.Length, MaxHashLength);

    public static string NumberToHashString(long number)
    {
        var result = "";
        do
        {
            result += Alphabet[(int)(number % (long)Alphabet.Length)];
            number /= Alphabet.Length;
        } while (number > 0);

        return result;
    }

    public static long HashStringToNumber(string hash)
    {
        if (!ShortenedUrlRegex.IsMatch(hash))
            throw new Exception("Hash is invalid");

        long result = 0;
        for (var i = 0; i < hash.Length; i++)
        {
            result += Alphabet.IndexOf(hash[i]) * (long)Math.Pow(Alphabet.Length, i);
        }
        return result;
    }
}
