namespace Grammar.PluginBase.Token.Contracts
{
    /// <summary>
    /// Represent a token result after a finding is made on the source to analyze
    /// </summary>
    public interface ITokenResultBase
    {
        /// <summary>
        /// The position of the END of the current result
        /// </summary>
        ITokenParsingPosition Position { get; }

        TokenState State { get; set; }

        bool Failed { get; set; }
    }

    public enum TokenState
    {
        Mandatory,
        Optional
    }
}