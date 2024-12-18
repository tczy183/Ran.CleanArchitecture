namespace Ran.Ddd.Domain.Abstractions.Entities;

public record class DomainEventRecord(object EventData, long EventOrder);
