using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ��������� ��������� ������ � ���������������
            modelBuilder.Entity<Section>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Topic>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            // ��������� �������� �����
            modelBuilder.Entity<Topic>()
                .HasOne(t => t.Section)
                .WithMany(s => s.Topics)
                .HasForeignKey(t => t.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ��������� ������� created_at
            modelBuilder.Entity<Topic>()
                .Property(t => t.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // ��������� ������ ��� Sections (�������, ��������� � Go)
            modelBuilder.Entity<Section>().HasData(
                new Section { Id = 1, SectionName = "������ Go" },
                new Section { Id = 2, SectionName = "�������������� � Go" },
                new Section { Id = 3, SectionName = "���-���������� �� Go" }
            );

            // ��������� ������ ��� Topics (�� 3 ���� � ������ �������)
            modelBuilder.Entity<Topic>().HasData(
                // ������ "������ Go"
                new Topic
                {
                    Id = 1,
                    SectionId = 1,
                    TopicName = "�������� � Go",
                    Description = "������ ���������� � ��������� Go",
                    Difficulty = 1,
                    TimeLimit = 60,
                    AuthorName = "������� �������",
                    AuthorEmail = "alex@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-01"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 2,
                    SectionId = 1,
                    TopicName = "���������� � ���� ������",
                    Description = "������ � ����������� � �������� ������ � Go",
                    Difficulty = 1,
                    TimeLimit = 45,
                    AuthorName = "����� �������",
                    AuthorEmail = "maria@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-02"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 3,
                    SectionId = 1,
                    TopicName = "��������� � ����������",
                    Description = "������������� �������� � ����������� � Go",
                    Difficulty = 2,
                    TimeLimit = 90,
                    AuthorName = "������� ������",
                    AuthorEmail = "dmitry@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-03"), DateTimeKind.Utc)
                },

                // ������ "�������������� � Go"
                new Topic
                {
                    Id = 4,
                    SectionId = 2,
                    TopicName = "��������",
                    Description = "������ �������������� � �������������� �������",
                    Difficulty = 2,
                    TimeLimit = 75,
                    AuthorName = "����� ��������",
                    AuthorEmail = "elena@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-04"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 5,
                    SectionId = 2,
                    TopicName = "������",
                    Description = "������������� � ������� ������� � Go",
                    Difficulty = 3,
                    TimeLimit = 90,
                    AuthorName = "������ ��������",
                    AuthorEmail = "sergey@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-05"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 6,
                    SectionId = 2,
                    TopicName = "������� Worker Pool",
                    Description = "���������� ���� ���������� � ����������",
                    Difficulty = 3,
                    TimeLimit = 120,
                    AuthorName = "����� ���������",
                    AuthorEmail = "olga@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-06"), DateTimeKind.Utc)
                },

                // ������ "���-���������� �� Go"
                new Topic
                {
                    Id = 7,
                    SectionId = 3,
                    TopicName = "�������� REST API",
                    Description = "���������� RESTful API � ������� net/http",
                    Difficulty = 2,
                    TimeLimit = 120,
                    AuthorName = "����� ��������",
                    AuthorEmail = "igor@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-07"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 8,
                    SectionId = 3,
                    TopicName = "������ � Gin",
                    Description = "�������� ���-���������� � ����������� Gin",
                    Difficulty = 2,
                    TimeLimit = 90,
                    AuthorName = "������� ��������",
                    AuthorEmail = "tatiana@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-08"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 9,
                    SectionId = 3,
                    TopicName = "Middleware � Go",
                    Description = "������������� �������������� �� � ���-�����������",
                    Difficulty = 3,
                    TimeLimit = 105,
                    AuthorName = "����� �������",
                    AuthorEmail = "pavel@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-09"), DateTimeKind.Utc)
                }
            );
        }
    }
}