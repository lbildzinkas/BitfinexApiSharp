namespace BitfinexClientSharp.WSocket.Adapters
{
    public interface IResponseValidator
    {
        bool IsHeaderMsg(string msg);
    }
}