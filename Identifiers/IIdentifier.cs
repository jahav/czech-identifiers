namespace Identifiers.Czech
{
    /// <summary>
    /// Interface for all identifiers.
    /// </summary>
    public interface IIdentifier
    {
        /// <summary>
        /// Is the identifier valid? The identifier must have <see cref="HasStandardFormat">standard format</see>
        /// plus it must satisfy some internal conditions for it to be valid.
        /// </summary>
        bool IsValid { get; }
    }
}
