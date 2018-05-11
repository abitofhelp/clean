// SOLUTION: Clean
// PROJECT: Clean.Adapter
// FILE: MotorcycleRepositoryTest.cs
// CREATED: Mike Gardner

namespace Clean.Adapter.Gateways.Repositories.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Domain.Entities;
    using Shared.Enumerations;
    using Shared.Interfaces;
    using Xunit;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// MotorcycleRepositoryTest implementes tests for the MotorcycleRepository. This class cannot be
    /// inherited.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public sealed class MotorcycleRepositoryTest : IDisposable
    {
        #region Setup/Teardown

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// / <summary> MotorcycleRepositoryTest is the default constructor and is executed before each
        /// test.   </summary>
        ///   public MotorcycleRepositoryTest()
        ///   {
        ///       // MotorcycleRepositoryTest is the default constructor and is executed before each
        ///       test. Configure common test parameters, here...
        ///   }
        /// / <summary>   Dispose destroys testing environment and is executed after each test..
        /// </summary>
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            // Configure common test teardown, here...
        }

        #endregion

        /// <summary>   kUpdatedMake is the manufacturer that we use when testing for updates. </summary>
        private const string kUpdatedMake = "Harley Davidson";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// kDoesNotExistId is the value that we use when testing for an entity that does not exist.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private const long kDoesNotExistId = 1234;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// kDoesNotExistVin is the value that we use when testing for an entity's VIN does not already
        /// exist.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private const string kDoesNotExistVin = "00000000000000000";

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_Delete verifies that a motorcycle can be successfully deleted.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_Delete()
        {
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                MotorcycleContext.LoadContextWithTestData(context);
                var motorcycle = context.Entities.FirstOrDefault();
                var repo = new MotorcycleRepository(context);

                // ACT
                (OperationStatus status, _) = await repo.DeleteAsync(motorcycle?.Id ?? Constants.InvalidEntityId);

                // ASSERT
                Assert.True(status == OperationStatus.Ok);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_Delete_NotExist verifies that a motorcycle cannot be deleted if it doesn't
        /// exist.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_Delete_NotExist()
        {
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                var repo = new MotorcycleRepository(context);

                // ACT
                //  Look for an Id that doesn't exist...
                (OperationStatus status, _) = await repo.DeleteAsync(kDoesNotExistId);

                // ASSERT
                Assert.True(status == OperationStatus.NotFound);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_ExistsById verifies that a motorcycle exists using its primary key.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_ExistsById()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                MotorcycleContext.LoadContextWithTestData(context);
                var motorcycle = context.Entities.FirstOrDefault();
                var repo = new MotorcycleRepository(context);

                // ACT
                (bool exists, _, _) = await repo.ExistsByIdAsync(motorcycle?.Id ?? Constants.InvalidEntityId);

                // ASSERT
                Assert.True(exists);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_ExistsById_NotExist verifies that a motorcycle is cannot be found using an
        /// invalid primary key.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_ExistsById_No()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                var repo = new MotorcycleRepository(context);

                // ACT
                (bool exists, _, _) = await repo.ExistsByIdAsync(kDoesNotExistId);

                // ASSERT
                Assert.False(exists);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_ExistsByVin verifies that a motorcycle exists using its unique VIN.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_ExistsByVin()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                MotorcycleContext.LoadContextWithTestData(context);
                var motorcycle = context.Entities.FirstOrDefault();
                var repo = new MotorcycleRepository(context);

                // ACT
                (bool exists, _, _) = await repo.ExistsByVinAsync(motorcycle?.Vin);

                // ASSERT
                Assert.True(exists);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_ExistsByVin_NotExist verifies that a motorcycle cannot be found using an
        /// invalid VIN.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_ExistsByVin_No()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                var repo = new MotorcycleRepository(context);

                // ACT
                (bool exists, _, _) = await repo.ExistsByVinAsync(kDoesNotExistVin);

                // ASSERT
                Assert.False(exists);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_FetchById verifies that a motorcycle can be fetched using its primary key.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_FetchById()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                MotorcycleContext.LoadContextWithTestData(context);
                var motorcycle = context.Entities.FirstOrDefault();
                var repo = new MotorcycleRepository(context);

                // ACT
                (Motorcycle found, _, _) = await repo.FetchByIdAsync(motorcycle?.Id ?? Constants.InvalidEntityId);

                // ASSERT
                Assert.NotNull(found);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_FetchById_NotExist verifies that a motorcycle cannot be fetched using an
        /// invalid primary key.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_FetchById_No()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                var repo = new MotorcycleRepository(context);

                // ACT
                (Motorcycle found, _, _) = await repo.FetchByIdAsync(kDoesNotExistId);

                // ASSERT
                Assert.Null(found);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_FetchByVin verifies that a motorcycle can be fetched using a VIN.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_FetchByVin()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                MotorcycleContext.LoadContextWithTestData(context);
                var motorcycle = context.Entities.FirstOrDefault();
                var repo = new MotorcycleRepository(context);

                // ACT
                (Motorcycle found, _, _) = await repo.FetchByVinAsync(motorcycle?.Vin);

                // ASSERT
                Assert.NotNull(found);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_FetchByVin_NotExist verifies that a motorcycle cannot be fetched using an
        /// invalid VIN.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_FetchByVin_No()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                var repo = new MotorcycleRepository(context);

                // ACT
                (Motorcycle motorcycle, _, _) = await repo.FetchByVinAsync(kDoesNotExistVin);

                // ASSERT
                Assert.Null(motorcycle);
            }
        }

        /// <summary>   TestRepository_Insert verifies that an insert is successful. </summary>
        [Fact]
        public static async void TestRepository_Insert()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                (Motorcycle motorcycle, _) = Motorcycle.NewTestMotorcycle();
                var repo = new MotorcycleRepository(context);
                var found = false;

                // ACT
                (Motorcycle entity, _, _) = await repo.InsertAsync(motorcycle,
                                                                   (moto) =>
                                                                   {
                                                                       return repo.ExistsByIdAsync(moto.Id);
                                                                   });

                found = context.Entities.Any(x => x.Id.Equals(entity.Id));

                // ASSERT
                Assert.NotNull(entity);
                Assert.True(found);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_Insert_VinAlreadyExists verifies that an insert is not successful when the VIN
        /// already exists.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_Insert_VinExists()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                MotorcycleContext.LoadContextWithTestData(context);
                var motorcycle = context.Entities.FirstOrDefault();
                var repo = new MotorcycleRepository(context);

                // ACT
                (_, OperationStatus status, IError error) = await repo.InsertAsync(motorcycle,
                                                                        (moto) =>
                                                                        {
                                                                            // If exists is true, then the VIN is not unique.
                                                                            return repo.ExistsByVinAsync(moto.Vin);
                                                                        });

                // ASSERT
                Assert.True(status == OperationStatus.Found);
                Assert.NotNull(error);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_ListEmpty verifies that an empty list of motorcycles is returned.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_ListEmpty()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                var repo = new MotorcycleRepository(context);

                // ACT
                (IReadOnlyCollection<Motorcycle> list, _, _) = await repo.ListAsync();

                // ASSERT
                Assert.Empty(list);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_ListOfOne verifies that a list with one motorcycle is returned.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_ListOfOne()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                MotorcycleContext.LoadContextWithTestData(context);
                var repo = new MotorcycleRepository(context);

                // ACT
                (IReadOnlyCollection<Motorcycle> list, _, _) = await repo.ListAsync();

                // ASSERT
                Assert.True(list.Count == 1);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_Save verifies that a save of the unit of work/dbContext is successful.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_Save()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                var repo = new MotorcycleRepository(context);

                // ACT
                (OperationStatus status, _) = await repo.SaveAsync();

                // ASSERT
                Assert.True(status == OperationStatus.Ok);
            }
        }

        /// <summary>   TestRepository_Update verifies that an update is successful. </summary>
        [Fact]
        public static async void TestRepository_Update()
        {
            // ARRANGE           
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                MotorcycleContext.LoadContextWithTestData(context);
                var motorcycle = context.Entities.FirstOrDefault();
                var repo = new MotorcycleRepository(context);
                if (motorcycle != null) motorcycle.Make = kUpdatedMake;

                // ACT
                (Motorcycle updated, _, _) = await repo.UpdateAsync(motorcycle?.Id ?? Constants.InvalidEntityId,
                                                                    motorcycle,
                                                                        async (moto) =>
                                                                        {
                                                                            var count = (await repo.ListAsync()).list.Count(x => x.Vin.Equals(moto.Vin, StringComparison.CurrentCultureIgnoreCase));
                                                                            return (count == 1);
                                                                        });

                // ASSERT
                Assert.True(updated.Make.Equals(kUpdatedMake));
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_Update_NotExist verifies that an update fails if the entity does not exist.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_Update_NotExist()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                MotorcycleContext.LoadContextWithTestData(context);
                var motorcycle = context.Entities.FirstOrDefault();
                var repo = new MotorcycleRepository(context);
                if (motorcycle != null) motorcycle.Make = kUpdatedMake;

                // ACT
                (_, OperationStatus status, _) = await repo.UpdateAsync(kDoesNotExistId,
                                                                        motorcycle,
                                                                        async (moto) =>
                                                                        {
                                                                            var count = (await repo.ListAsync()).list.Count(x => x.Vin.Equals(moto.Vin, StringComparison.CurrentCultureIgnoreCase));
                                                                            return (count == 1);
                                                                        });

                // ASSERT
                Assert.True(status == OperationStatus.NotFound);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestRepository_Update_VinAlreadyExists verifies that an update fails if the VIN already
        /// exists for an entity with a different primary key.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static async void TestRepository_Update_VinExists()
        {
            // ARRANGE
            var options = MotorcycleContext.CreateTestingContextOptions();
            using (var context = new MotorcycleContext(options))
            {
                MotorcycleContext.LoadContextWithTestData(context, 2);
                var motorcycle1 = context.Entities.FirstOrDefault();
                var motorcycle2 = context.Entities.LastOrDefault();
                var repo = new MotorcycleRepository(context);
                if (motorcycle2 != null) motorcycle2.Vin = motorcycle1?.Vin;

                // ACT
                (_, OperationStatus status, _) = await repo.UpdateAsync(motorcycle2?.Id ?? Constants.InvalidEntityId,
                                                                        motorcycle2,
                                                                        async (moto) =>
                                                                        {
                                                                            var count = (await repo.ListAsync()).list.Count(x => x.Vin.Equals(moto.Vin, StringComparison.CurrentCultureIgnoreCase));
                                                                            return (count == 1);
                                                                        });

                // ASSERT
                Assert.True(status == OperationStatus.InternalError);
            }
        }
    }
}