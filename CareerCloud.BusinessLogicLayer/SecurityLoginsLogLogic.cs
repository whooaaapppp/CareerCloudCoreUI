using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CareerCloud.Pocos;
using CareerCloud.DataAccessLayer;

namespace CareerCloud.BusinessLogicLayer
{
    public class SecurityLoginsLogLogic : BaseLogic<SecurityLoginsLogPoco>
    {
        public SecurityLoginsLogLogic(IDataRepository<SecurityLoginsLogPoco> repository) : base(repository)
        {

        }

        protected override void Verify(SecurityLoginsLogPoco[] pocos)
        {
            //implement the exception rules as defined
            List<ValidationException> exceptions = new List<ValidationException>();
            foreach (SecurityLoginsLogPoco poco in pocos)
            {
                //for future iterations
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
        public override void Add(SecurityLoginsLogPoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            base.Add(pocos);
        }

        public override void Update(SecurityLoginsLogPoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            base.Update(pocos);
        }

    }
}
