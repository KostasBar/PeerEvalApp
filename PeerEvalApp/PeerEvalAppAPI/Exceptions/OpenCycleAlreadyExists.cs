namespace PeerEvalAppAPI.Exceptions
{
    public class OpenCycleAlreadyExists: AppException
    {
        private static readonly string DEFAULT_CODE = "OpenCycleExists";

        public OpenCycleAlreadyExists(string code, string message)
            : base(code + DEFAULT_CODE, message)
        {
        }
    }

}
