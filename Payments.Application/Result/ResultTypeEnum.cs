namespace Payments.Application.Result
{
    public enum ResultType
    {
        Ok = 200,
        Unexpected = 502,
        NotFound = 404,
        Unauthorized = 401,
        Invalid = 500
    }
}
