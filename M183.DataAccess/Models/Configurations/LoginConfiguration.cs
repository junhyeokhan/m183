using System.ComponentModel.DataAnnotations.Schema;

namespace M183.DataAccess.Models.Configurations
{
    [Table("LoginConfigurations")]
    public class LoginConfiguration : UserConfiguration
    {
        public int LoginMethod { get; set; }
    }
}