using System.Collections.Generic;

namespace jigsaw.Engine
{
    class Membership
    {
        public Dictionary<string, string> Values { get; private set; }

        public Membership()
        {
            Values = new Dictionary<string, string>();
        }
    }
}
