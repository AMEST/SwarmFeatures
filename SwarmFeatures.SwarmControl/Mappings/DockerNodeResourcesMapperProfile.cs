using AutoMapper;
using Docker.DotNet.Models;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SwarmControl.Mappings
{
    public class DockerNodeResourcesMapperProfile : Profile
    {
        public DockerNodeResourcesMapperProfile()
        {
            CreateMap<SwarmResources, DockerNodeResources>()
                .ForMember(d => d.MemoryBytes, opt => opt.MapFrom(s => s.MemoryBytes))
                .ForMember(d => d.NanoCPUs, opt => opt.MapFrom(s => s.NanoCPUs));
        }
    }
}