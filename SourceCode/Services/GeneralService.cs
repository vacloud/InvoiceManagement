using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SourceCode.Models;

namespace SourceCode.Services
{
    // General Service for Update Settings of Webmaster
    public class GeneralService : IGeneralService
    {
        private readonly AppDBContext _Context;

        public GeneralService(AppDBContext _context)
        {
            _Context = _context;
        }

        // Fetch Data Saved from General Settings like Invoice Header, Sub Header, etc. 
        public General GetGeneralSettings()
        {
            //AsNoTracking() is used so no id tracking will heppend for this , else it will give error while updating 
            var GetData = _Context.GeneralSettings.AsNoTracking().FirstOrDefault();
                return GetData;
            
        }

        // Update Data like Invoice Header, Sub Header, etc. 
        public General UpdateGeneralSettings(General general)
        {
            _Context.Entry(general).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _Context.SaveChanges();

            return general;
        }
    }
}
