using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SourceCode.Models;

namespace SourceCode.Services
{
    // Funtions for performing different tasks on Currency
    public class CurrencyService : ICurrencyService
    {
        private readonly AppDBContext _Context;

        public CurrencyService(AppDBContext _context)
        {
            _Context = _context;
        }

        public Currency AddCurrency(Currency currency)
        {
          var data=_Context.Currency.Add(currency);
            _Context.SaveChanges();
            return currency;
        }

        public List<SelectListItem> AllCurrencies()
        {
            // .AsNoTracking() is used because when using edit (UpdateCurrency) & fill (AllCurrencies())
            // it doesn't know which ID to track and gives exception
            return _Context.Currency.AsNoTracking().Select( x=> new SelectListItem {Text=x.Symbol, Value = x.ID.ToString()}).ToList();
        }
        public IEnumerable<Currency> AllCurrenciesList()
        {
            return _Context.Currency.AsNoTracking().OrderByDescending(x=>x.ID).ToList();
        }
        public Currency DeleteCurrency(Currency currency)
        {
            _Context.Currency.Remove(currency);
            _Context.SaveChanges();
            return currency;
        }

        public Currency GetCurrencyByID(int ID)
        {
           return  _Context.Currency.Find(ID);
        }

        public Currency UpdateCurrency(Currency currency)
        {
            _Context.Entry(currency).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _Context.SaveChanges();

            return currency;
        }
    }
}
