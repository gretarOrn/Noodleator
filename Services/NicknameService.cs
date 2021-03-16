using MongoDB.Driver;
using System.Collections.Generic;
using Noodleator.Models;
namespace Noodleator.Services
{
    public class NicknameService
    {
        private readonly IMongoCollection<Nickname> _nicknames;

        public NicknameService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _nicknames = database.GetCollection<Nickname>("nicknames");
        }

        public List<Nickname> Get() =>
            _nicknames.Find(nick => true).ToList();

        public Nickname Get(string id) =>
            _nicknames.Find(nick => nick.Id == id).SingleOrDefault();


        public Nickname Create(Nickname nickname)
        {
            _nicknames.InsertOne(nickname);
            return nickname;
        }

        public void Update(Nickname nickname) =>
            _nicknames.ReplaceOne(nick => nick.Id == nickname.Id, nickname);

        public void Delete(string id) =>
            _nicknames.DeleteOne(nick => nick.Id == id);
    }
}
