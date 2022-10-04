using AutoMapper;
using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;

namespace DBR.Web.AutoMapper;

public class ApplicationMapper : Profile
{
	public ApplicationMapper()
	{
		CreateMap<Address, AddressInputModel>().ReverseMap();
		CreateMap<Address, AddressDTO>().ReverseMap();

		CreateMap<Specialization, SpecializationInputModel>().ReverseMap();
		CreateMap<Specialization, SpecializationDTO>().ReverseMap();

		CreateMap<Workshop, WorkshopInputModel>().ReverseMap();
		CreateMap<Workshop, WorkshopDTO>().ReverseMap();

		CreateMap<Vehicle, VehicleInputModel>().ReverseMap();
		CreateMap<Vehicle, VehicleDTO>().ReverseMap();

		CreateMap<Customer, CustomerInputModel>().ReverseMap();
		CreateMap<Customer, CustomerDTO>().ReverseMap();

		CreateMap<Case, CaseInputModel>().ReverseMap();
		CreateMap<Case, CaseDTO>().ReverseMap();

		CreateMap<Incident, IncidentInputModel>().ReverseMap();
		CreateMap<Incident, IncidentDTO>().ReverseMap();

		CreateMap<Image, ImageInputModel>().ReverseMap();
		CreateMap<Image, ImageDTO>().ReverseMap();

		CreateMap<Invoice, InvoiceInputModel>().ReverseMap();
		CreateMap<Invoice, InvoiceDTO>().ReverseMap();

		CreateMap<Video, VideoInputModel>().ReverseMap();
		CreateMap<Video, VideoDTO>().ReverseMap();

		CreateMap<Attachment, AttachmentInputModel>().ReverseMap();
		CreateMap<Attachment, AttachmentDTO>().ReverseMap();

		CreateMap<Member, MemberDTO>().ReverseMap();
	}
}