namespace SeedApp.Common.Exception
{
    public enum MmpApiErrorCodes
    {
        GenericError,
        ConnectivityLost,
        ServerUnreachable,
        Unauthorized,
        InvalidRequest,
        InvalidCurrentPassword
    }
}