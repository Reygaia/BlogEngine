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
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
    public class FollowRequest
    {
        public string UserID { get; set; }
        public string TargetID { get; set; }
    }
    [Keyless]
    public class Follow
    {
        public string UserID { get; set; }
        [NotMapped]
        public FollowList? FollowList { get; set; } = new FollowList();
        // Navigation properties
        public Follow(string userId)
        {
            UserID = userId;
            FollowList = new FollowList();
        }
        public Follow()
        {

        }


    }
    public class FollowList
    {
        public List<string>? FollowingList { get; set; } = new List<string>();
        public List<string>? FollowerList { get; set; } = new List<string>();
    }
    
}
