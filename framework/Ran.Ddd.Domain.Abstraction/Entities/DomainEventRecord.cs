namespace Ran.Ddd.Domain.Abstraction.Entities;

public record class DomainEventRecord(object EventData, long EventOrder);
