using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CareerCloud.Pocos;
using CareerCloud.DataAccessLayer;

namespace CareerCloud.BusinessLogicLayer
{
    public class ApplicantWorkHistoryLogic : BaseLogic<ApplicantWorkHistoryPoco>
    {
        public ApplicantWorkHistoryLogic(IDataRepository<ApplicantWorkHistoryPoco> repository) : base(repository)
        {

        }

        protected override void Verify(ApplicantWorkHistoryPoco[] pocos)
        {
            //implement the exception rules 105
            List<ValidationException> exceptions = new List<ValidationException>();
            foreach (ApplicantWorkHistoryPoco poco in pocos)
            {
                if (poco.CompanyName.Length < 3)
                {
                    exceptions.Add(new ValidationException(105, $"Company Name {poco.CompanyName} must be more than 2 characters!"));
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
        public override void Add(ApplicantWorkHistoryPoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            base.Add(pocos);
        }

        public override void Update(ApplicantWorkHistoryPoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            base.Update(pocos);
        }
    }
}
