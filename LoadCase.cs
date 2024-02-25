using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionCutter
{
    public class LoadCase
    {
        public int NumberNames { get; set; }
        public string MyName { get; set; }
        public int Status { get; set; }
        public Dictionary<string, bool> SeismicInfo { get; set; }
        public List<string> SeismicLoadDirection { get; set; }

        // Method to return ordered list of seismic directions
        public List<string> GetOrderedSeismicDirections()
        {
            // Define the desired order
            string[] order = { "EQx", "EQy", "EQx + E", "EQy + E", "EQx - E", "EQy - E" };

            // Sort keys based on values and then order
            var orderedKeys = SeismicInfo.OrderBy(kv => kv.Value)
                                          .ThenBy(kv => Array.IndexOf(order, kv.Key))
                                          .Select(kv => kv.Key)
                                          .ToList();

            // Filter out keys with false values
            var filteredKeys = orderedKeys.Where(key => SeismicInfo[key]).ToList();

            return filteredKeys;
        }


    }
}
