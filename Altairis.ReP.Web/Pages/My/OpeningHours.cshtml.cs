using Altairis.ReP.Data;
using Olbrasoft.ReP.Business;

namespace Altairis.ReP.Web.Pages.My;
public class OpeningHoursModel : PageModel {
    private readonly IOpeningHoursService _service;

    public OpeningHoursModel(IOpeningHoursService hoursProvider) {
        _service = hoursProvider ?? throw new ArgumentNullException(nameof(hoursProvider));
    }

    public IEnumerable<OpeningHoursInfo> OpeningHours
    {
        get
        {
            return _service.GetOpeningHours(0, 14);
        }
    }
}
