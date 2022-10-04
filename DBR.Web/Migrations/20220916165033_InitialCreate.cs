using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBR.Web.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Addresses",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					City = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					PostCode = table.Column<short>(type: "smallint", nullable: false),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Addresses", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Images",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ImageURI = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Images", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Invoices",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					InvoiceURI = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Invoices", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Roles",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Roles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Specializations",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Specializations", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Vehicles",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LicensePlate = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
					VIN = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					Brand = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
					Year = table.Column<short>(type: "smallint", nullable: false),
					Kilometers = table.Column<int>(type: "int", nullable: true),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Vehicles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Videos",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					IsVideoReady = table.Column<bool>(type: "bit", nullable: false),
					ThumbnailURL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
					ProgressiveDownloadURL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
					HLSStreamingURL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
					DASHStreamingURL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
					SmoothStreamingURL = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
					TransformName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					InputAssetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					OutputAssetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					JobName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					LocatorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Videos", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Workshops",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Workshops", x => x.Id);
					table.ForeignKey(
						name: "FK_Workshops_Addresses_AddressId",
						column: x => x.AddressId,
						principalTable: "Addresses",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "RoleClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RoleClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_RoleClaims_Roles_RoleId",
						column: x => x.RoleId,
						principalTable: "Roles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Customers",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
					Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
					VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Customers", x => x.Id);
					table.ForeignKey(
						name: "FK_Customers_Vehicles_VehicleId",
						column: x => x.VehicleId,
						principalTable: "Vehicles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Members",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					WorkshopId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					IsTermsAccepted = table.Column<bool>(type: "bit", nullable: false),
					RefreshToken = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
					RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
					UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
					PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
					TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
					LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
					AccessFailedCount = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Members", x => x.Id);
					table.ForeignKey(
						name: "FK_Members_Workshops_WorkshopId",
						column: x => x.WorkshopId,
						principalTable: "Workshops",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "SpecializationWorkshop",
				columns: table => new
				{
					SpecializationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					WorkshopsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SpecializationWorkshop", x => new { x.SpecializationsId, x.WorkshopsId });
					table.ForeignKey(
						name: "FK_SpecializationWorkshop_Specializations_SpecializationsId",
						column: x => x.SpecializationsId,
						principalTable: "Specializations",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_SpecializationWorkshop_Workshops_WorkshopsId",
						column: x => x.WorkshopsId,
						principalTable: "Workshops",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Cases",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					WorkshopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Status = table.Column<int>(type: "int", nullable: false),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Cases", x => x.Id);
					table.ForeignKey(
						name: "FK_Cases_Customers_CustomerId",
						column: x => x.CustomerId,
						principalTable: "Customers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Cases_Workshops_WorkshopId",
						column: x => x.WorkshopId,
						principalTable: "Workshops",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "MemberClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MemberClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_MemberClaims_Members_UserId",
						column: x => x.UserId,
						principalTable: "Members",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "MemberLogins",
				columns: table => new
				{
					LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MemberLogins", x => new { x.LoginProvider, x.ProviderKey });
					table.ForeignKey(
						name: "FK_MemberLogins_Members_UserId",
						column: x => x.UserId,
						principalTable: "Members",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "MemberRoles",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MemberRoles", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_MemberRoles_Members_UserId",
						column: x => x.UserId,
						principalTable: "Members",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_MemberRoles_Roles_RoleId",
						column: x => x.RoleId,
						principalTable: "Roles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "MemberTokens",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MemberTokens", x => new { x.UserId, x.LoginProvider, x.Name });
					table.ForeignKey(
						name: "FK_MemberTokens_Members_UserId",
						column: x => x.UserId,
						principalTable: "Members",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Incidents",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ShortId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
					Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
					Type = table.Column<int>(type: "int", nullable: false),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Incidents", x => x.Id);
					table.ForeignKey(
						name: "FK_Incidents_Cases_CaseId",
						column: x => x.CaseId,
						principalTable: "Cases",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Attachments",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					IncidentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					Type = table.Column<int>(type: "int", nullable: false),
					RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Attachments", x => x.Id);
					table.ForeignKey(
						name: "FK_Attachments_Images_ImageId",
						column: x => x.ImageId,
						principalTable: "Images",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Attachments_Incidents_IncidentId",
						column: x => x.IncidentId,
						principalTable: "Incidents",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Attachments_Invoices_InvoiceId",
						column: x => x.InvoiceId,
						principalTable: "Invoices",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Attachments_Videos_VideoId",
						column: x => x.VideoId,
						principalTable: "Videos",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_Attachments_ImageId",
				table: "Attachments",
				column: "ImageId");

			migrationBuilder.CreateIndex(
				name: "IX_Attachments_IncidentId",
				table: "Attachments",
				column: "IncidentId");

			migrationBuilder.CreateIndex(
				name: "IX_Attachments_InvoiceId",
				table: "Attachments",
				column: "InvoiceId");

			migrationBuilder.CreateIndex(
				name: "IX_Attachments_VideoId",
				table: "Attachments",
				column: "VideoId");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_CustomerId",
				table: "Cases",
				column: "CustomerId");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_WorkshopId",
				table: "Cases",
				column: "WorkshopId");

			migrationBuilder.CreateIndex(
				name: "IX_Customers_VehicleId",
				table: "Customers",
				column: "VehicleId");

			migrationBuilder.CreateIndex(
				name: "IX_Incidents_CaseId",
				table: "Incidents",
				column: "CaseId");

			migrationBuilder.CreateIndex(
				name: "IX_MemberClaims_UserId",
				table: "MemberClaims",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_MemberLogins_UserId",
				table: "MemberLogins",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_MemberRoles_RoleId",
				table: "MemberRoles",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "EmailIndex",
				table: "Members",
				column: "NormalizedEmail");

			migrationBuilder.CreateIndex(
				name: "IX_Members_WorkshopId",
				table: "Members",
				column: "WorkshopId");

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "Members",
				column: "NormalizedUserName",
				unique: true,
				filter: "[NormalizedUserName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_RoleClaims_RoleId",
				table: "RoleClaims",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "Roles",
				column: "NormalizedName",
				unique: true,
				filter: "[NormalizedName] IS NOT NULL");

			migrationBuilder.CreateIndex(
				name: "IX_SpecializationWorkshop_WorkshopsId",
				table: "SpecializationWorkshop",
				column: "WorkshopsId");

			migrationBuilder.CreateIndex(
				name: "IX_Workshops_AddressId",
				table: "Workshops",
				column: "AddressId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Attachments");

			migrationBuilder.DropTable(
				name: "MemberClaims");

			migrationBuilder.DropTable(
				name: "MemberLogins");

			migrationBuilder.DropTable(
				name: "MemberRoles");

			migrationBuilder.DropTable(
				name: "MemberTokens");

			migrationBuilder.DropTable(
				name: "RoleClaims");

			migrationBuilder.DropTable(
				name: "SpecializationWorkshop");

			migrationBuilder.DropTable(
				name: "Images");

			migrationBuilder.DropTable(
				name: "Incidents");

			migrationBuilder.DropTable(
				name: "Invoices");

			migrationBuilder.DropTable(
				name: "Videos");

			migrationBuilder.DropTable(
				name: "Members");

			migrationBuilder.DropTable(
				name: "Roles");

			migrationBuilder.DropTable(
				name: "Specializations");

			migrationBuilder.DropTable(
				name: "Cases");

			migrationBuilder.DropTable(
				name: "Customers");

			migrationBuilder.DropTable(
				name: "Workshops");

			migrationBuilder.DropTable(
				name: "Vehicles");

			migrationBuilder.DropTable(
				name: "Addresses");
		}
	}
}