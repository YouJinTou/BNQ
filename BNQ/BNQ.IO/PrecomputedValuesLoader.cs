using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BNQ.IO
{
    public class PrecomputedValuesLoader : IValuesLoader
    {
        private string storagePath;
        private IDictionary<double, ulong[]> preflopRanges;

        /// <summary>
        /// In ../Storage/, looks for:
        /// <para>/HandPercentages.json</para>
        /// </summary>
        /// <param name="storagePath">The Storage folder path.</param>
        public PrecomputedValuesLoader(string storagePath)
        {
            if (!Directory.Exists(storagePath))
            {
                throw new DirectoryNotFoundException("The storage directory does not exist.");
            }

            this.storagePath = storagePath;

            this.LoadPreflopRanges();
        }

        public ulong[] GetRange(double range)
        {
            double closestRange = 1;
            double bestDistance = double.MaxValue;

            foreach (var pfRange in this.preflopRanges)
            {
                double currentDistance = Math.Abs(pfRange.Key - range);

                if (currentDistance <= bestDistance)
                {
                    closestRange = pfRange.Key;
                    bestDistance = currentDistance;
                }
            }

            return this.preflopRanges[closestRange];
        }

        private void LoadPreflopRanges()
        {
            string rangesFile = Path.Combine(this.storagePath, "HandPercentages.json");

            if (!File.Exists(rangesFile))
            {
                throw new FileNotFoundException("Could not locate ../Storage/HandPercentages.json");
            }

            IDictionary<double, ulong[]> ranges;

            try
            {
                ranges = JsonConvert.DeserializeObject<
                        IDictionary<double, ulong[]>>(File.ReadAllText(rangesFile));
            }
            catch
            {
                throw new IOException("Could not deserialize HandPercentages.json");
            }

            List<ulong> prevRange = new List<ulong>();
            this.preflopRanges = new Dictionary<double, ulong[]>();

            foreach (var range in ranges)
            {
                prevRange.AddRange(range.Value);
                this.preflopRanges.Add(range.Key / 100.0, prevRange.ToArray());
            }     
        }
    }
}
