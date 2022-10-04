using DBR.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DBR.Web.Context;

public class DBRContext : IdentityDbContext<Member, IdentityRole<Guid>, Guid>
{
	public DbSet<Address>? Addresses { get; set; }

	public DbSet<Case>? Cases { get; set; }

	public DbSet<Customer>? Customers { get; set; }

	public DbSet<Image>? Images { get; set; }

	public DbSet<Incident>? Incidents { get; set; }

	public DbSet<Attachment>? IncidentAttachments { get; set; }

	public DbSet<Invoice>? Invoices { get; set; }

	public DbSet<Specialization>? Specializations { get; set; }

	public DbSet<Vehicle>? Vehicles { get; set; }

	public DbSet<Video>? Videos { get; set; }

	public DbSet<Workshop>? Workshops { get; set; }

	public DBRContext(DbContextOptions<DBRContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<IdentityUserClaim<Guid>>().ToTable("MemberClaims");
		builder.Entity<IdentityUserRole<Guid>>().ToTable("MemberRoles");
		builder.Entity<IdentityUserLogin<Guid>>().ToTable("MemberLogins");
		builder.Entity<IdentityUserToken<Guid>>().ToTable("MemberTokens");
		builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
		builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
		builder.Entity<Member>().ToTable("Members");
	}
}