using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Services
{
   public interface IGeneralService
    {
        General UpdateGeneralSettings(General general);

        General GetGeneralSettings();
    }
}
