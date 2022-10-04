using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBR.Infrastructure.Migrations;

public partial class NullableCreatedDate : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "ThumbnailURL",
			table: "Videos");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Workshops",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Videos",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Vehicles",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Specializations",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Members",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Invoices",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Incidents",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Images",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Customers",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Cases",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Attachments",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Addresses",
			type: "datetime2",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Workshops",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Videos",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AddColumn<string>(
			name: "ThumbnailURL",
			table: "Videos",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: false,
			defaultValue: "");

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Vehicles",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Specializations",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Members",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Invoices",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Incidents",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Images",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Customers",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Cases",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Attachments",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);

		migrationBuilder.AlterColumn<DateTime>(
			name: "CreatedDate",
			table: "Addresses",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			oldClrType: typeof(DateTime),
			oldType: "datetime2",
			oldNullable: true);
	}
}