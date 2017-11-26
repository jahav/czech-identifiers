namespace Identifiers.Czech
{
    /// <summary>
    /// Interface for all identifiers.
    /// </summary>
    public interface IIdentifier
    {
        /// <summary>
        /// Does the string representation of the identifier match to standard format of the identifier?
        /// If it doesn't match, the identifier is not valid and basically all properties for parts of the identifer will return null.
        /// </summary>
        bool HasStandardFormat { get; }

        /// <summary>
        /// Is the identifier valid? The identifier must have <see cref="HasStandardFormat">standard format</see>
        /// plus it must satisfy some internal conditions for it to be valid.
        /// </summary>
        bool IsValid { get; }
    }
}
