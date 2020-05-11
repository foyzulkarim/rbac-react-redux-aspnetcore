using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication2.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        public string EmText { get; set; }

        public string ArticleText { get; set; }

        public string ImgUrl { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }

        public bool IsActive { get; set; }

        public List<Comment> Comments { get; set; }
    }

    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Body { get; set; }

        public string Date { get; set; }
    }
}
