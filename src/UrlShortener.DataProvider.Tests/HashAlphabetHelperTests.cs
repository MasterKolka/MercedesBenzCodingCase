using FluentAssertions;
using System;
using UrlShortener.DataProvider.Helpers;
using Xunit;

namespace UrlShortener.DataProvider.Tests;

public class HashAlphabetHelperTests
{
    [Fact]
    public void VerifyHasher()
    {
        var random = new Random();
        for (int i = 0; i < 10000; i++)
        {
            var r = random.NextInt64(0L, HashAlphabetHelper.MaxHashItems - 1);
            var hash = HashAlphabetHelper.NumberToHashString(r);
            var value = HashAlphabetHelper.HashStringToNumber(hash);

            hash.Length.Should().BeLessThanOrEqualTo(HashAlphabetHelper.MaxHashLength);
            value.Should().Be(r);
        }
    }
}
