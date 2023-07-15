namespace Todoist.BusinessLogic.ServiceResults.Base
{
    public class ServiceValueResult<TResult> : ServiceResult
    {
        public TResult? Result { get; set; }

        public ServiceValueResult() : base() { }
        public ServiceValueResult(string errorCode) : base(errorCode) { }
        public ServiceValueResult(IEnumerable<string> errorCodes) : base(errorCodes) { }

        public static new ServiceValueResult<TResult> Success(TResult result) => new ServiceValueResult<TResult> { Result = result };
        
        public static new ServiceValueResult<TResult> Error(string error) => new(error);
        public static new ServiceValueResult<TResult> Error(IEnumerable<string> errors) => new(errors);
    }
}
