using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracedFramework
{
    [Serializable]
    public class GameData
    {
        public Dictionary<string, SimpleDataContainer> Containers = new Dictionary<string, SimpleDataContainer>();
        public Dictionary<string, Yarn.Value> Variables = new Dictionary<string, Yarn.Value>();

    }
}