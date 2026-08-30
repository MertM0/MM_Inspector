using System;
using System.Collections.Generic;

namespace MM.Inspector.Editor
{
    public abstract class MMTypeProcessor
    {
        public virtual int Order => 0;

        public abstract void Process(Type type, List<MMMemberSchema> members);
    }
}
