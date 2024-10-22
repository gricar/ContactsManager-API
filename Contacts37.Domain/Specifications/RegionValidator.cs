namespace Contacts37.Domain.Specifications
{
    public class RegionValidator : IRegionValidator
    {
        private static readonly Dictionary<string, int[]> Regions = new Dictionary<string, int[]>
        {
            { "Centro-Oeste", new[] { 61, 62, 64, 65, 66, 67 } },
            { "Nordeste", new[] { 71, 73, 74, 75, 77, 79, 81, 82, 83, 84, 85, 86, 87, 88, 89, 98, 99 } },
            { "Norte", new[] { 63, 68, 69, 91, 92, 93, 94, 95, 96, 97 } },
            { "Sudeste", new[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 24, 27, 28, 31, 32, 33, 34, 35, 37, 38 } },
            { "Sul", new[] { 41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 53, 54, 55 } }
        };

        public bool IsValid(int dddCode)
        {
            return Regions.Values.Any(region => region.Contains(dddCode));
        }
    }
}
