using CleanModularTemplate.Shared.Domain;

namespace CleanModularTemplate.Accounts.Domain;

public class Account : EntityWithDomainEvents<Guid>, IAuditable
{
  public Account(Guid userId, Guid createdBy)
  {
	CreatedAt = DateTimeOffset.UtcNow;
	UpdatedAt = DateTimeOffset.UtcNow;
	CreatedBy = createdBy;
	UpdatedBy = createdBy;
	UserId = userId;
  }

  public string FullName { get; set; } = string.Empty;
  public Guid UserId { get; set; }
  public DateTimeOffset CreatedAt { get; protected set; }
  public Guid CreatedBy { get; protected set; }
  public DateTimeOffset UpdatedAt { get; protected set; }
  public Guid UpdatedBy { get; protected set; }


#pragma warning disable CS8618 // Enable EF Core
  protected Account()
#pragma warning restore CS8618
  {
  }

}
