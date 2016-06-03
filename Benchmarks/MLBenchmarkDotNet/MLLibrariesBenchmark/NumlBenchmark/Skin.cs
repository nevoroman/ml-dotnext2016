using numl.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MLLibrariesBenchmark.NumlBenchmark
{
    public class Skin
    {
        [Feature]
        public decimal R { get; set; }
        [Feature]
        public decimal G { get; set; }
        [Feature]
        public decimal B { get; set; }
        [StringLabel]
        public string Label { get; set; }

        public static IEnumerable<Skin> LoadData(string path)
        {
            var dataLines = File.ReadAllLines(path).Skip(1);
            return dataLines
                .Select(line => line.Split(','))
                .Select(data => new Skin
            {
                R = int.Parse(data[0]),
                G = int.Parse(data[1]),
                B = int.Parse(data[2]),
                Label = data[3]
            });
        }
    }
}
