// SOLUTION: Clean
// PROJECT: Clean.Domain
// FILE: MotorcycleTest.cs
// CREATED: Mike Gardner

// Namespace Entities contains the domain entities.
namespace Clean.Domain.Entities.UnitTests
{
    using System;
    using Shared.Interfaces;
    using Xunit;

    /// <summary>   A motorcycle test. This class cannot be inherited. </summary>
    public sealed class MotorcycleTest : IDisposable
    {
        #region Setup/Teardown

        // ARRANGE
        // Setup testing environment
        // You can't have methods executing before and after each test separately.  
        // The aim of this is to improve test isolation and to make the tests more readable. 
        // We are constructing and disposing the test object for EACH TEST.

        /// <summary>   Destroy testing environment. </summary>
        public void Dispose() { }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestMotorcycle_ChangeFieldValueAndValidate_Failure verifies that changing a field with an
        /// invalid value will fail validation.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestMotorcycle_ChangeFieldValue_Failure()
        {
            // ARRANGE
            (Motorcycle motorcycle, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 2006, "01234567890123456");
            motorcycle.Year = 3000;

            // ACT
            error = motorcycle.Validate();

            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestMotorcycle_ChangeFieldValueAndValidate_Successful verifies that changing a field with a
        /// correct value will pass validation.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestMotorcycle_ChangeFieldValue_Success()
        {
            // ARRANGE
            (Motorcycle motorcycle, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 2006, "01234567890123456");
            motorcycle.Year = 2007;

            // ACT
            error = motorcycle.Validate();

            // ASSERT
            Assert.Null(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestMotorcycleMake_FordIsInvalid verifies that Ford is an invalid manufacturer.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestMotorcycleMake_FordIsInvalid()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Ford", "Falcon", 2006, "01234567890123456");

            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestMotorcycleMake_HondaIsValid verifies that Honda is a valid manufacturer.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestMotorcycleMake_HondaIsValid()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 2006, "01234567890123456");

            // ASSERT
            Assert.Null(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestMotorcycleMake_LengthLTE20 verifies that the make cannot exceed 20 characters.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestMotorcycleMake_LengthLTE20()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("0123456789012345678901234", "Shadow", 2006, "01234567890123456");

            // ASSERT
            Assert.NotNull(error);
        }

        /// <summary>   TestMotorcycleMake_NotEmpty verifies that the make cannot be empty. </summary>
        [Fact]
        public static void TestMotorcycleMake_NotEmpty()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("", "Shadow", 2006, "01234567890123456");

            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestMotorcycleModel_LengthLTE20 verifies that the model cannot exceed 20 characters.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestMotorcycleModel_LengthLTE20()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Honda", "0123456789012345678901234", 2006, "01234567890123456");

            // ASSERT
            Assert.NotNull(error);
        }

        /// <summary>   TestMotorcycleModel_NotEmpty verifies that the model cannot be empty. </summary>
        [Fact]
        public static void TestMotorcycleModel_NotEmpty()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Honda", "", 2006, "01234567890123456");

            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestMotorcycleVin_17Characters verifies that the VIN's length is 17 characters.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestMotorcycleVin_17Characters()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 2006, "01234567890123456");

            // ASSERT
            Assert.Null(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestMotorcycleVin_GT17Characters verifies that the VIN's length cannot be more than 17
        /// characters.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestMotorcycleVin_GT17Characters()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 2006, "012345678901234567");

            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestMotorcycleVin_LT17Characters verifies that the VIN's length cannot be less than 17
        /// characters.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestMotorcycleVin_LT17Characters()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 2006, "0123456789012345");

            // ASSERT
            Assert.NotNull(error);
        }

        /// <summary>   TestMotorcycleYear_1999 verifies that 1999 is a valid year. </summary>
        [Fact]
        public static void TestMotorcycleYear_1999()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 1999, "01234567890123456");

            // ASSERT
            Assert.Null(error);
        }

        /// <summary>   TestMotorcycleYear_2020 verifies that 2020 is a valid year. </summary>
        [Fact]
        public static void TestMotorcycleYear_2020()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 2020, "01234567890123456");

            // ASSERT
            Assert.Null(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestMotorcycleYear_GT2020 verifies that the year cannot be greater than 2020.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestMotorcycleYear_GT2020()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 2021, "01234567890123456");

            // ASSERT
            Assert.NotNull(error);
        }

        /// <summary>   TestMotorcycleYear_LT1999 verifies that the year cannot be less than 1999. </summary>
        [Fact]
        public static void TestMotorcycleYear_LT1999()
        {
            // ARRANGE

            // ACT
            (_, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 1998, "01234567890123456");

            // ASSERT
            Assert.NotNull(error);
        }
    }
}