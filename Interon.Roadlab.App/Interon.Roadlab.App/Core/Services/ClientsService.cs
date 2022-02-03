using System.Collections.Generic;
using System.Linq;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.Core.Dto;
using SQLite;

namespace Interon.Roadlab.App.Core.Services
{


    class ClientsService : BaseService
    {
        private SQLiteConnection _db;
        public ClientsService()
        {
            _db = GetConnection();

            this._db.CreateTable<Client>();
        }
         

        public Client DtoToCompany(Interon.Roadlab.LIMS.DTO.Client clientDto)
        {
            return new Client(){ 
                Id = clientDto.Id,
                Name =  clientDto.Name,
                AccountNumber = clientDto.AccountNumber
            };
        }

        public Interon.Roadlab.LIMS.DTO.Client CompanyToDto(Client client)
        {
            return new Interon.Roadlab.LIMS.DTO.Client()
            {
                 Id = client.Id,
                 Name = client.Name,
                 AccountNumber = client.AccountNumber
            };
        }
        public bool IsSavedOnLocalDevice(string accountNumber)
        {
            return _db.Query<Client>($"SELECT * FROM Client where AccountNumber = '{accountNumber}' ").Any();
        }

        public List<Client> GetCompanies()
        {
            var Companyes = _db.Query<Client>("SELECT * FROM Client");
            return Companyes;
        }
        public Client GetCompanyByAccountNumber(string accountNumber)
        {
            var Companies = _db.Query<Client>($"SELECT * FROM Client where AccountNumber='{accountNumber}'");
            return Companies.FirstOrDefault();
        }
    

        public void CreateOrUpdateCompany(Client client)
        {
            try
            {
                if (IsSavedOnLocalDevice(client.AccountNumber))
                {
                    _db.Update(client);
                }
                else
                {
                    _db.Insert(client);
                }
            }
            catch (SQLite.SQLiteException es)
            {

            }

            _db.Commit();
                
        }
        public void DeleteAll()
        {
            _db.Execute("Delete from Client where 1=1");
        }


    }
}
