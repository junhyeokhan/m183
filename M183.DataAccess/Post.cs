using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M183.DataAccess
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EditedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<PostStatus> PostStatuses { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
