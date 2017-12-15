namespace Identifiers.Czech
{
    /// <summary>
    /// Interface for all identifiers.
    /// </summary>
    public interface IIdentifier
    {
        /// <summary>
        /// Is the identifier valid? The identifier instance may be parsable, but it might be or might not be valid, depending on some internal conditions.
        /// </summary>
        bool IsValid { get; }
    }
}
