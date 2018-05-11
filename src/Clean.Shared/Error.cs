// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: Error.cs
// CREATED: Mike Gardner

namespace Clean.Shared
{
    using System.Collections.Generic;
    using Interfaces;

    /// <summary>   Information about non-exception based errors. </summary>
    public sealed class Error : IError
    {
        #region Constructors / Finalizers

        /// <summary>   Default Constructor. </summary>
        public Error() { }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="message">  The message to add. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public Error(string message)
        {
            Add(message);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="messages"> The list of messages to add. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public Error(List<string> messages)
        {
            AddRange(messages);
        }

        #endregion

        #region IError Members

        #region IDisposable

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            Messages = null;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the read-only list of error messages. </summary>
        ///
        /// <value> A list of read-only error messages. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<string> Messages { get; private set; } = new List<string>();

        #endregion

        #region Implementation of IError

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds a message to the list of error messages. </summary>
        ///
        /// <param name="message">  The message to add. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Add(string message)
        {
            if (message == null) return;

            Messages.Add(message);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds a list of messages to the list of error messages. </summary>
        ///
        /// <param name="messages"> The list of messages to add. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void AddRange(List<string> messages)
        {
            if (messages == null) return;

            Messages.AddRange(messages);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Addition operator. </summary>
        ///
        /// <param name="left">     The left error. </param>
        /// <param name="right">    The right error. </param>
        ///
        /// <returns>   The result of the operation is that the errors 
        ///             in these two objects are merged into a new object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Error operator +(Error left, IError right)
        {   
            var error = new Error();

            if (left != null && right != null)
            {
                error.AddRange(left.Messages);
                error.AddRange(right.Messages);
                return error;
            }

            if (left == null && right != null)
            {
                error.AddRange(right.Messages);
                return error;
            }

            if (left != null)
            {
                error.AddRange(left.Messages);
                return error;
            }

            // When left and right are null.
            return error;
        }

        #endregion
    }
}