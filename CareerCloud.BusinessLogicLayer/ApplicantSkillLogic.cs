using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CareerCloud.Pocos;
using CareerCloud.DataAccessLayer;

namespace CareerCloud.BusinessLogicLayer
{
    public class ApplicantSkillLogic : BaseLogic<ApplicantSkillPoco>
    {
        public ApplicantSkillLogic(IDataRepository<ApplicantSkillPoco> repository) : base(repository)
        {

        }
        protected override void Verify(ApplicantSkillPoco[] pocos)
        {
            //implement the exception rules 101,102,103,104
            List<ValidationException> exceptions = new List<ValidationException>();
            foreach (ApplicantSkillPoco poco in pocos)
            {
                if(poco.StartMonth > 12)
                {
                    exceptions.Add(new ValidationException(101, $"This {poco.StartMonth} is beyond the 12 month standard!"));
                }
                if (poco.EndMonth > 12)
                {
                    exceptions.Add(new ValidationException(102, $"This {poco.EndMonth} is beyond the 12 month standard!"));
                }
                if (poco.StartYear < 1900)
                {
                    exceptions.Add(new ValidationException(103, $"Uh oh! {poco.StartYear} is before 1900"));
                }
                if (poco.EndYear < poco.StartYear)
                {
                    exceptions.Add(new ValidationException(104, $"You shall not pass! {poco.EndYear} End Year is lesser than {poco.StartYear} Start Year!"));
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
        public override void Add(ApplicantSkillPoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            base.Add(pocos);
        }

        public override void Update(ApplicantSkillPoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            base.Update(pocos);
        }
    }
}
