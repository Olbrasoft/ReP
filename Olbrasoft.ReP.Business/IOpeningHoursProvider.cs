using Altairis.ReP.Data;

namespace Olbrasoft.ReP.Business;
public interface IOpeningHoursProvider
{
    Task<OpeningHoursInfo> GetOpeningHours(DateTime date);
    IEnumerable<OpeningHoursInfo> GetOpeningHours(DateTime dateFrom, DateTime dateTo);
    Task<OpeningHoursInfo> GetOpeningHours(int dayOffset);
    IEnumerable<OpeningHoursInfo> GetOpeningHours(int dayOffsetFrom, int dayOffsetTo);
    IEnumerable<OpeningHoursInfo> GetStandardOpeningHours();
}