using System;

namespace Identifiers.Czech
{
    /// <summary>
    /// A result of an attept to <see cref="IPattern{TValue}.Parse(string)">parse</see> a string into a value.
    /// The parsing either succeeded or failed. 
    /// * If it succeeded, then <see cref="Success"/> is set to <c>true</c> and the <see cref="Value"/> contains parsed value.
    /// * If it failed, then <see cref="Success"/> is set to <c>fale</c> and the <see cref="Exception"/> contains the exception that caused the parsing to fail.
    /// </summary>
    /// <typeparam name="TValue">Type of parsed value.</typeparam>
    public sealed class ParseResult<TValue>
    {
        private TValue value;
        private Exception exception;

        private ParseResult(TValue value, Exception exception)
        {
            this.value = value;
            this.exception = exception;
        }

        /// <summary>
        /// Indicate if the parsing was successfull.
        /// </summary>
        public bool Success => exception == null;

        /// <summary>
        /// Get value, if parsing was successfull, otherwise throw an exception indicating parsing failure.
        /// </summary>
        public TValue Value => GetValueOrThrow();

        /// <summary>
        /// Get an exception that caused the parsing to fail.
        /// </summary>
        /// <exception cref="InvalidOperationException">If parsing succeeded, then this property throws an exception.</exception>
        public Exception Exception
        {
            get
            {
                if (exception == null)
                {
                    throw new InvalidOperationException("Parsing was successfull.");
                }

                return exception;
            }
        }

        /// <summary>
        /// Get value, if parsing was successfull, otherwise throw the exception that caused parsing failure.
        /// </summary>
        /// <returns>Parsed value, if parsing <see cref="Success">succeeded</see>.</returns>
        public TValue GetValueOrThrow()
        {
            if (exception == null)
            {
                return value;
            }

            throw exception;
        }

        /// <summary>
        /// Create a new parsed result indicating a success.
        /// </summary>
        /// <param name="value">Successfully parsed value.</param>
        /// <returns>Created parsing result.</returns>
        public static ParseResult<TValue> ForValue(TValue value) => new ParseResult<TValue>(value, null);

        /// <summary>
        /// Create a new parsed result indicating an error.
        /// </summary>
        /// <param name="exception">The exception indicating a parsing failure.</param>
        /// <returns>Created parsing result.</returns>
        public static ParseResult<TValue> ForException(Exception exception) => new ParseResult<TValue>(default(TValue), exception);
    }
}
