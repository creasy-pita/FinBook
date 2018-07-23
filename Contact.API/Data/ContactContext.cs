using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Models;
using Contact.API.Dto;
using Microsoft.Extensions.Options;

namespace Contact.API.Data
{
    public class ContactContext
    {
        private IMongoDatabase _database;
        private IMongoCollection<Models.Contact> _collection;
        private AppSettings _appSettings;

        public ContactContext(IOptionsSnapshot<AppSettings> settings)
        {
            _appSettings = settings.Value;
            var client = new MongoClient(_appSettings.MongoContactConnectionString);
            if(client != null)
            {
                //it will actually creatr the database if it has not already been created
                _database = client.GetDatabase(_appSettings.ContactDatabaseName);
            }
        }

        public void CheckAndCreate(string collectionName)
        {
            var collections = _database.ListCollections().ToList();
            var collectionNames = new List<string>();
            collections.ForEach(d => collectionNames.Add(d["name"].AsString));
            if (!collectionNames.Contains(collectionName))
            {
                _database.CreateCollection(collectionName);
            }
        }

        public IMongoCollection<ContactApplyRequest> ContactApplyRequests
        {
            get {
                CheckAndCreate("ContactApplyRequest");
                return _database.GetCollection<ContactApplyRequest>("ContactApplyRequest");
            }
        }

        public IMongoCollection<ContactBook> ContactBooks
        {
            get
            {
                CheckAndCreate("ContactBook");
                return _database.GetCollection<ContactBook>("ContactBook");
            }
        }

        public IMongoCollection<Models.Contact> Contacts
        {
            get
            {
                CheckAndCreate("Contact");
                return _database.GetCollection<Models.Contact>("Contact");
            }
        }
    }
}
