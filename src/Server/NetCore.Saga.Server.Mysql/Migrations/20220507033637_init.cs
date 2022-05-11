using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCore.Saga.Server.Mysql.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompensationTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "nvarchar(24)", nullable: false, comment: "类型"),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false, comment: "时间戳"),
                    GlobalId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "全局id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocalId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "本地id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "方法类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImplementMethod = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "实现的方法")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Retries = table.Column<int>(type: "int", nullable: false, comment: "重试次数"),
                    Payloads = table.Column<byte[]>(type: "longblob", nullable: false, comment: "参数"),
                    ServiceName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "服务名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ServiceId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "服务id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompensableMethod = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "补偿的方法")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExceptionMessage = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "异常信息")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompensationTable", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "nvarchar(24)", nullable: false, comment: "类型"),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false, comment: "时间戳"),
                    GlobalId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "全局id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocalId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "本地id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "方法类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImplementMethod = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "实现的方法")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Payloads = table.Column<byte[]>(type: "longblob", nullable: false, comment: "参数"),
                    ServiceName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "服务名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ServiceId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "服务id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompensableMethod = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "补偿的方法")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Retries = table.Column<int>(type: "int", nullable: false, comment: "重试次数"),
                    ExceptionMessage = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "异常信息")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTable", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompensationTable");

            migrationBuilder.DropTable(
                name: "EventTable");
        }
    }
}
