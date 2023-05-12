namespace Sadin.EmailEngine.Domain.Aggregates.EmailAggregate.ValueObjects;

public sealed class EmailAttachment : ValueObject
{
    public byte[] Attachment { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        throw new NotImplementedException();
    }
}