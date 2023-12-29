using BlogEngineClone.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Entity
{
    public class Comment
    {
        [Key]
        public string CommentID { get; set; }
        public string ContentCmt { get; set; }
        public DateTime CommentAt { get; set; }
        public string UserID { get; set; }
        public virtual BlogEngineCloneUser User { get; set; }
    }
}
