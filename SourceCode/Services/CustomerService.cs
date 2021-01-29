using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SourceCode.Models;

namespace SourceCode.Services
{
    // Funtions for performing different tasks on Customers
    
    public class CustomerService : ICustomerService
    {
        private readonly AppDBContext _Context;

        public CustomerService(AppDBContext _context)
        {
            _Context = _context;
        }

           
        public Customer AddCustomer(Customer customer)
        {
            var data = _Context.Customer.Add(customer);
            _Context.SaveChanges();
            return customer;
        }


        public IEnumerable<Customer> AllCustomerList()
        {
            return _Context.Customer.AsNoTracking().OrderByDescending(x => x.ID).ToList();
        }

        public List<SelectListItem> AllCustomerForDdl()
        {
            // make other like this
            var Data = _Context.Customer.AsNoTracking().OrderByDescending(x => x.ID).ToList();
          
                return Data.Select(x => new SelectListItem { Text = (x.Name + " ("+  x.Email + ")").ToString(), Value = x.ID.ToString() }).ToList();

          
        }

        public Customer DeleteCustomer(Customer customer)
        {
            _Context.Customer.Remove(customer);
            _Context.SaveChanges();
            return customer;
        }

        public Customer GetCustomerByID(int ID)
        {
            return _Context.Customer.Find(ID);
        }

        public Customer UpdateCustomer(Customer customer)
        {
            _Context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _Context.SaveChanges();

            return customer;
        }

        public int TotalCustomers()
        {
            return _Context.Customer.ToList().Count();
        }

        public IEnumerable<Customer> Top5CustomerList()
        {
            return _Context.Customer.AsNoTracking().OrderByDescending(x => x.ID).ToList().Take(5);
        }
    }
}
