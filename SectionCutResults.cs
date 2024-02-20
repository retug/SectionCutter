using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionCutter
{
    public class SectionCutResults
    {
        public SectionCutResults() { }
        public static List<int> FindIndices(string[] array, string value)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                {
                    indices.Add(i);
                }
            }
            return indices;
        }
    }
}
