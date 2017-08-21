using NUnit.Framework;

namespace BNQ.IO.Tests
{
    [TestFixture]
    public class PrecomputedValuesLoaderTests
    {
        private const string StoragePath = @"D:\SVN\BNQ\trunk\BNQ\BNQ.Playground\Storage";

        [TestCase(1326)]
        public void PrecomputedValuesLoader_LoadAndParsePreflopRanges_ValidTotalCount(int totalCombos)
        {
            var loader = new PrecomputedValuesLoader(StoragePath);
            var combos = loader.GetRange(100.0);

            Assert.AreEqual(totalCombos, combos.Length);
        }

        [TestCase(0.005, 6)]
        [TestCase(0.019, 24)]
        public void PrecomputedValuesLoader_LoadAndParsePreflopRanges_ValidComboCounts(double range, int comboCount)
        {
            var loader = new PrecomputedValuesLoader(StoragePath);
            var combos = loader.GetRange(range);

            Assert.AreEqual(comboCount, combos.Length);
        }
    }
}
