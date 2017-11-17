using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess.Models
{
    [Table("PersonalDocument")]
    public class PersonalDocument : Document
    {
        public User User { get; set; }
    }
}
