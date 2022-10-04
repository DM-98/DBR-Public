using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using DBR.Infrastructure.Repositories;
using DBR.Infrastructure.Services;
using DBR.Web.Context;

namespace DBR.Web.Helpers;

public static class ExtensionMethods
{
	public static IServiceCollection ConfigureDBRServices(this IServiceCollection services)
	{
		services.AddScoped<IRepository<Address, AddressInputModel, AddressDTO>, EFRepository<Address, AddressInputModel, AddressDTO, DBRContext>>();
		services.AddScoped<IRepository<Specialization, SpecializationInputModel, SpecializationDTO>, EFRepository<Specialization, SpecializationInputModel, SpecializationDTO, DBRContext>>();
		services.AddScoped<IRepository<Workshop, WorkshopInputModel, WorkshopDTO>, EFRepository<Workshop, WorkshopInputModel, WorkshopDTO, DBRContext>>();
		services.AddScoped<IRepository<Vehicle, VehicleInputModel, VehicleDTO>, EFRepository<Vehicle, VehicleInputModel, VehicleDTO, DBRContext>>();
		services.AddScoped<IRepository<Customer, CustomerInputModel, CustomerDTO>, EFRepository<Customer, CustomerInputModel, CustomerDTO, DBRContext>>();
		services.AddScoped<IRepository<Case, CaseInputModel, CaseDTO>, EFRepository<Case, CaseInputModel, CaseDTO, DBRContext>>();
		services.AddScoped<IRepository<Incident, IncidentInputModel, IncidentDTO>, EFRepository<Incident, IncidentInputModel, IncidentDTO, DBRContext>>();
		services.AddScoped<IRepository<Image, ImageInputModel, ImageDTO>, EFRepository<Image, ImageInputModel, ImageDTO, DBRContext>>();
		services.AddScoped<IRepository<Invoice, InvoiceInputModel, InvoiceDTO>, EFRepository<Invoice, InvoiceInputModel, InvoiceDTO, DBRContext>>();
		services.AddScoped<IRepository<Video, VideoInputModel, VideoDTO>, EFRepository<Video, VideoInputModel, VideoDTO, DBRContext>>();
		services.AddScoped<IRepository<Attachment, AttachmentInputModel, AttachmentDTO>, EFRepository<Attachment, AttachmentInputModel, AttachmentDTO, DBRContext>>();
		services.AddScoped<IRepository<Member, Member, Member>, EFRepository<Member, Member, Member, DBRContext>>();

		services.AddScoped<IService<Address, AddressInputModel, AddressDTO>, Service<Address, AddressInputModel, AddressDTO>>();
		services.AddScoped<IService<Specialization, SpecializationInputModel, SpecializationDTO>, Service<Specialization, SpecializationInputModel, SpecializationDTO>>();
		services.AddScoped<IService<Workshop, WorkshopInputModel, WorkshopDTO>, Service<Workshop, WorkshopInputModel, WorkshopDTO>>();
		services.AddScoped<IService<Vehicle, VehicleInputModel, VehicleDTO>, Service<Vehicle, VehicleInputModel, VehicleDTO>>();
		services.AddScoped<IService<Customer, CustomerInputModel, CustomerDTO>, Service<Customer, CustomerInputModel, CustomerDTO>>();
		services.AddScoped<IService<Case, CaseInputModel, CaseDTO>, Service<Case, CaseInputModel, CaseDTO>>();
		services.AddScoped<IService<Incident, IncidentInputModel, IncidentDTO>, Service<Incident, IncidentInputModel, IncidentDTO>>();
		services.AddScoped<IService<Image, ImageInputModel, ImageDTO>, Service<Image, ImageInputModel, ImageDTO>>();
		services.AddScoped<IService<Invoice, InvoiceInputModel, InvoiceDTO>, Service<Invoice, InvoiceInputModel, InvoiceDTO>>();
		services.AddScoped<IService<Video, VideoInputModel, VideoDTO>, Service<Video, VideoInputModel, VideoDTO>>();
		services.AddScoped<IService<Attachment, AttachmentInputModel, AttachmentDTO>, Service<Attachment, AttachmentInputModel, AttachmentDTO>>();
		services.AddScoped<IService<Member, Member, Member>, Service<Member, Member, Member>>();

		services.AddScoped<IAuthService, AuthService>();

		return services;
	}
}