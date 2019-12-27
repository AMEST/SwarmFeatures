using AutoMapper;
using Docker.DotNet.Models;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SwarmControl.Mappings
{
    public class DockerNodeMapperProfile : Profile
    {
        public DockerNodeMapperProfile()
        {
            CreateMap<NodeListResponse, DockerNode>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ID))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Spec.Name))
                .ForMember(d => d.Labels, opt => opt.MapFrom(s => s.Spec.Labels))
                .ForMember(d => d.Availability, opt => opt.MapFrom(s => s.Spec.Availability))
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Spec.Role))
                .ForMember(d => d.Hostname, opt => opt.MapFrom(s => s.Description.Hostname))
                .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Status.Addr));
        }
    }
}