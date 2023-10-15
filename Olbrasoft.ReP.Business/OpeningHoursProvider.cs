using System.Globalization;
using Altairis.ReP.Data;
using Altairis.Services.DateProvider;
using Microsoft.Extensions.Options;

namespace Olbrasoft.ReP.Business;

public class OpeningHoursProvider : IOpeningHoursProvider
{
    private readonly IOptions<AppSettings> optionsAccessor;
    private readonly IDateProvider dateProvider;
    private readonly IOpeningHoursChangeService _service;

    public OpeningHoursProvider(IOptions<AppSettings> optionsAccessor, IDateProvider dateProvider, IOpeningHoursChangeService service)
    {
        this.optionsAccessor = optionsAccessor ?? throw new ArgumentNullException(nameof(optionsAccessor));
        this.dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));
        _service = service ?? throw new ArgumentNullException(nameof(service));

    }

    public async Task<OpeningHoursInfo> GetOpeningHours(int dayOffset) => await GetOpeningHours(dateProvider.Today.AddDays(dayOffset));

    public IEnumerable<OpeningHoursInfo> GetOpeningHours(int dayOffsetFrom, int dayOffsetTo) => GetOpeningHours(dateProvider.Today.AddDays(dayOffsetFrom), dateProvider.Today.AddDays(dayOffsetTo));

    public async Task<OpeningHoursInfo> GetOpeningHours(DateTime date)
    {
        date = date.Date;

        var ohch = await _service.GetOpeningHoursChangeOrNullByAsync(date);
        return ohch == null
            ? GetStandardOpeningHours(date)
            : new OpeningHoursInfo
            {
                Date = date,

                IsException = true,
                OpeningTime = ohch.OpeningTime,
                ClosingTime = ohch.ClosingTime
            };
    }

    public IEnumerable<OpeningHoursInfo> GetOpeningHours(DateTime dateFrom, DateTime dateTo)
    {
        var date = dateFrom.Date;

        var ohchs = _service.GetOpeningHoursChangesBetween(dateFrom, dateTo).GetAwaiter().GetResult();

        while (date <= dateTo.Date)
        {
            var ohch = ohchs.SingleOrDefault(x => x.Date == date);
            yield return ohch == null
               ? GetStandardOpeningHours(date)
               : new OpeningHoursInfo
               {
                   Date = date,
                   IsException = true,
                   OpeningTime = ohch.OpeningTime,
                   ClosingTime = ohch.ClosingTime
               };
            date = date.AddDays(1);
        }
    }

    public IEnumerable<OpeningHoursInfo> GetStandardOpeningHours()
    {
        var dt = DateTime.Today;
        while (dt.DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) dt = dt.AddDays(1);
        for (var i = 0; i < 7; i++)
        {
            yield return GetStandardOpeningHours(dt);
            dt = dt.AddDays(1);
        }
    }

    private OpeningHoursInfo GetStandardOpeningHours(DateTime date)
    {
        var value = optionsAccessor.Value.OpeningHours.FirstOrDefault(x => x.DayOfWeek == date.DayOfWeek);
        return value == null
            ? new OpeningHoursInfo { Date = date.Date }
            : new OpeningHoursInfo { Date = date.Date, OpeningTime = value.OpeningTime, ClosingTime = value.ClosingTime, IsException = false };
    }

}
