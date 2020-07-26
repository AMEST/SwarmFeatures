using AutoMapper;
using Docker.DotNet.Models;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SwarmControl.Mappings
{
    public class DockerServiceMapperProfile : Profile
    {
        public DockerServiceMapperProfile()
        {
            CreateMap<SwarmService, DockerService>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ID))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Spec.Name))
                .ForMember(d => d.Labels, opt => opt.MapFrom(s => s.Spec.Labels))
                .ForMember(d => d.Replicas, opt => opt.MapFrom(s => s.Spec.Mode.Replicated.Replicas))
                .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Spec.TaskTemplate.ContainerSpec.Image))
                .ForMember(d => d.Placement, opt => opt.MapFrom(s => s.Spec.TaskTemplate.Placement))
                .ForMember(d => d.Ports, opt => opt.MapFrom(s => s.Spec.EndpointSpec.Ports));
        }
    }
}