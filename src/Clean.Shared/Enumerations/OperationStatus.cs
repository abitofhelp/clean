// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: OperationStatus.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Enumerations
{
    using System.ComponentModel;

    // To get the description for a particular enumeration: i.e. OperationStatus.NotAuthenticated.GetDescription()

    /// <summary>   OperationStatus indicates the success or error of an operation. </summary>
    public enum OperationStatus
    {
/// <summary>   . </summary>
        [Description("Ok")]
        Ok = 200,

/// <summary>   . </summary>
        [Description("Created")]
        Created = 201,

/// <summary>   . </summary>
        [Description("NoContent")]
        NoContent = 204,

/// <summary>   . </summary>
        [Description("Found")]
        Found = 302,

/// <summary>   . </summary>
        [Description("BadRequest")]
        BadRequest = 400,

/// <summary>   . </summary>
        [Description("NotAuthenticated")]
        NotAuthenticated = 401,

/// <summary>   . </summary>
        [Description("NotAuthorized")]
        NotAuthorized = 403,

/// <summary>   . </summary>
        [Description("NotFound")]
        NotFound = 404,

/// <summary>   . </summary>
        [Description("InternalError")]
        InternalError = 500
    }
}