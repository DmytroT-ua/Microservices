﻿using CustomerApi.Domain.Entities;
using CustomerApi.Interface.Repository.v1;
using CustomerApi.Interface.Sender.v1;
using MediatR;

namespace CustomerApi.Service.v1.Command
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Customer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerUpdateSender _customerUpdateSender;

        public UpdateCustomerCommandHandler(ICustomerUpdateSender customerUpdateSender, ICustomerRepository customerRepository)
        {
            _customerUpdateSender = customerUpdateSender;
            _customerRepository = customerRepository;
        }

        public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.UpdateAsync(request.Customer);

            _customerUpdateSender.SendCustomer(customer);

            return customer;
        }
    }
}
