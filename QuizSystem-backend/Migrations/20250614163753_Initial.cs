using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizSystem_backend.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    department_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RoomExams",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomExams", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    class_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    department_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Classes_Departments_department_id",
                        column: x => x.department_id,
                        principalTable: "Departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    department_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.id);
                    table.ForeignKey(
                        name: "FK_Subjects_Departments_department_id",
                        column: x => x.department_id,
                        principalTable: "Departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    gender = table.Column<bool>(type: "bit", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    avatar_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_role_id",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    exam_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    duration_minutes = table.Column<int>(type: "int", nullable: false),
                    number_of_questions = table.Column<int>(type: "int", nullable: false),
                    total_score = table.Column<double>(type: "float", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    room_exam_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.id);
                    table.ForeignKey(
                        name: "FK_Exams_RoomExams_room_exam_id",
                        column: x => x.room_exam_id,
                        principalTable: "RoomExams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exams_Subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionBank_Subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomExamSubjects",
                columns: table => new
                {
                    room_exam_id = table.Column<int>(type: "int", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    exam_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomExamSubjects", x => new { x.room_exam_id, x.subject_id });
                    table.ForeignKey(
                        name: "FK_RoomExamSubjects_RoomExams_room_exam_id",
                        column: x => x.room_exam_id,
                        principalTable: "RoomExams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomExamSubjects_Subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    student_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_first_time_login = table.Column<bool>(type: "bit", nullable: false),
                    class_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.id);
                    table.ForeignKey(
                        name: "FK_Students_Classes_class_id",
                        column: x => x.class_id,
                        principalTable: "Classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Users_id",
                        column: x => x.id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    teacher_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_first_time_login = table.Column<bool>(type: "bit", nullable: false),
                    department_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Teachers_Departments_department_id",
                        column: x => x.department_id,
                        principalTable: "Departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_Users_id",
                        column: x => x.id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    difficulty = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    QuestionBankId = table.Column<int>(type: "int", nullable: false),
                    topic = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionBank_QuestionBankId",
                        column: x => x.QuestionBankId,
                        principalTable: "QuestionBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_Teachers_created_by",
                        column: x => x.created_by,
                        principalTable: "Teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSubjectClasses",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    class_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSubjectClasses", x => new { x.TeacherId, x.subject_id, x.class_id });
                    table.ForeignKey(
                        name: "FK_TeacherSubjectClasses_Classes_class_id",
                        column: x => x.class_id,
                        principalTable: "Classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherSubjectClasses_Subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "Subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherSubjectClasses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    is_correct = table.Column<bool>(type: "bit", nullable: false),
                    answer_order = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    question_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_question_id",
                        column: x => x.question_id,
                        principalTable: "Questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamQuestions",
                columns: table => new
                {
                    exam_id = table.Column<int>(type: "int", nullable: false),
                    question_id = table.Column<int>(type: "int", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    score = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamQuestions", x => new { x.exam_id, x.question_id });
                    table.ForeignKey(
                        name: "FK_ExamQuestions_Exams_exam_id",
                        column: x => x.exam_id,
                        principalTable: "Exams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamQuestions_Questions_question_id",
                        column: x => x.question_id,
                        principalTable: "Questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_question_id",
                table: "Answers",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_class_code",
                table: "Classes",
                column: "class_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_department_id",
                table: "Classes",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_department_code",
                table: "Departments",
                column: "department_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_question_id",
                table: "ExamQuestions",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_exam_code",
                table: "Exams",
                column: "exam_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_room_exam_id",
                table: "Exams",
                column: "room_exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_subject_id",
                table: "Exams",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionBank_subject_id",
                table: "QuestionBank",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_created_by",
                table: "Questions",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionBankId",
                table: "Questions",
                column: "QuestionBankId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomExamSubjects_subject_id",
                table: "RoomExamSubjects",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_Students_class_id",
                table: "Students",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_department_id",
                table: "Subjects",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_department_id",
                table: "Teachers",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjectClasses_class_id",
                table: "TeacherSubjectClasses",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjectClasses_subject_id",
                table: "TeacherSubjectClasses",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_id",
                table: "Users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_username",
                table: "Users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "ExamQuestions");

            migrationBuilder.DropTable(
                name: "RoomExamSubjects");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "TeacherSubjectClasses");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "RoomExams");

            migrationBuilder.DropTable(
                name: "QuestionBank");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
