using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Services
{
    public interface IEmailService
    {
        EmailSettings UpdateEmailSettings(EmailSettings general);

        EmailSettings GetEmailSettings();
    }
}
