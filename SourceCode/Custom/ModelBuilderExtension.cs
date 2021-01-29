using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SourceCode.Custom
{
    public static class ModelBuilderExtension
    {
        // We created this custom extention method for ModelBuilder to seed some predefined data to database
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // For Usign HasData Function/extension we installed below package
            // Microsoft.EntityFrameworkCore.Tools

            // Created Entry AspNetRoles Table
            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole
               {
                   Name = "Webmaster",
                   NormalizedName = "Webmaster".ToUpper()
               }
               );
            // Created Entry Currency Table
            modelBuilder.Entity<Currency>().HasData(
               new Currency
               {
                   ID = 1,
                   Symbol = "USD"
               },
               new Currency
               {
                   ID = 2,
                   Symbol = "GBP"
               },
                new Currency
                {
                    ID = 3,
                    Symbol = "EUR"
                },
                new Currency
                {
                    ID = 4,
                    Symbol = "INR"
                },
                new Currency
                {
                    ID = 5,
                    Symbol = "BDT"
                }
               );
            // Created Entry in GeneralSettings Table
            modelBuilder.Entity<General>().HasData(new General
            {
                ID = 1,
                Address = "New York, United States.",
                Email = "info@website.com",
                InvoiceFooter = "Advance Invoice & Billing | Email: info@website.com",
                Name = "Advance Invoice & Billing",
                Website="www.website.com",
                PageSize=150,
                InvoiceHeader= "Advance Invoice",
                InvoiceSubHeader = "Invoice Solutions by AI Corp.",
                InvoicePrefix="AI",
                Taxes="5",
                Phone="+2-99-45-90-88",
                Terms= "This is a digital invoice so doesn't require any signature.",
                CurrencyID=1
                

            });

            // Created Entry in EmailSettings Table
            modelBuilder.Entity<EmailSettings>().HasData(new EmailSettings
            {
                ID = 1,
              EmailPassword="",
              FromEmail="",
              Host="",
              PortNo=25,
              EnableSsl=false,
              Username="",
              Subject=""


            });

        }
    }
}
