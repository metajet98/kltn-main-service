using System;
using System.Collections.Generic;
using System.Linq;
using main_service.Databases;
using main_service.Repositories.Base;
using main_service.RestApi.Requests;

namespace main_service.Repositories
{
    public class TopicRepository : BaseRepository<Topic>
    {
        public TopicRepository(AppDBContext context) : base(context)
        {
        }

        public IEnumerable<Topic> QueryTopic(TopicQuery topicQuery)
        {
            IQueryable<Topic> query = DbSet;
            if (topicQuery.CreatedUserId != null)
            {
                query = query.Where(x => x.UserId.Equals(topicQuery.CreatedUserId));
            }

            return query.ToList();
        }
        public bool CreateTopic(TopicRequest topicRequest, int userId)
        {
            try
            {
                var newTopic = new Topic
                {
                    UserId = userId,
                    Content = topicRequest.Content,
                    Title = topicRequest.Title,
                    CreatedDate = DateTime.Now
                };
                Context.Topic.Add(newTopic);
                Context.SaveChanges();

                if (topicRequest.Images != null && topicRequest.Images.Count > 0)
                {
                    var newImages = topicRequest.Images.Select(x => new TopicImage
                    {
                        TopicId = newTopic.Id,
                        Image = x,
                        CreatedDate = DateTime.Now,
                    }).ToList();
                    Context.TopicImage.AddRange(newImages);
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool PostReply(TopicReplyRequest topicReplyRequest, int topicId, int userId)
        {
            try
            {
                var newTopicReply = new TopicReply
                {
                    Image = topicReplyRequest.Image,
                    Content = topicReplyRequest.Content,
                    UserId = userId,
                    CreatedDate = DateTime.Now,
                    TopicId = topicId
                };
                Context.TopicReply.Add(newTopicReply);
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}