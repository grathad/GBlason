namespace Grammar.PluginBase.Token.Contracts
{
    public interface ITokenResult : ITokenResultBase
    {
        /// <summary>
        /// The resulting unique token
        /// </summary>
        IToken ResultToken { get; }
    }
}