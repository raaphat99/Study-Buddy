using Application.DTOs.TopicDTOs;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TopicService : ITopicService
    {
        public ITopicRepository _topicRepository { get; set; }
        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }
        public async Task<IEnumerable<TopicDto>> GetAllTopicsAsync()
        {
            var topics = await _topicRepository.GetAllAsync();
            List<TopicDto> topicDtos = new List<TopicDto>();
            foreach (var topic in topics)
            {
                var topicDto = new TopicDto
                {
                    Id = topic.Id,
                    Name = topic.Name
                };
                topicDtos.Add(topicDto);
            }
            return topicDtos;
        }
    }
}
