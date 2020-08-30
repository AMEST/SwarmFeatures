using AutoMapper;
using Docker.DotNet.Models;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SwarmControl.Mappings
{
    public class DockerTaskMapperProfile : Profile
    {
        public DockerTaskMapperProfile()
        {
            CreateMap<TaskResponse, DockerTask>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ID))
                .ForMember(d => d.ServiceId, opt => opt.MapFrom(s => s.ServiceID))
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(s => s.UpdatedAt))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.NodeID, opt => opt.MapFrom(s => s.NodeID))
                .ForMember(d => d.State, opt => opt.MapFrom(s => s.DesiredState))
                .ForMember(d => d.Slot, opt => opt.MapFrom(s => s.Slot));
        }
    }
}