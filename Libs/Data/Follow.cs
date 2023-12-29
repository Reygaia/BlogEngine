using BlogEngineClone.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Entity
{
    public class Follow
    {
        [Key]
        public string UserID { get; set; }

        public string FollowerID { get; set; }
        public string FollowingID { get; set; }

        // Navigation properties
        public virtual BlogEngineCloneUser Follower { get; set; }
        public virtual BlogEngineCloneUser Following { get; set; }

    }
}
