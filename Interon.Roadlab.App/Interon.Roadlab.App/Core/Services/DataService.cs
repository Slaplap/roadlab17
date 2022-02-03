using System.Collections.Generic;
using System.Linq;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.Core.Dto;
using SQLite;

namespace Interon.Roadlab.App.Core.Services
{


    class DataService : BaseService
    {
        private SQLiteConnection _db;
        public DataService()
        {
            
            _db = GetConnection();
            this._db.CreateTable<TestCategory>();
            this._db.CreateTable<Test>();

        }

        public TestCategory DtoToTestCategory(TestCategoryDto testCategoryDto)
        {
            return new TestCategory()
            {
                Id = testCategoryDto.Id,
                Name = testCategoryDto.Name,
                Description = testCategoryDto.Description
            };

        }

        public TestCategoryDto TestCategoryToDto(TestCategory testCategory)
        {
            return new TestCategoryDto()
            {
                Id = testCategory.Id,
                Name = testCategory.Name,
                Description = testCategory.Description
            };
        }


        public Test DtoToTest(TestDto testDto)
        {
            return new Test()
            {
                Id          = testDto.Id,
                TestCategoryId = testDto.TestCategoryId,
                Name        = testDto.Name,
                Description = testDto.Description
            };

        }

        public TestDto TestDto(Test test)
        {
            return new TestDto()
            {
                Id          = test.Id,
                TestCategoryId = test.TestCategoryId,
                Name        = test.Name,
                Description = test.Description
            };
        }


        public bool IsCategorySavedOnLocalDevice(int Id)
        {
            var result = _db.Query<TestCategory>($"SELECT * FROM TestCategory  where Id = {Id} ").Any();
            
            return result;
        }
        public bool IsTestSavedOnLocalDevice(int Id)
        {
            var result = _db.Query<TestCategory>($"SELECT * FROM Test  where Id = {Id} ").Any();

            return result;
        }
        public List<TestCategory> GetTestCategories()
        {
            var tt = _db.Query<TestCategory>("SELECT * FROM TestCategory ORDER BY Name");
            return tt;
        }
        public TestCategory GetCategoryById(int id)
        {
            var tt = _db.Query<TestCategory>($"SELECT * FROM TestCategory  where Id={id}");
            return tt.FirstOrDefault();
        }
     

        public void CreateOrUpdateTestCategory(TestCategory testCategory)
        {
            if (IsCategorySavedOnLocalDevice(testCategory.Id))
            {
                _db.Update(testCategory);
            }
            else
            {
                _db.Insert(testCategory);
            }
        }
        public void DeleteTestCategories()
        {
            _db.Execute("Delete from TestCategory where 1=1");
        }

        public List<Test> GetTests()
        {
            var tt = _db.Query<Test>("SELECT * FROM Test ORDER BY Name");
            return tt;
        }
        public TestCategory GetTestById(int id)
        {
            var tt = _db.Query<TestCategory>($"SELECT * FROM Test  where Id={id}");
            return tt.FirstOrDefault();
        }
        public List<TestCategory> GetTestsByCategoryId(int categoryId)
        {
            var tt = _db.Query<TestCategory>($"SELECT * FROM Test  where TestCategoryId={categoryId}");
            return tt;
        }


        public void CreateOrUpdateTest(Test test)
        {
            if (IsCategorySavedOnLocalDevice(test.Id))
            {
                _db.Update(test);
            }
            else
            {
                _db.Insert(test);
            }
        }
        public void DeleteTests()
        {
            _db.Execute("Delete from Test where 1=1");
        }
    }
}
