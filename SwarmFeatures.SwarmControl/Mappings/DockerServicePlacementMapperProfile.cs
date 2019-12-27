using AutoMapper;
using Docker.DotNet.Models;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SwarmControl.Mappings
{
    public class DockerServicePlacementMapperProfile : Profile
    {
        public DockerServicePlacementMapperProfile()
        {
            CreateMap<Placement, DockerServicePlacement>()
                .ForMember(d => d.Constraints, opt => opt.MapFrom(s => s.Constraints));
            CreateMap<DockerServicePlacement, Placement> ()
                .ForMember(d => d.Constraints, opt => opt.MapFrom(s => s.Constraints));
        }
    }
}