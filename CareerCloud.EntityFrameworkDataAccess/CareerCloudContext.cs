using CareerCloud.Pocos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CareerCloud.EntityFrameworkDataAccess
{
    public class CareerCloudContext : DbContext
    {
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            //pass db sql config on the optionsBuilder
            optionsBuilder.UseSqlServer(config.Build().GetSection("ConnectionStrings").GetSection("DataConnection").Value);

            base.OnConfiguring(optionsBuilder);
        }

        //EF6 uses DbModelBuilder while EFCore uses ModelBuilder https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext.onmodelcreating?view=efcore-3.1
        #region OnModelCreating Model Builder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region SystemLanguageCodePoco modelBuilder
            modelBuilder.Entity<SystemLanguageCodePoco>()
                .HasMany(x => x.CompanyDescriptions)
                .WithOne(x => x.SystemLanguageCodes)
                .HasForeignKey(x => x.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region SystemCountryCodePoco modelBuilder
            modelBuilder.Entity<SystemCountryCodePoco>()
                .HasMany(x => x.ApplicantWorkHistory)
                .WithOne(x => x.SystemCountryCodes)
                .HasForeignKey(x => x.CountryCode)
                .OnDelete(DeleteBehavior.Cascade); 
            modelBuilder.Entity<SystemCountryCodePoco>()
                .HasMany(x => x.ApplicantProfiles)
                .WithOne(x => x.SystemCountryCodes)
                .HasForeignKey(x => x.Country)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region SecurityRolePoco modelBuilder
            modelBuilder.Entity<SecurityRolePoco>()
                .HasMany(x => x.SecurityLoginsRoles)
                .WithOne(x => x.SecurityRoles)
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region SecurityLoginPoco modelBuilder
            modelBuilder.Entity<SecurityLoginPoco>()
                .HasMany(x => x.SecurityLoginsLog)
                .WithOne(x => x.SecurityLogins)
                .HasForeignKey(x=>x.Login)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SecurityLoginPoco>()
                .HasMany(x=> x.ApplicantProfiles)
                .WithOne(x=> x.SecurityLogins)
                .HasForeignKey(x=>x.Login)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SecurityLoginPoco>()
                .HasMany(x => x.SecurityLoginsRoles)
                .WithOne(x => x.SecurityLogins)
                .HasForeignKey(x => x.Login)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region ApplicantProfilePoco
            modelBuilder.Entity<ApplicantProfilePoco>()
                .HasMany(x => x.ApplicantSkills)
                .WithOne(x => x.ApplicantProfiles)
                .HasForeignKey(x => x.Applicant)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicantProfilePoco>()
                .HasMany(x => x.ApplicantResumes)
                .WithOne(x => x.ApplicantProfiles)
                .HasForeignKey(x => x.Applicant)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicantProfilePoco>()
                .HasMany(x => x.ApplicantWorkHistory)
                .WithOne(x => x.ApplicantProfiles)
                .HasForeignKey(x => x.Applicant)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicantProfilePoco>()
                .HasMany(x => x.ApplicantJobApplications)
                .WithOne(x => x.ApplicantProfiles)
                .HasForeignKey(x => x.Applicant)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicantProfilePoco>()
                .HasMany(x => x.ApplicantEducations)
                .WithOne(x => x.ApplicantProfiles)
                .HasForeignKey(x => x.Applicant)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region CompanyJobPoco modelBuilder
            modelBuilder.Entity<CompanyJobPoco>()
                .HasMany(x => x.CompanyJobEducations)
                .WithOne(x => x.CompanyJobs)
                .HasForeignKey(x => x.Job)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CompanyJobPoco>()
                .HasMany(x => x.CompanyJobsDescriptions)
                .WithOne(x => x.CompanyJobs)
                .HasForeignKey(x => x.Job)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CompanyJobPoco>()
                .HasMany(x => x.CompanyJobSkills)
                .WithOne(x => x.CompanyJobs)
                .HasForeignKey(x => x.Job)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CompanyJobPoco>()
                .HasMany(x => x.ApplicantJobApplications)
                .WithOne(x => x.CompanyJobs)
                .HasForeignKey(x => x.Job)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region CompanyProfilePoco modelBuilder
            modelBuilder.Entity<CompanyProfilePoco>()
                .HasMany(x => x.CompanyLocations)
                .WithOne(x => x.CompanyProfiles)
                .HasForeignKey(x => x.Company)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CompanyProfilePoco>()
                .HasMany(x => x.CompanyDescriptions)
                .WithOne(x => x.CompanyProfiles)
                .HasForeignKey(x => x.Company)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CompanyProfilePoco>()
                .HasMany(x => x.CompanyJobs)
                .WithOne(x => x.CompanyProfiles)
                .HasForeignKey(x => x.Company)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region timestamp ignore implementation
            //don't map timestamp, with optimistic concurrency detection -> .Property(t => t.TimeStamp).IsRowVersion() or .Ignore(t => t.TimeStamp);
            modelBuilder.Entity<ApplicantProfilePoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<ApplicantJobApplicationPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<ApplicantEducationPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<ApplicantSkillPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<ApplicantWorkHistoryPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<CompanyDescriptionPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<CompanyJobSkillPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<CompanyJobPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<CompanyJobEducationPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<CompanyJobDescriptionPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<CompanyLocationPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<CompanyProfilePoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<SecurityLoginPoco>().Property(t => t.TimeStamp).IsRowVersion();
            modelBuilder.Entity<SecurityLoginsRolePoco>().Property(t => t.TimeStamp).IsRowVersion();
            #endregion
            //once all models are created execute this
            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region DBSet Per Poco
        //setting DbSet properties for each Poco per CareerCloud.Pocos
        public DbSet<ApplicantEducationPoco> ApplicantEducations { get; set; }
        public DbSet<ApplicantJobApplicationPoco> ApplicantJobApplications { get; set; }
        public DbSet<ApplicantProfilePoco> ApplicantProfiles { get; set; }
        public DbSet<ApplicantResumePoco> ApplicantResumes { get; set; }
        public DbSet<ApplicantSkillPoco> ApplicantSkills { get; set; }
        public DbSet<ApplicantWorkHistoryPoco> ApplicantWorkHistory { get; set; }
        public DbSet<CompanyDescriptionPoco> CompanyDescriptions { get; set; }
        public DbSet<CompanyJobDescriptionPoco> CompanyJobsDescriptions { get; set; }
        public DbSet<CompanyJobPoco> CompanyJobs { get; set; }
        public DbSet<CompanyJobSkillPoco> CompanyJobSkills { get; set; }
        public DbSet<CompanyLocationPoco> CompanyLocations { get; set; }
        public DbSet<CompanyProfilePoco> CompanyProfiles { get; set; }
        public DbSet<SecurityLoginPoco> SecurityLogins { get; set; }
        public DbSet<SecurityLoginsLogPoco> SecurityLoginsLog { get; set; }
        public DbSet<SecurityLoginsRolePoco> SecurityLoginsRoles { get; set; }
        public DbSet<SecurityRolePoco> SecurityRoles { get; set; }
        public DbSet<SystemCountryCodePoco> SystemCountryCodes { get; set; }
        public DbSet<SystemLanguageCodePoco> SystemLanguageCodes { get; set; }
        #endregion


    }
}
