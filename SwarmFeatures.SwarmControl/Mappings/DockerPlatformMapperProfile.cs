using AutoMapper;
using Docker.DotNet.Models;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SwarmControl.Mappings
{
    public class DockerPlatformMapperProfile : Profile
    {
        public DockerPlatformMapperProfile()
        {
            CreateMap<Platform, DockerPlatform>()
                .ForMember(d => d.Architecture, opt => opt.MapFrom(s => s.Architecture))
                .ForMember(d => d.OS, opt => opt.MapFrom(s => s.OS));
        }
    }
}