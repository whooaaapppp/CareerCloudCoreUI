using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CareerCloud.Pocos;
using CareerCloud.DataAccessLayer;

namespace CareerCloud.BusinessLogicLayer
{
    public class CompanyJobDescriptionLogic : BaseLogic<CompanyJobDescriptionPoco>
    {
        public CompanyJobDescriptionLogic(IDataRepository<CompanyJobDescriptionPoco> repository) : base(repository)
        {

        }

        protected override void Verify(CompanyJobDescriptionPoco[] pocos)
        {
            //implement the exception rules 300,301
            List<ValidationException> exceptions = new List<ValidationException>();
            foreach (CompanyJobDescriptionPoco poco in pocos)
            {
                if (string.IsNullOrEmpty(poco.JobName))
                {
                    exceptions.Add(new ValidationException(300, $"Job Name {poco.JobName} cannot be empty. Please enter Job Name."));
                }
                if (string.IsNullOrEmpty(poco.JobDescriptions))
                {
                    exceptions.Add(new ValidationException(301, $"Job Description {poco.JobDescriptions} cannot be empty. Please enter Job Description."));
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
        public override void Add(CompanyJobDescriptionPoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            base.Add(pocos);
        }

        public override void Update(CompanyJobDescriptionPoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            base.Update(pocos);
        }
    }
}
