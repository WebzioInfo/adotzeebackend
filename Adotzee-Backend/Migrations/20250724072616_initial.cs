using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adotzee_Backend.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colleges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colleges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stream = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddonCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddonCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddonCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AddonColleges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddonCourseId = table.Column<int>(type: "int", nullable: false),
                    CollegeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddonColleges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddonColleges_AddonCourses_AddonCourseId",
                        column: x => x.AddonCourseId,
                        principalTable: "AddonCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddonColleges_Colleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddonColleges_AddonCourseId",
                table: "AddonColleges",
                column: "AddonCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AddonColleges_CollegeId",
                table: "AddonColleges",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_AddonCourses_CourseId",
                table: "AddonCourses",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddonColleges");

            migrationBuilder.DropTable(
                name: "AddonCourses");

            migrationBuilder.DropTable(
                name: "Colleges");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
