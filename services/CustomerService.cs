using POS_qu.DTO;
using POS_qu.Models;
using POS_qu.Repositories;
using System.Collections.Generic;

namespace POS_qu.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _repo;

        public CustomerService()
        {
            _repo = new CustomerRepository();
        }

        public List<CustomerDto> GetAll()
        {
            return _repo.GetAll();
        }

        public void Insert(string name)
        {
            var session = SessionUser.GetCurrentUser();
            _repo.Insert(name, session.UserId);
        }
    }
}
