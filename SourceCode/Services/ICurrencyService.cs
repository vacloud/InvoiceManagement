using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Services
{
    public interface ICurrencyService
    {
        List<SelectListItem> AllCurrencies();

        Currency GetCurrencyByID(int ID);

        Currency DeleteCurrency(Currency currency);

        Currency UpdateCurrency(Currency currency);

        Currency AddCurrency(Currency currency);

        IEnumerable<Currency> AllCurrenciesList();

    }
}
