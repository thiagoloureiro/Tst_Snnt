using WebAPI.Filters;

namespace WebAPI.Requests
{
    public class CSVFileExample : MultipartRequestOperationFilter.IExamplesProvider
    {
        public object GetExamples()
        {
            return new CSVFile()
            {
                userId = 1
            };
        }
    }
}