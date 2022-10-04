using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBR.Infrastructure.Migrations;

public partial class AttachmentURLChange : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "DASHStreamingURL",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "HLSStreamingURL",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "InputAssetName",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "IsVideoReady",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "JobName",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "LocatorName",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "OutputAssetName",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "ProgressiveDownloadURL",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "SmoothStreamingURL",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "ThumbnailURL",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "TransformName",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "InvoiceURI",
			table: "Invoices");

		migrationBuilder.DropColumn(
			name: "ImageURI",
			table: "Images");

		migrationBuilder.AddColumn<string>(
			name: "VideoURL",
			table: "Videos",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: false,
			defaultValue: "");

		migrationBuilder.AddColumn<string>(
			name: "InvoiceURL",
			table: "Invoices",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: false,
			defaultValue: "");

		migrationBuilder.AddColumn<string>(
			name: "ImageURL",
			table: "Images",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: false,
			defaultValue: "");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "VideoURL",
			table: "Videos");

		migrationBuilder.DropColumn(
			name: "InvoiceURL",
			table: "Invoices");

		migrationBuilder.DropColumn(
			name: "ImageURL",
			table: "Images");

		migrationBuilder.AddColumn<string>(
			name: "DASHStreamingURL",
			table: "Videos",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: true);

		migrationBuilder.AddColumn<string>(
			name: "HLSStreamingURL",
			table: "Videos",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: true);

		migrationBuilder.AddColumn<string>(
			name: "InputAssetName",
			table: "Videos",
			type: "nvarchar(50)",
			maxLength: 50,
			nullable: false,
			defaultValue: "");

		migrationBuilder.AddColumn<bool>(
			name: "IsVideoReady",
			table: "Videos",
			type: "bit",
			nullable: false,
			defaultValue: false);

		migrationBuilder.AddColumn<string>(
			name: "JobName",
			table: "Videos",
			type: "nvarchar(50)",
			maxLength: 50,
			nullable: false,
			defaultValue: "");

		migrationBuilder.AddColumn<string>(
			name: "LocatorName",
			table: "Videos",
			type: "nvarchar(50)",
			maxLength: 50,
			nullable: false,
			defaultValue: "");

		migrationBuilder.AddColumn<string>(
			name: "OutputAssetName",
			table: "Videos",
			type: "nvarchar(50)",
			maxLength: 50,
			nullable: false,
			defaultValue: "");

		migrationBuilder.AddColumn<string>(
			name: "ProgressiveDownloadURL",
			table: "Videos",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: true);

		migrationBuilder.AddColumn<string>(
			name: "SmoothStreamingURL",
			table: "Videos",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: true);

		migrationBuilder.AddColumn<string>(
			name: "ThumbnailURL",
			table: "Videos",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: true);

		migrationBuilder.AddColumn<string>(
			name: "TransformName",
			table: "Videos",
			type: "nvarchar(50)",
			maxLength: 50,
			nullable: false,
			defaultValue: "");

		migrationBuilder.AddColumn<string>(
			name: "InvoiceURI",
			table: "Invoices",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: true);

		migrationBuilder.AddColumn<string>(
			name: "ImageURI",
			table: "Images",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: true);
	}
}