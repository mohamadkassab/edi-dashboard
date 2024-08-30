using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EDI.ViewComponents
{
    public class FixedElementsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string view)
        {
            if(!view.IsNullOrEmpty())
            {
                return View(view);
            }
            else
            {
                return View();
            }
        }
    }
}
