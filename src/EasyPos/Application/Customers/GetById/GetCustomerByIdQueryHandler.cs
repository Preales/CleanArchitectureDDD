using Application.Customers.Common;
using Domain.Customers;
using Domain.DomainErrors;

namespace Application.Customers.GetById;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ErrorOr<CustomerResponse>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public async Task<ErrorOr<CustomerResponse>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        if (await _customerRepository.GetByIdAsync(new CustomerId(query.Id)) is not Customer customer)
            return Errors.Customer.CustomerNoFoundById;

        return new CustomerResponse(
            customer.Id.Value,
            customer.FullName,
            customer.Email,
            customer.PhoneNumber.Value,
            new AddressResponse(customer.Address.Country,
            customer.Address.Line1,
            customer.Address.Line2,
            customer.Address.City,
            customer.Address.State,
            customer.Address.ZipCode),
            customer.Active);
    }
}