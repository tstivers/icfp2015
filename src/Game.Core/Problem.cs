using System;
using System.IO;
using Newtonsoft.Json;

namespace Game.Core
{
    public class Problem
    {
        public long Id { get; set; }
        public Unit[] Units { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Cell[] Filled { get; set; }
        public int SourceLength { get; set; }
        public int[] SourceSeeds { get; set; }

        public static Problem FromFile(string filename)
        {
            if (!File.Exists(filename))
                throw new ArgumentException("file not found", nameof(filename));

            using (var file = File.OpenText(filename))
            {
                var serializer = new JsonSerializer();
                return (Problem) serializer.Deserialize(file, typeof (Problem));
            }
        }
    }
}