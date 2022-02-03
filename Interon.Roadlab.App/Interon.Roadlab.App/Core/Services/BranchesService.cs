using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.Core.Dto;
using SQLite;
using Xamarin.Essentials;

namespace Interon.Roadlab.App.Core.Services
{


    class BranchesService : BaseService
    {
        private SQLiteConnection _db;
        public BranchesService()
        {
            _db = GetConnection();

            this._db.CreateTable<Branch>();

        }

        public Branch DtoToBranch(Interon.Roadlab.Core.Dto.BranchDto branchDto)
        {
            return new Branch(){ 
                CellphoneNumber = branchDto.CellNumber, 
                TelephoneNumber = branchDto.TelephoneNumber,
                Address1 = branchDto.Address1,
                Address2 = branchDto.Address2,
                Address3 = branchDto.Address3,
                City = branchDto.City,
                Province = branchDto.Province,
                Code = branchDto.Code,
                Email = branchDto.Email,
                Id=branchDto.Id,
                Name = branchDto.Name,
                ManagerName = branchDto.MangerName,
                Longitude = branchDto.Longitude,
                Latitude = branchDto.Latitude,
                Country = branchDto.Country
            };
        }

        public BranchDto BranchToDto(Branch branch)
        {
            return new BranchDto()
            {
                CellNumber = branch.CellphoneNumber,
                Email = branch.Email,
                Address1 = branch.Address1,
                Address2 = branch.Address2,
                Address3 = branch.Address3,
                City = branch.City,
                Province = branch.Province,
                Code = branch.Code,
                MangerName = branch.ManagerName,
                TelephoneNumber = branch.TelephoneNumber,
                Id = branch.Id,
                Latitude = branch.Latitude,
                Longitude = branch.Longitude,
                Name = branch.Name,
                Country = branch.Country
            };
        }
        public bool IsSavedOnLocalDevice(int Id)
        {
            return _db.Query<Branch>($"SELECT * FROM Branch where Id = {Id} ").Any();
        }

        public IEnumerable<Branch> GetBranches()
        {
            var branches = _db.Query<Branch>("SELECT * FROM Branch");
            var _branches = branches.OrderBy(x => x.Distance);
            return _branches;
        }
        public Branch GetBranchById(int id)
        {
            var branches = _db.Query<Branch>($"SELECT * FROM Branch where Id={id}");
            return branches.FirstOrDefault();
        }
        public Dictionary<double,Branch> GetBranchesByDistance()
        {
            var dicBranches = new Dictionary<double, Branch>();
            Location locationStart = new Location();
            try
            {
               
                for (int i = 0; i < 3; i++)
                {
                    locationStart = Geolocation.GetLastKnownLocationAsync().Result;
                }
               

                if (locationStart.Latitude == 0)
                {
                    foreach (var branch in GetBranches())
                    {
                        dicBranches.Add(0, branch);
                    }

                    return dicBranches;
                }

                foreach (var branch in GetBranches())
                {
                    var locationEnd = new Location(double.Parse(branch.Latitude, CultureInfo.InvariantCulture),double.Parse(branch.Longitude, CultureInfo.InvariantCulture));
                    dicBranches.Add(Location.CalculateDistance(locationStart,locationEnd,DistanceUnits.Kilometers),branch);
                }

                return dicBranches;

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return dicBranches;
        }

        public void CreateOrUpdateBranch(Branch Branch)
        {
            if (IsSavedOnLocalDevice(Branch.Id))
            {
                _db.Update(Branch);
            }
            else
            {
                _db.Insert(Branch);
            }
        }
        public void DeleteBranches()
        {
            _db.Execute("Delete from Branch where 1=1");
        }


    }
}
