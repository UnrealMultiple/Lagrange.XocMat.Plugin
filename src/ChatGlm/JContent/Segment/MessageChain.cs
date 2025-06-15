using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGlm.JContent.Segment;

public class MessageChain : List<BaseSegment>
{
    public List<T> GetEntity<T>() where T : BaseSegment
    {
        return [.. this.Where(s => s is T).Select(s => (T)s)];
    }
}
