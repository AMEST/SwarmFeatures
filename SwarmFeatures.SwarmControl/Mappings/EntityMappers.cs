using System.Collections.Generic;
using AutoMapper;
using Docker.DotNet.Models;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SwarmControl.Mappings
{
    public static class EntityMappers
    {
        static EntityMappers()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DockerPortConfigurationProfile>();
                cfg.AddProfile<DockerServiceMapperProfile>();
                cfg.AddProfile<DockerServicePlacementMapperProfile>();
                cfg.AddProfile<DockerNodeMapperProfile>();
                cfg.AddProfile<DockerNodeResourcesMapperProfile>();
                cfg.AddProfile<DockerPlatformMapperProfile>();
            });
            Mapper = config.CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static DockerService ToEntity(this SwarmService source)
        {
            return Mapper.Map<DockerService>(source);
        }

        public static List<DockerService> ToEntity(this IEnumerable<SwarmService> source)
        {
            return Mapper.Map<List<DockerService>>(source);
        }

        public static DockerServicePlacement ToEntity(this Placement source)
        {
            return Mapper.Map<DockerServicePlacement>(source);
        }

        public static Placement ToObject(this DockerServicePlacement source)
        {
            return Mapper.Map<Placement>(source);
        }

        public static IEnumerable<DockerNode> ToEntity(this IEnumerable<NodeListResponse> source)
        {
            return Mapper.Map<IEnumerable<DockerNode>>(source);
        }
        public static DockerNode ToEntity(this NodeListResponse source)
        {
            return Mapper.Map<DockerNode>(source);
        }
    }
}