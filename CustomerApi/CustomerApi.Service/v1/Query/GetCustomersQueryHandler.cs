using CustomerApi.Domain.Entities;
using CustomerApi.Interface.Repository.v1;
using MediatR;

namespace CustomerApi.Service.v1.Query
{
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Customer>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            return _customerRepository.GetAll().ToList();
        }
    }
}
