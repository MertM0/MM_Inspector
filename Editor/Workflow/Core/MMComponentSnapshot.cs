using System;
using System.Collections.Generic;

namespace MM.Inspector.Workflow.Editor
{
    [Serializable]
    public sealed class MMComponentSnapshot
    {
        public string Id;
        public string Json;
        public List<string> ReferencePaths = new List<string>();
        public List<string> ReferenceIds = new List<string>();
    }
}
