// SOLUTION: Clean
// PROJECT: Clean.Domain
// FILE: Motorcycle.cs
// CREATED: Mike Gardner

// Namespace Entities contains the domain entities.
namespace Clean.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Shared;
    using Shared.Extensions;
    using Shared.Interfaces;

    /// <summary>   A motorcycle. This class cannot be inherited. </summary>
    public sealed class Motorcycle : IEntity, IEquatable<Motorcycle>
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets a value indicating whether this object has been soft-deleted. </summary>
        ///
        /// <value> True if this object has been soft-deleted, false if not. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool IsDeleted { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the make, which is the manufacturer of the motorcycle. </summary>
        ///
        /// <value> The make. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Make { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets the model, which is the particular version of a motorcycle from a manufacturer.
        /// </summary>
        ///
        /// <value> The model. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Model { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the vehicle identification number ("VIN"). </summary>
        ///
        /// <value> The vin. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Vin { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the year when the motorcycle was manufactured. </summary>
        ///
        /// <value> The year. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public uint Year { get; set; }

        #endregion

        #region IEntity Members

        #region IDisposable

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose() { }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the Date/Time in UTC when the entity was created. </summary>
        ///
        /// <value> The created UTC. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DateTime? CreatedUtc { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the primary key identifier. </summary>
        ///
        /// <value> The identifier. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public long Id { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the Date/Time in UTC when the entity was modified. </summary>
        ///
        /// <value> The modified UTC. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DateTime? ModifiedUtc { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the identifier of the tenant for multitenant applications. </summary>
        ///
        /// <value> The identifier of the tenant. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public long TenantId { get; set; }

        #endregion

        #region IEquatable<Motorcycle> Members

        #region Equality members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        ///
        /// <param name="other">    An object to compare with this object. </param>
        ///
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter;
        /// otherwise, false.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Equals(Motorcycle other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Id == other.Id &&
                   TenantId == other.TenantId &&
                   IsDeleted == other.IsDeleted &&
                   string.Equals(Make, other.Make, StringComparison.CurrentCulture) &&
                   string.Equals(Model, other.Model, StringComparison.CurrentCulture) &&
                   Year == other.Year &&
                   string.Equals(Vin, other.Vin, StringComparison.CurrentCulture) &&
                   CreatedUtc.Equals(other.CreatedUtc) &&
                   ModifiedUtc.Equals(other.ModifiedUtc);
        }

        #endregion

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Validate verifies that all of the fields in a Motorcycle instance meet the requirements.
        /// </summary>
        ///
        /// <returns>   Null on success, otherwise an Error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IError Validate()
        {
            return Validate(Id, TenantId, Make, Model, Year, Vin);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Validate verifies that all of the fields in a Motorcycle instance meet the requirements.
        /// </summary>
        ///
        /// <returns>   Null on success, otherwise an Error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IError Validate(long id, long tenantId, string make, string model, uint year, string vin)
        {
            var error = new Error();

            error += ValidateNonIdFields(make, model, year, vin);

            // Is the primary key valid?
            if (Id < 0) error.Add("The Id cannot be a negative value.");

            // Is the tenant identifier valid?
            if (TenantId < 0) error.Add("The TenantId cannot be a negative value.");

            return error.Messages.Count > 0
                ? error
                : null;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Verifies that all of the non-id fields have valid data.  </summary>
        ///
        /// <param name="make">     The make. </param>
        /// <param name="model">    The model. </param>
        /// <param name="year">     The year. </param>
        /// <param name="vin">      The vin. </param>        
        /// 
        /// <returns>   Null on success, otherwise an Error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static IError ValidateNonIdFields(string make, string model, uint year, string vin)
        {
            var error = new Error();

            // Is the manufacturer valid?
            error += IsInvalidMake(make);

            // Is the model valid?
            error += IsInvalidModel(model);

            // Is the year valid?
            error += IsInvalidYear(year);

            // Is the VIN valid?
            error += IsInvalidVin(vin);

            return error.Messages.Count > 0
                ? error
                : null;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// IsInvalidMake verifies that a motorcycle is from an valid manufacturer.
        /// </summary>
        ///
        /// <param name="make"> The make is the same as the manufacturer's name. </param>
        ///
        /// <returns>   Returns null if the make is a valid manufacturer, otherwise an Error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static IError IsInvalidMake(string make)
        {
            var error = new Error();

            if (string.IsNullOrEmpty(make)) error.Add("A make cannot be empty.");

            if (make?.Length > 20) error.Add("A make cannot contain more than 20 characters.");

            var invalidMake = new ReadOnlyCollection<string>(new List<string>
            {
                "Ford"
            });
            if (invalidMake.Any(x => x.Equals(make, StringComparison.CurrentCultureIgnoreCase)))
            {
                error.Add($"Make '{make}' is not a valid motorcycle manufacturer.");
            }

            return error.Messages.Count > 0
                ? error
                : null;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// IsInvalidModel verifies that a motorcycle's model is valid.
        /// </summary>
        ///
        /// <param name="model"> The model of the motorcycle </param>
        ///
        /// <returns>   Returns null if the model is valid, otherwise an Error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static IError IsInvalidModel(string model)
        {
            var error = new Error();

            if (string.IsNullOrEmpty(model)) error.Add("A model cannot be empty.");
            if (model?.Length > 20) error.Add("A model's name cannot be more than 20 characters.");

            return error.Messages.Count > 0
                ? error
                : null;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// IsInvalidYear verifies that a motorcycle's year is valid.
        /// </summary>
        ///
        /// <param name="year"> The year when the motorcycle was created.</param>
        ///
        /// <returns>   Returns null if the year is valid, otherwise an Error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static IError IsInvalidYear(uint year)
        {
            var error = new Error();

            if (year < 1999) error.Add("A year cannot be less than 1999.");
            if (year > 2020) error.Add("A year cannot be more than 2020.");

            return error.Messages.Count > 0
                ? error
                : null;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// IsInvalidVin verifies that a motorcycle's vin.
        /// </summary>
        ///
        /// <param name="vin"> The vehicle identification number ("VIN") </param>
        ///
        /// <returns>   Returns null if the vin is valid, otherwise an Error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static IError IsInvalidVin(string vin)
        {
            var error = new Error();

            if (vin.Length != 17) error.Add($"A VIN requires 17 characters, but the provided value had {vin.Length} characters.");
            if (vin.Length > 17) error.Add("A VIN cannot be more than 17 characters.");
            if (vin.Length < 17) error.Add("A VIN cannot be less than 17 characters.");
            if (vin.Length > 20) error.Add("A manufacturer's name cannot be more than 20 characters.");

            return error.Messages.Count > 0
                ? error
                : null;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   This factory creates a Motorcycle instance. </summary>
        ///
        /// <param name="make">     The make. </param>
        /// <param name="model">    The model. </param>
        /// <param name="year">     The year. </param>
        /// <param name="vin">      The vin. </param>
        ///
        /// <returns>   (motorcycle, null) on success, otherwise, (null, Error) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (Motorcycle motorcycle, IError error) NewMotorcycle(string make, string model, uint year, string vin)
        {
            var motorcycle = new Motorcycle
            {
                Id = Constants.InvalidEntityId, // Always initialize the Id to 0, or automatic incrementing will not happen.
                TenantId = Constants.InvalidTenantId,
                Make = make,
                Model = model,
                Year = year,
                Vin = vin,
                IsDeleted = false

                // CreatedUtc: Set when an instance is created in the repository,  otherwise null.            
                // ModifiedUtc: Set when an instance is updated in the repository, otherwise null.
            };

            var err = motorcycle.Validate();
            if (err != null)
            {
                return (null, err);
            }

            // All okay
            return (motorcycle, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This factory creates a Motorcycle instance with random values for testing purposes.
        /// </summary>
        ///
        /// <remarks>   The random fields are the following: make, model, vin, and year. </remarks>
        ///
        /// <returns>   (motorcycle, null) on success, otherwise, (null, Error) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (Motorcycle motorcycle, IError error) NewTestMotorcycle()
        {
            Random rnd = new Random();

            (Motorcycle motorcycle, IError error) = NewMotorcycle(StringExtensions.GenerateRandomString(RandomStringContent.LettersOnly, 5),
                                                                  StringExtensions.GenerateRandomString(RandomStringContent.LettersOnly),
                                                                  (uint) rnd.Next((int) Constants.MinYear, (int) Constants.MaxYear),
                                                                  StringExtensions.GenerateRandomString(RandomStringContent.AlphaNumeric,
                                                                                                        Constants.VinLength));

            if (error != null) return (null, error);

            var err = motorcycle.Validate();
            if (err != null)
            {
                return (null, err);
            }

            // All okay
            return (motorcycle, null);
        }

        #endregion
    }
}