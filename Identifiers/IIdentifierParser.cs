namespace Identifiers.Czech
{
    /// <summary>
    /// Parse an object to a standard string form used by the validators. Reponsibility of this class is only 
    /// parsing, not validation.
    /// </summary>
    /// <typeparam name="TInput">Type of an object that is used as an input.</typeparam>
    /// <typeparam name="TIdentifier">Type of identifier that is an output.</typeparam>
    public interface IIdentifierParser<TInput, TIdentifier>
        where TIdentifier : IIdentifier
    {
        /// <summary>
        /// Parse the input object into an identifier.
        /// </summary>
        /// <param name="input">Input object.</param>
        /// <returns>Parsed identifier.</returns>
        /// <exception cref="ArgumentNullException">When the <paramref name="input"/> is null.</exception>
        /// <exception cref="FormatException">When the input is not in standard format.</exception>
        TIdentifier Parse(TInput input);
    }
}
