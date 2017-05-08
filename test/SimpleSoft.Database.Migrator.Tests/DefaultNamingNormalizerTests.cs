using Xunit;

namespace SimpleSoft.Database.Migrator.Tests
{
    public class DefaultNamingNormalizerTests
    {
        [Fact]
        public void GivenANormalizerWhenPassingNullNameThenNullMustBeReturned()
        {
            var normalizer = new DefaultNamingNormalizer();

            var normalizedName = normalizer.Normalize(null);

            Assert.Null(normalizedName);
        }

        [Fact]
        public void GivenANormalizerWhenPassingWhitespaceNameThenEmptyMustBeReturned()
        {
            var normalizer = new DefaultNamingNormalizer();

            var normalizedName = normalizer.Normalize("      ");

            Assert.NotNull(normalizedName);
            Assert.Same(string.Empty, normalizedName);
        }

        [Fact]
        public void GivenANormalizerWhenPassingNotFullUppercaseNameThenUppercaseMustBeReturned()
        {
            const string nonNormalizedName = "!Hello.123.World!";

            var normalizer = new DefaultNamingNormalizer();

            var normalizedName = normalizer.Normalize(nonNormalizedName);

            Assert.NotNull(normalizedName);
            Assert.Equal("!HELLO.123.WORLD!", normalizedName);
        }

        [Fact]
        public void GivenAgNormalizerWhenPassingNotTrimNameThenTrimMustBeReturned()
        {
            const string nonNormalizedName = "   !HELLO.123.WORLD!   ";

            var normalizer = new DefaultNamingNormalizer();

            var normalizedName = normalizer.Normalize(nonNormalizedName);

            Assert.NotNull(normalizedName);
            Assert.Equal("!HELLO.123.WORLD!", normalizedName);
        }

        [Fact]
        public void GivenANormalizerWhenPassingNotTrimAndUppercaseNameThenTrimAndUppercaseMustBeReturned()
        {
            const string nonNormalizedName = "   !Hello.123.World!   ";

            var normalizer = new DefaultNamingNormalizer();

            var normalizedName = normalizer.Normalize(nonNormalizedName);

            Assert.NotNull(normalizedName);
            Assert.Equal("!HELLO.123.WORLD!", normalizedName);
        }

        [Fact]
        public void GivenANormalizerWhenPassingNormalizedNameThenEqualValueMustBeReturned()
        {
            const string nonNormalizedName = "!HELLO.123.WORLD!";

            var normalizer = new DefaultNamingNormalizer();

            var normalizedName = normalizer.Normalize(nonNormalizedName);

            Assert.NotNull(normalizedName);
            Assert.Equal(nonNormalizedName, normalizedName);
        }
    }
}
