//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Diagnostics;
//using PeerEvalAppAPI.Security;

//namespace PeerEvalAppAPI.Data
//{
//    public class PeerEvalAppDbContext : DbContext
//    {
//        protected PeerEvalAppDbContext()
//        {
//        }

//        public PeerEvalAppDbContext(DbContextOptions options) : base(options)
//        {
//        }

//        public DbSet<User> Users { get; set; }
//        public DbSet<Evaluation> Evaluations { get; set; }
//        public DbSet<EvaluationAnswer> EvaluationsAnswers { get; set; }
//        public DbSet<EvaluationCycle> EvaluationsCycles { get; set; }
//        public DbSet<Group> Groups { get; set; }
//        public DbSet<Question> Questions { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {

//            modelBuilder.Entity<Group>(entity =>
//            {
//                entity.ToTable("Groups");
//                entity.HasKey(g => g.Id);

//                entity.Property(g => g.GroupName)
//                      .HasMaxLength(128)
//                      .IsRequired(true);
//                entity.HasMany(g => g.Users)
//                      .WithOne(u => u.Group)
//                      .HasForeignKey(u => u.GroupId)
//                      .OnDelete(DeleteBehavior.Restrict);

//                // From Base Entity
//                entity.Property(e => e.InsertedAt)
//                       .ValueGeneratedOnAdd()
//                       .HasDefaultValueSql("GETDATE()");
//                entity.Property(e => e.ModifiedAt)
//                      .ValueGeneratedOnAddOrUpdate()
//                      .HasDefaultValueSql("GETDATE()");

//                entity.HasData(
//                    new Group
//                    {
//                        Id = 1,
//                        GroupName = "Admins"
//                    }
//                    );
//            });

//            modelBuilder.Entity<User>(static entity =>
//            {
//                entity.ToTable("Users");
//                entity.HasKey(e => e.Id);

//                entity.Property(e => e.FirstName)
//                      .HasMaxLength(255)
//                      .IsRequired();
//                entity.Property(e => e.LastName)
//                      .HasMaxLength(255)
//                      .IsRequired();
//                entity.Property(e => e.Email)
//                      .HasMaxLength(255)
//                      .IsRequired();
//                entity.Property(e => e.Password)
//                      .HasMaxLength(255)
//                      .IsRequired();
//                // Index Email
//                entity.HasIndex(e => e.Email)
//                      .IsUnique();
//                // Enum to String
//                entity.Property(e => e.Role)
//                      .HasConversion<string>()
//                      .HasMaxLength(50)
//                      .IsRequired();
//                // self-reference: User -> Manager
//                entity.HasOne(u => u.Manager)
//                      .WithMany(m => m.Subordinates)
//                      .HasForeignKey(u => u.ManagerId)
//                      .OnDelete(DeleteBehavior.Restrict);
//                entity.HasOne(u => u.Group)
//                      .WithMany(g => g.Users)
//                      .HasForeignKey(u => u.GroupId)
//                      .OnDelete(DeleteBehavior.Restrict);

//                // From Base Entity
//                entity.Property(e => e.InsertedAt)
//                       .ValueGeneratedOnAdd()
//                       .HasDefaultValueSql("GETDATE()");
//                entity.Property(e => e.ModifiedAt)
//                      .ValueGeneratedOnAddOrUpdate()
//                      .HasDefaultValueSql("GETDATE()");

//                entity.HasData(
//                    new User
//                    {
//                        Id = 1,
//                        FirstName = "Admin",
//                        LastName = "User",
//                        Email = "admin@example.com",
//                        Password = EncryptionUtil.Encrypt("Super111!!!"), // Hash the password
//                        Role = Core.Enums.UserRole.Admin,
//                        GroupId = 1,
//                        InsertedAt = DateTime.Now,
//                        ModifiedAt = DateTime.Now
//                    }
//                );
//            });

//            modelBuilder.Entity<Evaluation>(entity =>
//            {
//                entity.ToTable("Evaluations");
//                entity.HasKey(e => e.Id);

//                // Many Evaluations by One User
//                entity.HasOne(ev => ev.EvaluatorUser)
//                      .WithMany(u => u.EvaluationsMade)
//                      .HasForeignKey(ev => ev.EvaluatorUserId)
//                      .OnDelete(DeleteBehavior.Restrict);
//                // And Many Evaluations to One User
//                entity.HasOne(ev => ev.EvaluateeUser)
//                      .WithMany(u => u.EvaluationsReceived)
//                      .HasForeignKey(ev => ev.EvaluateeUserId)
//                      .OnDelete(DeleteBehavior.Restrict);
//                entity.HasOne(ev => ev.EvaluationCycle)
//                      .WithMany(ec => ec.Evaluations)
//                      .HasForeignKey(ev => ev.EvaluationCycleId)
//                      .OnDelete(DeleteBehavior.Cascade);
//                // Set a default timestamp
//                entity.Property(ev => ev.TimeStamp)
//                      .HasDefaultValueSql("GETDATE()");

//                // From Base Entity
//                entity.Property(e => e.InsertedAt)
//                       .ValueGeneratedOnAdd()
//                       .HasDefaultValueSql("GETDATE()");
//                entity.Property(e => e.ModifiedAt)
//                      .ValueGeneratedOnAddOrUpdate()
//                      .HasDefaultValueSql("GETDATE()");
//            });

//            modelBuilder.Entity<EvaluationCycle>(entity =>
//            {
//                entity.ToTable("EvaluationCycles");
//                entity.HasKey(ec => ec.Id);

//                entity.Property(ec => ec.Title)
//                      .HasMaxLength(255)
//                      .IsRequired();
//                entity.Property(ec => ec.Status)
//                      .IsRequired();
//                entity.Property(ec => ec.StartDate)
//                      .IsRequired();
//                entity.Property(ec => ec.EndDate)
//                      .IsRequired();

//                // Index Cycle
//                entity.HasIndex(e => e.Title);

//                // From Base Entity
//                entity.Property(e => e.InsertedAt)
//                       .ValueGeneratedOnAdd()
//                       .HasDefaultValueSql("GETDATE()");
//                entity.Property(e => e.ModifiedAt)
//                      .ValueGeneratedOnAddOrUpdate()
//                      .HasDefaultValueSql("GETDATE()");

//            });

//            modelBuilder.Entity<Question>(entity =>
//            {
//                entity.ToTable("Questions");
//                entity.HasKey(q => q.Id);

//                entity.Property(q => q.Text)
//                      .HasMaxLength(500)
//                      .IsRequired();

//                // From Base Entity
//                entity.Property(e => e.InsertedAt)
//                       .ValueGeneratedOnAdd()
//                       .HasDefaultValueSql("GETDATE()");
//                entity.Property(e => e.ModifiedAt)
//                      .ValueGeneratedOnAddOrUpdate()
//                      .HasDefaultValueSql("GETDATE()");

//                entity.HasData(
//                   new Question
//                   {
//                       Id = 1,
//                       Text = "Self-Confidence",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   },
//                   new Question
//                   {
//                       Id = 2,
//                       Text = "Dedication",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   },
//                   new Question
//                   {
//                       Id = 3,
//                       Text = "Job Knowledge",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   },
//                   new Question
//                   {
//                       Id = 4,
//                       Text = "Quality and Accuracy of Work",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   },
//                   new Question
//                   {
//                       Id = 5,
//                       Text = "Ability to Meet Deadlines",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   },
//                   new Question
//                   {
//                       Id = 6,
//                       Text = "Independence",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   },
//                   new Question
//                   {
//                       Id = 7,
//                       Text = "Commitment",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   },
//                   new Question
//                   {
//                       Id = 8,
//                       Text = "Attention to Detail",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   },
//                   new Question
//                   {
//                       Id = 9,
//                       Text = "Ability to Work with Others",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   },
//                   new Question
//                   {
//                       Id = 10,
//                       Text = "Communication Skills",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   },
//                   new Question
//                   {
//                       Id = 11,
//                       Text = "Performs Assigned Duties Under Pressure",
//                       InsertedAt = DateTime.Now,
//                       ModifiedAt = DateTime.Now
//                   }
//               );
//            });

//            modelBuilder.Entity<EvaluationAnswer>(entity =>
//            {
//                entity.ToTable("EvaluationAnswers");
//                entity.HasKey(ea => ea.Id);

//                entity.Property(ea => ea.AnswerValue)
//                      .HasMaxLength(255)
//                      .IsRequired(false);
//                entity.HasOne(ea => ea.Evaluation)
//                      .WithMany(e => e.Answers)
//                      .HasForeignKey(ea => ea.EvaluationId)
//                      .OnDelete(DeleteBehavior.Cascade);
//                entity.HasOne(ea => ea.Question)
//                      .WithMany(q => q.Answers)
//                      .HasForeignKey(ea => ea.QuestionId)
//                      .OnDelete(DeleteBehavior.Restrict);

//                // From Base Entity
//                entity.Property(e => e.InsertedAt)
//                       .ValueGeneratedOnAdd()
//                       .HasDefaultValueSql("GETDATE()");
//                entity.Property(e => e.ModifiedAt)
//                      .ValueGeneratedOnAddOrUpdate()
//                      .HasDefaultValueSql("GETDATE()");
//            });

//            base.OnModelCreating(modelBuilder);
//        }

//    }
//}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PeerEvalAppAPI.Security;

namespace PeerEvalAppAPI.Data
{
    public class PeerEvalAppDbContext : DbContext
    {
        public PeerEvalAppDbContext(DbContextOptions<PeerEvalAppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<EvaluationAnswer> EvaluationsAnswers { get; set; }
        public DbSet<EvaluationCycle> EvaluationsCycles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Groups");
                entity.HasKey(g => g.Id);
                entity.Property(g => g.GroupName)
                      .IsRequired()
                      .HasMaxLength(128);
                entity.HasMany(g => g.Users)
                      .WithOne(u => u.Group)
                      .HasForeignKey(u => u.GroupId)
                      .OnDelete(DeleteBehavior.Restrict);
                // From Base Entity
                entity.Property(e => e.InsertedAt)
                       .ValueGeneratedOnAdd()
                       .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.ModifiedAt)
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FirstName)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(u => u.LastName)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.HasIndex(u => u.Email)
                      .IsUnique();
                entity.Property(u => u.Role)
                      .HasConversion<string>()
                      .IsRequired();
                entity.HasOne(u => u.Manager)
                      .WithMany(m => m.Subordinates)
                      .HasForeignKey(u => u.ManagerId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Group)
                      .WithMany(g => g.Users)
                      .HasForeignKey(u => u.GroupId)
                      .OnDelete(DeleteBehavior.Restrict);

                // From Base Entity
                entity.Property(e => e.InsertedAt)
                       .ValueGeneratedOnAdd()
                       .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.ModifiedAt)
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.ToTable("Evaluations");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.EvaluatorUser)
                      .WithMany(u => u.EvaluationsMade)
                      .HasForeignKey(e => e.EvaluatorUserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.EvaluateeUser)
                      .WithMany(u => u.EvaluationsReceived)
                      .HasForeignKey(e => e.EvaluateeUserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.EvaluationCycle)
                      .WithMany(ec => ec.Evaluations)
                      .HasForeignKey(e => e.EvaluationCycleId)
                      .OnDelete(DeleteBehavior.Cascade);
                // From Base Entity
                entity.Property(e => e.InsertedAt)
                       .ValueGeneratedOnAdd()
                       .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.ModifiedAt)
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<EvaluationCycle>(entity =>
            {
                entity.ToTable("EvaluationCycles");
                entity.HasKey(ec => ec.Id);
                entity.Property(ec => ec.Title)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(ec => ec.StartDate)
                      .IsRequired();
                entity.Property(ec => ec.EndDate)
                      .IsRequired();
                entity.Property(ec => ec.Status)
                      .IsRequired();
                // From Base Entity
                entity.Property(e => e.InsertedAt)
                       .ValueGeneratedOnAdd()
                       .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.ModifiedAt)
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Questions");
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Text)
                      .IsRequired()
                      .HasMaxLength(500);
                // From Base Entity
                entity.Property(e => e.InsertedAt)
                       .ValueGeneratedOnAdd()
                       .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.ModifiedAt)
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<EvaluationAnswer>(entity =>
            {
                entity.ToTable("EvaluationAnswers");
                entity.HasKey(ea => ea.Id);
                entity.HasOne(ea => ea.Evaluation)
                      .WithMany(e => e.Answers)
                      .HasForeignKey(ea => ea.EvaluationId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ea => ea.Question)
                      .WithMany(q => q.Answers)
                      .HasForeignKey(ea => ea.QuestionId)
                      .OnDelete(DeleteBehavior.Restrict);
                // From Base Entity
                entity.Property(e => e.InsertedAt)
                       .ValueGeneratedOnAdd()
                       .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.ModifiedAt)
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("GETDATE()");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
