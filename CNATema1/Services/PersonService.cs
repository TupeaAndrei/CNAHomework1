using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CNATema1.Services
{
    public class PersonService : Persongroup.PersongroupBase
    {
        private readonly ILogger<PersonService> _logger;
        public PersonService(ILogger<PersonService> logger)
        {
            _logger = logger;
        }

        public override Task<AddPersonResponse> AddPerson(AddPersonRequest request, ServerCallContext context)
        {
            var person = request.Person;
            //add to persons
            string editableCNP = person.Cnp;
            person.BirthYear = getBirthYear(editableCNP).ToString();
            person.Gender = getGender(editableCNP);
            _logger.Log(LogLevel.Information, "\nAdded person: " + person.Name + "\nBorn in: " + person.BirthYear + "\nGender: " + person.Gender);
            return Task.FromResult(new AddPersonResponse() { Status = AddPersonResponse.Types.Status.Succes });
        }

        public override Task<GetAllPersonsResponse> GetAllPersons(Empty request, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information, "GetAllPersons Called");
            //red from file
            var response = new GetAllPersonsResponse();
            return Task.FromResult(response);
        }

        public int getBirthYear(string numericalCode)
        {
            string year = numericalCode[1].ToString() + numericalCode[2].ToString();
            int birthYear = 0;
            if (!int.TryParse(year, out birthYear))
            {
                return -1;
            }
            if (birthYear < 22)
            {
                return 2000 + birthYear;
            }
            return 1900 + birthYear;
        }

        public string getGender(string numericalCode)
        {
            int birthYear = getBirthYear(numericalCode);
            if (birthYear > 1900 && birthYear < 2000)
            {
                if (numericalCode[0].Equals('1'))
                {
                    return "Masculin";
                }
                return "Feminin";
            }
            if (birthYear > 2000)
            {
                if (numericalCode[0].Equals('5'))
                {
                    return "Masculin";
                }
                if (numericalCode[0].Equals('6'))
                {
                    return "Feminin";
                }
            }
            return "other";
        }
    }
}
