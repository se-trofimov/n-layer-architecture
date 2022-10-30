using System.Diagnostics.CodeAnalysis;

namespace API;
public class LinkWrapper<T>: LinkResourceBase
{
    public T Value { get; set; }  
 
    public LinkWrapper(T value, List<Link> links)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Links = links ?? throw new ArgumentNullException(nameof(links));
    }
}
