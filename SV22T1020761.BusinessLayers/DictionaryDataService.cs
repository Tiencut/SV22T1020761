using SV22T1020761.DataLayers.SQLServer.Dictionary;
using SV22T1020761.Models.Common;

namespace SV22T1020761.BusinessLayers
{
    /// <summary>
    /// Service cung c?p dữ liệu cho c�c b?ndữ liệu?u chu?n (dictionary) nh� Provinces
    /// </summary>
    public static class DictionaryDataService
    {
        private static ProvinceRepository _provinceRepo;

        static DictionaryDataService()
        {
            _provinceRepo = new ProvinceRepository(Configuration.ConnectionString);
        }

        public static PagedResult<string> ListProvinces(PaginationSearchInput input)
        {
            return _provinceRepo.ListAsync(input).GetAwaiter().GetResult();
        }
    }
}
