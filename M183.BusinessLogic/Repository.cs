using M183.BusinessLogic.ViewModels;
using M183.DataAccess;
using M183.DataAccess.Models;
using M183.DataAccess.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.BusinessLogic
{
    public class Repository
    {
        private static DatabaseContext db = new DatabaseContext();

        public string GetConfiguration(string key)
        {
            GlobalConfiguration globalConfiguration = db.GlobalConfigurations
                    .Where(gc => gc.Key.ToLower() == key.ToLower())
                    .FirstOrDefault();

            if (globalConfiguration == null)
            {
                globalConfiguration = new GlobalConfiguration()
                {
                    Key = key,
                    Value = string.Empty,
                };
                db.GlobalConfigurations.Add(globalConfiguration);
                db.SaveChanges();
            }

            return globalConfiguration.Value;
        }
    }
}
