using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xero.Api.Core;
using Xero.Api.Core.Model;
using Xero.Api.Core.Model.Types;
using Xero.Api.Example.Applications.Private;
using Xero.Api.Example.Applications.Public;
using Xero.Api.Example.TokenStores;
using Xero.Api.Infrastructure.OAuth;
using Xero.Api.Serialization;
using XeroFakesLoader.Properties;

namespace XeroFakesLoader {
    class Program {
        static void Main(string[] args) {
            var apiCore = new XeroCoreApi(
                "https://api.xero.com/api.xro/2.0/",
                new PrivateAuthenticator(getCert()),
                new Consumer("&CHANGE_TO_ACTUAL_CONSUMER_KEY&", "%CHANGE_TO_ACTUAL_CONSUMER_SECRET%"),  // https://app.xero.com/Application/Edit/...
                null,
                new DefaultMapper(),
                new DefaultMapper());


            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Generating contacts");
            var contactCreator = new ContactCreator();
            var contacts = new List<Contact>();
            for (int i = 0; i < 500; i++) {
                var newContact = contactCreator.GenerateRandomContact();
                contacts.Add(newContact);
            }
            Console.WriteLine($"Generation contacts completed and took {stopwatch.Elapsed}");
            Console.WriteLine("Uploading contacts into Xero company");
            stopwatch = Stopwatch.StartNew();
            try {
                apiCore.Contacts.Create(contacts);
                Console.WriteLine($"Uploading contacts into Xero company completed and took {stopwatch.Elapsed}");
            }
            catch (Exception ex) {
                Console.Write($"Exception occured while uploading contacts {ex.ToString()}");
            }

            Console.ReadLine();
        }

        private static X509Certificate2 getCert() {
            X509Certificate2 cert = new X509Certificate2();
            cert.Import(Resources.cert, "1qa@WS3ed", X509KeyStorageFlags.MachineKeySet);
            return cert;
        }
    }
}