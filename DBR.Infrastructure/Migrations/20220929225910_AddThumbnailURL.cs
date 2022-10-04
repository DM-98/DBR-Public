using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBR.Infrastructure.Migrations;

public partial class AddThumbnailURL : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "ThumbnailURL",
			table: "Videos",
			type: "nvarchar(150)",
			maxLength: 150,
			nullable: false,
			defaultValue: "");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "ThumbnailURL",
			table: "Videos");
	}
}