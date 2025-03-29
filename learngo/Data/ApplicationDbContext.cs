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
            // Настройка первичных ключей с автоинкрементом
            modelBuilder.Entity<Section>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Topic>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            // Настройка внешнего ключа
            modelBuilder.Entity<Topic>()
                .HasOne(t => t.Section)
                .WithMany(s => s.Topics)
                .HasForeignKey(t => t.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка столбца created_at
            modelBuilder.Entity<Topic>()
                .Property(t => t.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Начальные данные для Sections (разделы, связанные с Go)
            modelBuilder.Entity<Section>().HasData(
                new Section { Id = 1, SectionName = "Основы Go" },
                new Section { Id = 2, SectionName = "Конкурентность в Go" },
                new Section { Id = 3, SectionName = "Веб-разработка на Go" }
            );

            // Начальные данные для Topics (по 3 темы в каждом разделе)
            modelBuilder.Entity<Topic>().HasData(
                // Раздел "Основы Go"
                new Topic
                {
                    Id = 1,
                    SectionId = 1,
                    TopicName = "Введение в Go",
                    Description = "Основы синтаксиса и установка Go",
                    Difficulty = 1,
                    TimeLimit = 60,
                    AuthorName = "Алексей Смирнов",
                    AuthorEmail = "alex@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-01"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 2,
                    SectionId = 1,
                    TopicName = "Переменные и типы данных",
                    Description = "Работа с переменными и базовыми типами в Go",
                    Difficulty = 1,
                    TimeLimit = 45,
                    AuthorName = "Мария Петрова",
                    AuthorEmail = "maria@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-02"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 3,
                    SectionId = 1,
                    TopicName = "Структуры и интерфейсы",
                    Description = "Использование структур и интерфейсов в Go",
                    Difficulty = 2,
                    TimeLimit = 90,
                    AuthorName = "Дмитрий Иванов",
                    AuthorEmail = "dmitry@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-03"), DateTimeKind.Utc)
                },

                // Раздел "Конкурентность в Go"
                new Topic
                {
                    Id = 4,
                    SectionId = 2,
                    TopicName = "Горутины",
                    Description = "Основы конкурентности с использованием горутин",
                    Difficulty = 2,
                    TimeLimit = 75,
                    AuthorName = "Елена Соколова",
                    AuthorEmail = "elena@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-04"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 5,
                    SectionId = 2,
                    TopicName = "Каналы",
                    Description = "Синхронизация с помощью каналов в Go",
                    Difficulty = 3,
                    TimeLimit = 90,
                    AuthorName = "Сергей Кузнецов",
                    AuthorEmail = "sergey@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-05"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 6,
                    SectionId = 2,
                    TopicName = "Паттерн Worker Pool",
                    Description = "Реализация пула работников с горутинами",
                    Difficulty = 3,
                    TimeLimit = 120,
                    AuthorName = "Ольга Николаева",
                    AuthorEmail = "olga@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-06"), DateTimeKind.Utc)
                },

                // Раздел "Веб-разработка на Go"
                new Topic
                {
                    Id = 7,
                    SectionId = 3,
                    TopicName = "Создание REST API",
                    Description = "Разработка RESTful API с пакетом net/http",
                    Difficulty = 2,
                    TimeLimit = 120,
                    AuthorName = "Игорь Васильев",
                    AuthorEmail = "igor@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-07"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 8,
                    SectionId = 3,
                    TopicName = "Работа с Gin",
                    Description = "Создание веб-приложений с фреймворком Gin",
                    Difficulty = 2,
                    TimeLimit = 90,
                    AuthorName = "Татьяна Морозова",
                    AuthorEmail = "tatiana@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-08"), DateTimeKind.Utc)
                },
                new Topic
                {
                    Id = 9,
                    SectionId = 3,
                    TopicName = "Middleware в Go",
                    Description = "Использование промежуточного ПО в веб-приложениях",
                    Difficulty = 3,
                    TimeLimit = 105,
                    AuthorName = "Павел Соколов",
                    AuthorEmail = "pavel@example.com",
                    CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-03-09"), DateTimeKind.Utc)
                }
            );
        }
    }
}