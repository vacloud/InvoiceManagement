using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Services
{
    public interface ICustomerService
    {
        Customer GetCustomerByID(int ID);

        Customer DeleteCustomer(Customer customer);

        Customer UpdateCustomer(Customer customer);

        Customer AddCustomer(Customer customer);

        IEnumerable<Customer> AllCustomerList();

        List<SelectListItem> AllCustomerForDdl();

        int TotalCustomers();

        IEnumerable<Customer> Top5CustomerList();

    }
}
