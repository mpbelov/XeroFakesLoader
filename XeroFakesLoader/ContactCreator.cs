using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Api.Core.Model;
using Xero.Api.Core.Model.Types;

namespace XeroFakesLoader {
    public class ContactCreator {
        static string[] currencies = new string[] {
            "USD", "EUR", "GBP", "INR", "AUD", "CAD", "SGD", "CHF", "MYR", "JPY", "CNY"
        };
        Random random = new Random();

        public Contact GenerateRandomContact() {
            var contact = new Contact();
            contact.Name = Faker.Company.Name();
            if (random.Next(5) == 0) {
                contact.FirstName = Faker.Name.First();
                contact.LastName = Faker.Name.Last();
                contact.EmailAddress = Faker.Internet.Email(contact.FirstName);
                contact.ContactPersons = new List<ContactPerson>();

                for (int i = 0; i < random.Next(6); i++) {
                    var newPerson = new ContactPerson();
                    newPerson.FirstName = Faker.Name.First();
                    newPerson.LastName = Faker.Name.Last();
                    newPerson.EmailAddress = Faker.Internet.Email(newPerson.FirstName);
                    newPerson.IncludeInEmails = random.Next(2) == 0;
                    contact.ContactPersons.Add(newPerson);
                }
            }
            contact.Addresses = new List<Address>() {
                new Address() {
                    AddressType = AddressType.PostOfficeBox,
                    City = Faker.Address.City(),
                    AddressLine1 = Faker.Address.StreetAddress(),
                    Country = Faker.Address.Country(),
                    PostalCode = Faker.Address.ZipCode(),
                    AttentionTo = Faker.Lorem.Sentence()
                },
            };
            for (int i = 0; i < random.Next(4); i++) {
                contact.Addresses.Add(
                    new Address() {
                        AddressType = AddressType.Street,
                        City = Faker.Address.City(),
                        AddressLine1 = Faker.Address.StreetAddress(),
                        Country = Faker.Address.Country(),
                        PostalCode = Faker.Address.ZipCode(),
                        AttentionTo = Faker.Lorem.Sentence()
                    }
                );
            }
            contact.DefaultCurrency = currencies[random.Next(currencies.Length)];
            contact.Phones = new List<Phone>();
            for (int i = 0; i < random.Next(3); i++) {
                var newPhone = new Phone();
                newPhone.PhoneType = (PhoneType)i;
                newPhone.PhoneNumber = Faker.Phone.Number();
            }
            return contact;
        }
    }
}