using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.Entities;
using RewriteMe.DataAccess.EntitiesConfiguration;

namespace RewriteMe.DataAccess
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(string connectionString, DbContextOptions<AppDbContext> options)
            : base(options)
        {
            _connectionString = connectionString;
        }

        public DbSet<AdministratorEntity> Administrators { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<FileItemEntity> FileItems { get; set; }

        public DbSet<TranscribeItemEntity> TranscribeItems { get; set; }

        public DbSet<FileItemSourceEntity> FileItemSources { get; set; }

        public DbSet<UploadedChunkEntity> UploadedChunk { get; set; }

        public DbSet<TranscribeItemSourceEntity> TranscribeItemSources { get; set; }

        public DbSet<RecognizedAudioSampleEntity> RecognizedAudioSamples { get; set; }

        public DbSet<SpeechResultEntity> SpeechResults { get; set; }

        public DbSet<CurrentUserSubscriptionEntity> CurrentUserSubscription { get; set; }

        public DbSet<UserSubscriptionEntity> UserSubscriptions { get; set; }

        public DbSet<BillingPurchaseEntity> BillingPurchases { get; set; }

        public DbSet<ApplicationLogEntity> ApplicationLogs { get; set; }

        public DbSet<ContactFormEntity> ContactForms { get; set; }

        public DbSet<InformationMessageEntity> InformationMessages { get; set; }

        public DbSet<LanguageVersionEntity> LanguageVersions { get; set; }

        public DbSet<UserDeviceEntity> UserDevices { get; set; }

        public DbSet<InternalValueEntity> InternalValues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, providerOptions => providerOptions.CommandTimeout(60));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdministratorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FileItemEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FileItemSourceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UploadedChunkEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TranscribeItemEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TranscribeItemSourceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RecognizedAudioSampleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SpeechResultEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CurrentUserSubscriptionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserSubscriptionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BillingPurchaseEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationLogEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ContactFormEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InformationMessageEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageVersionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserDeviceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InternalValueEntityConfiguration());
        }
    }
}
