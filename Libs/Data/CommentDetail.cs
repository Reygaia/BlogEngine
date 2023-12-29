using BlogEngineClone.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Entity
{
    [Keyless]
    public class CommentDetail
    {
        public string UserID { get; set; }
        public BlogEngineCloneUser User { get; set; }
        public string PostID { get; set; }
        public Post Post { get; set; }
        public string CmtID { get; set; }
        public Comment Comment { get; set; }

    }
}
