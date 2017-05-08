using Xunit;

namespace SimpleSoft.Database.Migrator.Tests
{
    public class TrimNamingNormalizerTests
    {
        [Fact]
        public void GivenANormalizerWhenPassingNullNameThenNullMustBeReturned()
        {
            var normalizer = new TrimNamingNormalizer();

            var normalizedName = normalizer.Normalize(null);

            Assert.Null(normalizedName);
        }

        [Fact]
        public void GivenANormalizerWhenPassingWhitespaceNameThenEmptyMustBeReturned()
        {
            var normalizer = new TrimNamingNormalizer();

            var normalizedName = normalizer.Normalize("      ");

            Assert.NotNull(normalizedName);
            Assert.Same(string.Empty, normalizedName);
        }

        [Fact]
        public void GivenAgNormalizerWhenPassingNotTrimNameThenTrimMustBeReturned()
        {
            const string nonNormalizedName = "   !Hello.123.World!   ";

            var normalizer = new TrimNamingNormalizer();

            var normalizedName = normalizer.Normalize(nonNormalizedName);

            Assert.NotNull(normalizedName);
            Assert.Equal("!Hello.123.World!", normalizedName);
        }

        [Fact]
        public void GivenANormalizerWhenPassingNormalizedNameThenEqualValueMustBeReturned()
        {
            const string nonNormalizedName = "!Hello.123.World!";

            var normalizer = new TrimNamingNormalizer();

            var normalizedName = normalizer.Normalize(nonNormalizedName);

            Assert.NotNull(normalizedName);
            Assert.Equal(nonNormalizedName, normalizedName);
        }
    }
}