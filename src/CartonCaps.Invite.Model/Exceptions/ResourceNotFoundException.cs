namespace CartonCaps.Invite.Model.Exceptions
{
    /// <summary>
    /// Exception thrown when a resource is not found.
    /// </summary>
    public class ResourceNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException"/> class with a default error message.
        /// </summary>
        public ResourceNotFoundException() : base("Resource not found.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ResourceNotFoundException(string message) : base(message) { }
    }
}