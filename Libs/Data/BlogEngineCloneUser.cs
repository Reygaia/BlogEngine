using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Libs.Entity;
using Microsoft.AspNetCore.Identity;

namespace BlogEngineClone.Areas.Identity.Data;

// Add profile data for application users by adding properties to the BlogEngineCloneUser class
public class BlogEngineCloneUser : IdentityUser
{
    [Key]
    public string UserID { get; set; }
    public string name { get; set; }
    public virtual List<Post>? Post { get; set; }
    public virtual List<Comment>? Comment { get; set; }

}

