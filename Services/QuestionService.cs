using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Noodleator.Models;

// TODO: check for duplicates when posting

namespace Noodleator.Services
{
    public class QuestionService
    {
        private readonly IMongoCollection<Question> _questions;

        public QuestionService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _questions = database.GetCollection<Question>("questions");
        }

        public Question Random() =>
            _questions.AsQueryable().Sample(1).FirstOrDefault();
        public List<Question> Get() =>
            _questions.Find(quest => true).ToList();

        public Question Get(string id) =>
            _questions.Find(quest => quest.Id == id).SingleOrDefault();


        public Question Create(Question question)
        {
            _questions.InsertOne(question);
            return question;
        }

        public void Update(Question question) =>
            _questions.ReplaceOne(quest => quest.Id == question.Id, question);

        public void Delete(string id) =>
            _questions.DeleteOne(quest => quest.Id == id);
    }
}