using Microsoft.AspNetCore.Http;

namespace Sadin.EmailEngine.Domain.Aggregates.EmailAggregate.ValueObjects;

public sealed class EmailAttachment : ValueObject
{
    public IFormFile Attachment { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Attachment;
    }
}