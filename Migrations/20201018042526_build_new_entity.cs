using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace main_service.Migrations
{
    public partial class build_new_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckingItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    VehicleGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckingItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckingItem_VehicleGroup_VehicleGroupId",
                        column: x => x.VehicleGroupId,
                        principalTable: "VehicleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FcmToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    Token = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FcmToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FcmToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    VehicleGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceItem_VehicleGroup_VehicleGroupId",
                        column: x => x.VehicleGroupId,
                        principalTable: "VehicleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Activity = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatusChecks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusChecks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserVehicle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    VehicleGroupId = table.Column<int>(nullable: false),
                    ChassisNumber = table.Column<string>(nullable: true),
                    EngineNumber = table.Column<string>(nullable: true),
                    PlateNumber = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVehicle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVehicle_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVehicle_VehicleGroup_UserId",
                        column: x => x.UserId,
                        principalTable: "VehicleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandItemPrice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LaborCost = table.Column<int>(nullable: false),
                    SparePartPrice = table.Column<int>(nullable: false),
                    MaintenanceItemId = table.Column<int>(nullable: false),
                    BranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandItemPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandItemPrice_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandItemPrice_MaintenanceItem_MaintenanceItemId",
                        column: x => x.MaintenanceItemId,
                        principalTable: "MaintenanceItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    Odometer = table.Column<int>(nullable: false),
                    UserVehicleId = table.Column<int>(nullable: false),
                    ReceptionStaffId = table.Column<int>(nullable: false),
                    MaintenanceStaffId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maintenance_User_MaintenanceStaffId",
                        column: x => x.MaintenanceStaffId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Maintenance_User_ReceptionStaffId",
                        column: x => x.ReceptionStaffId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Maintenance_UserVehicle_UserVehicleId",
                        column: x => x.UserVehicleId,
                        principalTable: "UserVehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UserVehicleId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Odometer = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceSchedule_UserVehicle_UserVehicleId",
                        column: x => x.UserVehicleId,
                        principalTable: "UserVehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceCheckings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CheckingItemId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    MaintenanceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceCheckings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceCheckings_CheckingItem_CheckingItemId",
                        column: x => x.CheckingItemId,
                        principalTable: "CheckingItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceCheckings_Maintenance_MaintenanceId",
                        column: x => x.MaintenanceId,
                        principalTable: "Maintenance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MaintenanceCheckings_StatusChecks_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    MaintenanceId = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceImage_Maintenance_MaintenanceId",
                        column: x => x.MaintenanceId,
                        principalTable: "Maintenance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifyDate", "RoleName" },
                values: new object[] { 5, null, null, null, "User" });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleGroup_VehicleCompanyId",
                table: "VehicleGroup",
                column: "VehicleCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleGroup_VehicleTypeId",
                table: "VehicleGroup",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_BranchId",
                table: "User",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandItemPrice_BranchId",
                table: "BrandItemPrice",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandItemPrice_MaintenanceItemId",
                table: "BrandItemPrice",
                column: "MaintenanceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckingItem_VehicleGroupId",
                table: "CheckingItem",
                column: "VehicleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FcmToken_UserId",
                table: "FcmToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_MaintenanceStaffId",
                table: "Maintenance",
                column: "MaintenanceStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_ReceptionStaffId",
                table: "Maintenance",
                column: "ReceptionStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_UserVehicleId",
                table: "Maintenance",
                column: "UserVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceCheckings_CheckingItemId",
                table: "MaintenanceCheckings",
                column: "CheckingItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceCheckings_MaintenanceId",
                table: "MaintenanceCheckings",
                column: "MaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceCheckings_StatusId",
                table: "MaintenanceCheckings",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceImage_MaintenanceId",
                table: "MaintenanceImage",
                column: "MaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceItem_VehicleGroupId",
                table: "MaintenanceItem",
                column: "VehicleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceSchedule_UserVehicleId",
                table: "MaintenanceSchedule",
                column: "UserVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVehicle_PlateNumber",
                table: "UserVehicle",
                column: "PlateNumber");

            migrationBuilder.CreateIndex(
                name: "IX_UserVehicle_UserId",
                table: "UserVehicle",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Branch_BranchId",
                table: "User",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleGroup_VehicleCompany_VehicleCompanyId",
                table: "VehicleGroup",
                column: "VehicleCompanyId",
                principalTable: "VehicleCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleGroup_VehicleType_VehicleTypeId",
                table: "VehicleGroup",
                column: "VehicleTypeId",
                principalTable: "VehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Branch_BranchId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleGroup_VehicleCompany_VehicleCompanyId",
                table: "VehicleGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleGroup_VehicleType_VehicleTypeId",
                table: "VehicleGroup");

            migrationBuilder.DropTable(
                name: "BrandItemPrice");

            migrationBuilder.DropTable(
                name: "FcmToken");

            migrationBuilder.DropTable(
                name: "MaintenanceCheckings");

            migrationBuilder.DropTable(
                name: "MaintenanceImage");

            migrationBuilder.DropTable(
                name: "MaintenanceSchedule");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "MaintenanceItem");

            migrationBuilder.DropTable(
                name: "CheckingItem");

            migrationBuilder.DropTable(
                name: "StatusChecks");

            migrationBuilder.DropTable(
                name: "Maintenance");

            migrationBuilder.DropTable(
                name: "UserVehicle");

            migrationBuilder.DropIndex(
                name: "IX_VehicleGroup_VehicleCompanyId",
                table: "VehicleGroup");

            migrationBuilder.DropIndex(
                name: "IX_VehicleGroup_VehicleTypeId",
                table: "VehicleGroup");

            migrationBuilder.DropIndex(
                name: "IX_User_BranchId",
                table: "User");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "User");
        }
    }
}
