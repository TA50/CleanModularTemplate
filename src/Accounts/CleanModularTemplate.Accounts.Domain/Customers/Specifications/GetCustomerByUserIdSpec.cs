using Ardalis.Specification;
using CleanModularTemplate.Accounts.Domain.Customers.Entities;

namespace CleanModularTemplate.Accounts.Domain.Customers.Specifications;

public class GetCustomerByUserIdSpec : Specification<Customer>
{
  public GetCustomerByUserIdSpec(Guid userId)
  {
	Query.Where(c => c.UserId == userId);
  }
}
