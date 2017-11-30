using System;

namespace Identifiers.Czech
{
    /// <summary>
    /// Result of parsing.
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
        /// <exception cref="InvalidOperationException">If parsing succeeded.</exception>
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
        /// Get value, if parsing was successfull, otherwise throw an exception indicating parsing failure.
        /// </summary>
        /// <returns>Parsed value.</returns>
        public TValue GetValueOrThrow()
        {
            if (exception == null)
            {
                return value;
            }

            throw exception;
        }

        /// <summary>
        /// Create a new parsed result indicatin a success.
        /// </summary>
        /// <param name="value">Successfully parsed value.</param>
        /// <returns>Parsing result.</returns>
        public static ParseResult<TValue> ForValue(TValue value) => new ParseResult<TValue>(value, null);

        /// <summary>
        /// Create a new parsed result indicating error.
        /// </summary>
        /// <param name="exception">The exception indicating a parsing failure.</param>
        /// <returns>Parsing result.</returns>
        public static ParseResult<TValue> ForException(Exception exception) => new ParseResult<TValue>(default(TValue), exception);
    }
}
