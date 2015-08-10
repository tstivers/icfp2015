using System.Collections.Generic;
using Game.Core;

namespace Game.Controllers
{
    public class LockResult
    {        
        public int LinesRemoved { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
        public int MinDistanceFromCenter { get; set; }
        public int NumberOfHoles { get; set; }

        public List<List<Direction>> Directions { get; set; } = new List<List<Direction>>();
    }
}