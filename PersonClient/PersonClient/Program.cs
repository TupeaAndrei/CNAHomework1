using System;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System.Threading.Tasks;

namespace PersonClient
{
    class Program
    {
       static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Persongroup.PersongroupClient(channel);
            Console.WriteLine("Name: ");
            var name = Console.ReadLine();
            Console.WriteLine("CNP: ");
            var cnp = Console.ReadLine();
            var personToBeAdded = new Person() { Name = name.Trim().Length > 0 ? name : "anonym" , Cnp = cnp.Trim().Length == 13 ? cnp : "undefined"};
            var response = await client.AddPersonAsync(
                new AddPersonRequest { Person = personToBeAdded });
            Console.WriteLine("Person Adding status: " + response.Status);
        }
    }
}
