using AutoMapper;
using Docker.DotNet.Models;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SwarmControl.Mappings
{
    public class DockerPortConfigurationProfile : Profile
    {
        public DockerPortConfigurationProfile()
        {
            CreateMap<PortConfig, PortConfiguration>();
        }
    }
}