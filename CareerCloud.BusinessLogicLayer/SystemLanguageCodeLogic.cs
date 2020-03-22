using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CareerCloud.Pocos;
using CareerCloud.DataAccessLayer;

namespace CareerCloud.BusinessLogicLayer
{
    public class SystemLanguageCodeLogic
    {
        protected IDataRepository<SystemLanguageCodePoco> _repository;
        //create constructor to pass IDataRepository
        public SystemLanguageCodeLogic(IDataRepository<SystemLanguageCodePoco> repository)
        {
            _repository = repository;
        }
        protected void Verify(SystemLanguageCodePoco[] pocos)
        {
            //implement 1000,1001,1002
            List<ValidationException> exceptions = new List<ValidationException>();
            foreach(SystemLanguageCodePoco poco in pocos)
            {
                if (string.IsNullOrEmpty(poco.LanguageID))
                {
                    exceptions.Add(new ValidationException(1000, $"Language ID {poco.LanguageID} cannot be empty!"));
                }
                if (string.IsNullOrEmpty(poco.Name))
                {
                    exceptions.Add(new ValidationException(1001, $"Name {poco.Name} cannot be empty!"));
                }
                if (string.IsNullOrEmpty(poco.NativeName))
                {
                    exceptions.Add(new ValidationException(1002, $"Native Name {poco.NativeName} cannot be empty!"));
                }
            }
            //check exception count
            if(exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
        //access member LanguageID
        public SystemLanguageCodePoco Get(string languageID)
        {
            return _repository.GetSingle(x => x.LanguageID == languageID);
        }
        public List<SystemLanguageCodePoco> GetAll()
        {
            return _repository.GetAll().ToList();
        }
        public void Add(SystemLanguageCodePoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            _repository.Add(pocos);
        }
        public void Update(SystemLanguageCodePoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            _repository.Update(pocos);
        }
        public void Delete(SystemLanguageCodePoco[] pocos)
        {
            Verify(pocos);
            _repository.Remove(pocos);
        }
    }
}
