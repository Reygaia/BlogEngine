using BlogEngineClone.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Entity
{
    public class Notification
    {
        [Key]
        public string NotiID { get; set; }
        public string? ContentNoti { get; set; }
        public string UserID {  get; set; }
        public virtual BlogEngineCloneUser? UserEntity { get; set; }
    }
}
