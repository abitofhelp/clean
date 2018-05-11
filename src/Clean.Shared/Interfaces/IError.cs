// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: IError.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>   Information about non-exception based errors. </summary>
    public interface IError : IDisposable
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the read-only list of error messages. </summary>
        ///
        /// <value> A list of read-only error messages. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        List<string> Messages { get; }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds a message to the list of error messages. </summary>
        ///
        /// <param name="message">  The message to add. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        void Add(string message);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds a list of messages to the list of error messages. </summary>
        ///
        /// <param name="messages"> The list of messages to add. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        void AddRange(List<string> messages);

        #endregion
    }
}