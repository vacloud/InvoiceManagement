using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SourceCode.Models;

namespace SourceCode.Services
{
    // Funtions for performing Email tasks 
    public class EmailService : IEmailService
    {
        private readonly AppDBContext _Context;

        public EmailService(AppDBContext _context)
        {
            _Context = _context;
        }
        public EmailSettings GetEmailSettings()
        {
            //AsNoTracking() is used so no id tracking will heppend for this , else it will give error while updating 
            var GetData = _Context.EmailSettings.AsNoTracking().FirstOrDefault();
            return GetData; 
        }

        public EmailSettings UpdateEmailSettings(EmailSettings emailSettings)
        {
            _Context.Entry(emailSettings).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _Context.SaveChanges();
            return emailSettings;
        }
    }
}
