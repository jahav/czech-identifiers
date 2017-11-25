namespace Identifiers
{
    /// <summary>
    /// Parse an object to a standard string form used by the validators. Reponsibility of this class is only 
    /// parsing, not validation.
    /// </summary>
    /// <typeparam name="TInput">Type of an object that is used as an input.</typeparam>
    /// <typeparam name="TIdentifier">Type of identifier that is an output.</typeparam>
    public interface IIdentifierParser<TInput, TIdentifier>
    {
        /// <summary>
        /// Parse the input object into a sanitized standard used by the validators.
        /// </summary>
        /// <param name="input">Input object.</param>
        /// <returns>Sanitized format.</returns>
        /// <exception cref="FormatException">When the object can't be parsed to a standard form.</exception>
        string Parse(TInput input);
    }
}
