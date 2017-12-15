namespace Identifiers.Czech
{
    /// <summary>
    /// A string pattern of a type <typeparamref name="TValue"/>, used to 
    /// 
    /// * parse a string and create value from the string.
    /// * create a string from the value.
    /// 
    /// The implementation of the interface ensure that both parsing and formatting are using same string pattern, although parsing can be more generous with accepted input.
    /// </summary>
    /// <typeparam name="TValue">Type that is being parsed or formatted using a pattern.</typeparam>
    public interface IPattern<TValue>
    {
        /// <summary>
        /// Format the <paramref name="value"/> to a string according to the pattern.
        /// </summary>
        /// <param name="value">Value to format.</param>
        /// <returns>Formatted string.</returns>
        string Format(TValue value);

        /// <summary>
        /// Parse a string into the value. It doesn't thow exceptions, if an error 
        /// happens during parsing, return unsuccessfull <see cref="ParseResult{T}"/>.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        /// <returns>Parsing result.</returns>
        /// <exception cref="ArgumentNotNull">If <paramref name="text"/> is <c>null</c>.</exception>
        ParseResult<TValue> Parse(string text);
    }
}
