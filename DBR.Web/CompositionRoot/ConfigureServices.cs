using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Interfaces;
using DBR.Infrastructure.Repositories;
using DBR.Infrastructure.Services;
using DBR.Infrastructure.Context;

namespace DBR.Web.CompositionRoot;

public static class ConfigureServices
{
	public static IServiceCollection ConfigureDBRServices(this IServiceCollection services)
	{
		services.AddScoped<IRepository<Address>, EFRepository<Address, DBRContext>>();
		services.AddScoped<IRepository<Specialization>, EFRepository<Specialization, DBRContext>>();
		services.AddScoped<IRepository<Workshop>, EFRepository<Workshop, DBRContext>>();
		services.AddScoped<IRepository<Vehicle>, EFRepository<Vehicle, DBRContext>>();
		services.AddScoped<IRepository<Customer>, EFRepository<Customer, DBRContext>>();
		services.AddScoped<IRepository<Case>, EFRepository<Case, DBRContext>>();
		services.AddScoped<IRepository<Incident>, EFRepository<Incident, DBRContext>>();
		services.AddScoped<IRepository<Image>, EFRepository<Image, DBRContext>>();
		services.AddScoped<IRepository<Invoice>, EFRepository<Invoice, DBRContext>>();
		services.AddScoped<IRepository<Video>, EFRepository<Video, DBRContext>>();
		services.AddScoped<IRepository<Attachment>, EFRepository<Attachment, DBRContext>>();
		services.AddScoped<IRepository<Member>, EFRepository<Member, DBRContext>>();
		services.AddScoped<ISaveChangesService, SaveChangesService<DBRContext>>();

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
		services.AddScoped<IService<Member, Member, MemberDTO>, Service<Member, Member, MemberDTO>>();

		services.AddScoped<IAuthService, AuthService>();

		return services;
	}
}