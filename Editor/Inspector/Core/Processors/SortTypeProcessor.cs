using System;
using System.Collections.Generic;
using System.Linq;

namespace MM.Inspector.Editor
{
    internal sealed class SortTypeProcessor : MMTypeProcessor
    {
        public override int Order => int.MaxValue;

        public override void Process(Type type, List<MMMemberSchema> members)
        {
            List<MMMemberSchema> sorted = members.OrderBy(member => member.Order).ToList();
            members.Clear();
            members.AddRange(sorted);
        }
    }
}
