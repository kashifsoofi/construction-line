using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

        }


        public SearchResults Search(SearchOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var sizesToSearch = options.Sizes.ToHashSet();
            var colorsToSearch = options.Colors.ToHashSet();

            var matchedShirts = _shirts.Where(shirt =>
                (sizesToSearch.Count == 0 || sizesToSearch.Contains(shirt.Size)) &&
                (colorsToSearch.Count == 0 || colorsToSearch.Contains(shirt.Color)));
            
            var sizeCounts = matchedShirts.GroupBy(s => s.Size).Select(g => new SizeCount { Size = g.Key, Count = g.Count() });
            var unmatchedSizeCounts = Size.All.Where(s => !sizeCounts.Any(sc => sc.Size == s)).Select(s => new SizeCount { Size = s, Count = 0 });

            var colorCounts = matchedShirts.GroupBy(s => s.Color).Select(g => new ColorCount { Color = g.Key, Count = g.Count() });
            var unmatchedColorCounts = Color.All.Where(c => !colorCounts.Any(cc => cc.Color == c)).Select(c => new ColorCount { Color = c });

            return new SearchResults
            {
                Shirts = matchedShirts.ToList(),
                SizeCounts = sizeCounts.Union(unmatchedSizeCounts).ToList(),
                ColorCounts = colorCounts.Union(unmatchedColorCounts).ToList(),
            };
        }
    }
}