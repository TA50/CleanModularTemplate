namespace CleanModularTemplate.Shared.Infrastructure;

public class InfrastructureException
	: Exception
{
  public InfrastructureException(string message) : base(message)
  {
  }

  public InfrastructureException()
  {
  }

  public InfrastructureException(string message, Exception innerException) : base(message, innerException)
  {
  }
}

public class EntityNotFoundException
	: InfrastructureException
{
  public EntityNotFoundException(string id, string entityName) : base($"Entity {entityName} with id {id} does not exists")
  {
  }
  public EntityNotFoundException()
  {
  }

  public EntityNotFoundException(string message) : base(message)
  {
  }

  public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
  {
  }
}

public class DatabaseOperationException
	: InfrastructureException
{

  public DatabaseOperationException()
  {
  }

  public DatabaseOperationException(string message) : base(message)
  {
  }

  public DatabaseOperationException(string message, Exception innerException) : base(message, innerException)
  {
  }
}
