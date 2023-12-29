using BlogEngineClone.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Entity
{
    public class Post
    {
        [Key]
        public string postID { get; set; }
        public string Name { get; set; }
        public DateTime PostTime { get; set; }
        public string ContentPost {  get; set; }
        public string UserID { get; set; }
        public virtual BlogEngineCloneUser User {  get; set; }
    }
}
