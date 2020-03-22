using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CareerCloud.Pocos;
using CareerCloud.DataAccessLayer;

namespace CareerCloud.BusinessLogicLayer
{
    public class SystemCountryCodeLogic
    {
        protected IDataRepository<SystemCountryCodePoco> _repository;
        //create constructor to pass IDataRepository
        public SystemCountryCodeLogic(IDataRepository<SystemCountryCodePoco> repository)
        {
            _repository = repository;
        }
        protected void Verify(SystemCountryCodePoco[] pocos)
        {
            //implement 900,901
            List<ValidationException> exceptions = new List<ValidationException>();
            foreach (SystemCountryCodePoco poco in pocos)
            {
                if (string.IsNullOrEmpty(poco.Code))
                {
                    exceptions.Add(new ValidationException(900, $"Code {poco.Code} cannot be empty!"));
                }
                if (string.IsNullOrEmpty(poco.Name))
                {
                    exceptions.Add(new ValidationException(901, $"Name {poco.Name} cannot be empty!"));
                }
            }
            //check exception count
            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
        //access member LanguageID
        public SystemCountryCodePoco Get(string code)
        {
            return _repository.GetSingle(x => x.Code == code);
        }
        public List<SystemCountryCodePoco> GetAll()
        {
            return _repository.GetAll().ToList();
        }
        public void Add(SystemCountryCodePoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            _repository.Add(pocos);
        }
        public void Update(SystemCountryCodePoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            _repository.Update(pocos);
        }
        public void Delete(SystemCountryCodePoco[] pocos)
        {
            Verify(pocos);
            _repository.Remove(pocos);
        }
    }
}
