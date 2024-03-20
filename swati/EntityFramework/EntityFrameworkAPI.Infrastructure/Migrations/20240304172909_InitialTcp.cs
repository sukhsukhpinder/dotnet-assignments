using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkAPI.Infrastructure.Migrations
{
    public partial class InitialTcp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentStatuses_Statuses_StatusId",
                table: "StudentStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentStatuses_Student_StudentId",
                table: "StudentStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_Student_StudentId",
                table: "StudentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_Subjects_SubjectId",
                table: "StudentSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentStatuses",
                table: "StudentStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.RenameTable(
                name: "StudentSubjects",
                newName: "StudentSubject");

            migrationBuilder.RenameTable(
                name: "StudentStatuses",
                newName: "StudentStatus");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "Statuse");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubjects_SubjectId",
                table: "StudentSubject",
                newName: "IX_StudentSubject_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubjects_StudentId",
                table: "StudentSubject",
                newName: "IX_StudentSubject_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentStatuses_StudentId",
                table: "StudentStatus",
                newName: "IX_StudentStatus_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentStatuses_StatusId",
                table: "StudentStatus",
                newName: "IX_StudentStatus_StatusId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Subject",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Subject",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Statuse",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Statuse",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Statuse",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Statuse",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subject",
                table: "Subject",
                column: "SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSubject",
                table: "StudentSubject",
                column: "StudentSubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentStatus",
                table: "StudentStatus",
                column: "StudentStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuse",
                table: "Statuse",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentStatus_Statuse_StatusId",
                table: "StudentStatus",
                column: "StatusId",
                principalTable: "Statuse",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentStatus_Student_StudentId",
                table: "StudentStatus",
                column: "StudentId",
                principalSchema: "dbo",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Student_StudentId",
                table: "StudentSubject",
                column: "StudentId",
                principalSchema: "dbo",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Subject_SubjectId",
                table: "StudentSubject",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentStatus_Statuse_StatusId",
                table: "StudentStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentStatus_Student_StudentId",
                table: "StudentStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Student_StudentId",
                table: "StudentSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Subject_SubjectId",
                table: "StudentSubject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subject",
                table: "Subject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSubject",
                table: "StudentSubject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentStatus",
                table: "StudentStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuse",
                table: "Statuse");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Statuse");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Statuse");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Statuse");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Statuse");

            migrationBuilder.RenameTable(
                name: "Subject",
                newName: "Subjects");

            migrationBuilder.RenameTable(
                name: "StudentSubject",
                newName: "StudentSubjects");

            migrationBuilder.RenameTable(
                name: "StudentStatus",
                newName: "StudentStatuses");

            migrationBuilder.RenameTable(
                name: "Statuse",
                newName: "Statuses");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubject_SubjectId",
                table: "StudentSubjects",
                newName: "IX_StudentSubjects_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubject_StudentId",
                table: "StudentSubjects",
                newName: "IX_StudentSubjects_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentStatus_StudentId",
                table: "StudentStatuses",
                newName: "IX_StudentStatuses_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentStatus_StatusId",
                table: "StudentStatuses",
                newName: "IX_StudentStatuses_StatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects",
                column: "StudentSubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentStatuses",
                table: "StudentStatuses",
                column: "StudentStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentStatuses_Statuses_StatusId",
                table: "StudentStatuses",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentStatuses_Student_StudentId",
                table: "StudentStatuses",
                column: "StudentId",
                principalSchema: "dbo",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_Student_StudentId",
                table: "StudentSubjects",
                column: "StudentId",
                principalSchema: "dbo",
                principalTable: "Student",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_Subjects_SubjectId",
                table: "StudentSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
