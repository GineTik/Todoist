namespace Todoist.BusinessLogic.ServiceResults.Base
{
    public class ServiceResult
    {
        public bool Successfully => ErrorCodes.Any() == false;
        public IEnumerable<string> ErrorCodes { get; set; }

        public ServiceResult()
        {
            ErrorCodes = new List<string>();
        }

        public ServiceResult(string errorCode)
        {
            ErrorCodes = new List<string> { errorCode };
        }

        public ServiceResult(IEnumerable<string> errorCodes)
        {
            ErrorCodes = errorCodes;
        }

        public static ServiceResult Success => new ServiceResult();
        public static ServiceResult Error(string errorCode) => new ServiceResult(errorCode);
        public static ServiceResult Error(IEnumerable<string> errorCodes) => new ServiceResult(errorCodes);
    }
}
