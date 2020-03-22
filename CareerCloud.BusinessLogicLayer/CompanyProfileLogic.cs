using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CareerCloud.Pocos;
using CareerCloud.DataAccessLayer;
using System.Text.RegularExpressions;

namespace CareerCloud.BusinessLogicLayer
{
    public class CompanyProfileLogic : BaseLogic<CompanyProfilePoco>
    {
        public CompanyProfileLogic(IDataRepository<CompanyProfilePoco> repository) : base(repository)
        {

        }
        protected override void Verify(CompanyProfilePoco[] pocos)
        {
            //implement the exception rules 600, 601
            List<ValidationException> exceptions = new List<ValidationException>();
            foreach (CompanyProfilePoco poco in pocos)
            {
                //600 website suffix regex .ca | .com | .biz
                if (string.IsNullOrEmpty(poco.CompanyWebsite))
                {
                    exceptions.Add(new ValidationException(600, $"Company Website {poco.CompanyWebsite} field is empty."));
                }
                //regex101.com saves the day
                else if (!Regex.IsMatch(poco.CompanyWebsite, @"\A(?:(http)?s?:\/\/)?(www.)?([a-z0-9!]+\-?[a-z0-9!]+)+\.(ca|biz|com)\Z", RegexOptions.IgnoreCase))
                {
                    exceptions.Add(new ValidationException(600, $"Company Website {poco.CompanyWebsite} is not a valid website address format."));
                }

                //601 Must correspond to a valid phone number(e.g. 416 - 555 - 1234)
                if (string.IsNullOrEmpty(poco.ContactPhone))
                {
                    exceptions.Add(new ValidationException(601, $"Contact Phone Number is required"));
                }
                else
                {
                    string[] phoneComponents = poco.ContactPhone.Split('-');
                    if (phoneComponents.Length != 3)
                    {
                        exceptions.Add(new ValidationException(601, $"Contact Phone Number {poco.ContactPhone} is not in the required format."));
                    }
                    else
                    {
                        if (phoneComponents[0].Length != 3)
                        {
                            exceptions.Add(new ValidationException(601, $"Contact Phone Number {poco.ContactPhone} is not in the required format."));
                        }
                        else if (phoneComponents[1].Length != 3)
                        {
                            exceptions.Add(new ValidationException(601, $"Contact Phone Number {poco.ContactPhone} is not in the required format."));
                        }
                        else if (phoneComponents[2].Length != 4)
                        {
                            exceptions.Add(new ValidationException(601, $"Contact Phone Number {poco.ContactPhone} is not in the required format."));
                        }
                    }
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
        public override void Add(CompanyProfilePoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            base.Add(pocos);
        }

        public override void Update(CompanyProfilePoco[] pocos)
        {
            //make sure poco is clean, doesn't have exceptions
            Verify(pocos);
            base.Update(pocos);
        }

    }
}
