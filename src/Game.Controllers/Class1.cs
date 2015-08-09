using System.Collections.Generic;
using Game.Core;

namespace Game.Controllers
{
    public class LockResult
    {        
        public List<List<Direction>> Directions { get; set; } = new List<List<Direction>>();
    }
}