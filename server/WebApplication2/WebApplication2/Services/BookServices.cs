using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _books;

        public PostService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase("test");
            _books = database.GetCollection<Post>(settings.BooksCollectionName);
        }

        public List<Post> Get() =>
            _books.Find(book => book.IsActive).ToList();

        public Post Get(string id) =>
            _books.Find<Post>(book => book.Id == id).FirstOrDefault();

        public Post Create(Post post)
        {
            if (post.Comments==null)
            {
                post.Comments = new List<Comment>();
            }

            foreach (var comment in post.Comments)
            {
                if (string.IsNullOrWhiteSpace(comment.Id))
                {
                    comment.Id = (ObjectId.GenerateNewId()).ToString();
                }
            }

            post.IsActive = true;

            _books.InsertOne(post);
            return post;
        }

        public Comment CreateComment(string bookId, Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Id))
            {
                comment.Id = ObjectId.GenerateNewId().ToString();
            }

            var filter = Builders<Post>.Filter.Where(x => x.Id == bookId);
            var update = Builders<Post>.Update.Push(x => x.Comments, comment);
            _books.FindOneAndUpdate(filter, update);
            return comment;
        }

        public void Update(string id, Post postIn)
        {
            CreateComment(id, new Comment() {Body = $"Post updated. GEO: {postIn.Lat}, {postIn.Long}", Date = DateTime.Now.ToString("f")});

            var filter = Builders<Post>.Filter.Where(x => x.Id == id);
            var update = Builders<Post>.Update
                .Set(x=>x.ArticleText,postIn.ArticleText)
                .Set(x=>x.EmText, postIn.EmText)
                .Set(x=>x.ImgUrl, postIn.ImgUrl)
                .Set(x=>x.Title, postIn.Title)
                .Set(x => x.Lat, postIn.Lat)
                .Set(x => x.Long, postIn.Long);

            _books.FindOneAndUpdate(filter, update);
            return;
        }
         
        public void Remove(string id)
        {
            var filter = Builders<Post>.Filter.Where(x => x.Id == id);
            var update = Builders<Post>.Update.Set(x => x.IsActive, false);
            _books.FindOneAndUpdate(filter, update);
            return;
        }

        public Post DeleteComment(string postId, string commentId)
        {
            var collection = _books;

            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var update = Builders<Post>.Update.PullFilter("Comments",
                Builders<Comment>.Filter.Eq("Id", commentId));

            collection.FindOneAndUpdate(filter, update);

            var _findResult = collection.Find(filter).FirstOrDefault();
            return _findResult;
        }
    }
}
