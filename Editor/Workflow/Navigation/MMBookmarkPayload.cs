using System;
using System.Collections.Generic;

namespace MM.Inspector.Workflow.Editor
{
    [Serializable]
    public sealed class MMBookmarkPayload
    {
        public List<MMBookmarkEntry> Entries = new List<MMBookmarkEntry>();
    }
}
