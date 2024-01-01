using BlogEngineClone.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogEngineClone.Areas.Blog.Pages
{
    public class UserProfileModel : PageModel
    {
        public readonly BlogEngineCloneContext _context;

        public UserProfileModel(BlogEngineCloneContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }
    }
}
