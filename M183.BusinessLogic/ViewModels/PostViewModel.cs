using System;
using System.ComponentModel;

namespace M183.BusinessLogic.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //TODO: Decision on data type
        public string Content { get; set; }
        [DisplayName("Publish")]
        public bool IsPublished { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EditedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
