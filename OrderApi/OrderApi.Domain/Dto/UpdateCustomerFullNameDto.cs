namespace OrderApi.Domain.Dto
{
    public class UpdateCustomerFullNameDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}