using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PTRKApp.AppData
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Path.GetFullPath("..\\..\\PTRKBase.mdf")};Integrated Security=True")
        {
            Database.Log = message => Debug.WriteLine(message);
        }

        public virtual DbSet<group> group { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<group>()
                .Property(e => e.Name_group)
                .IsFixedLength();

            modelBuilder.Entity<group>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.group)
                .HasForeignKey(e => e.groupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<group>()
                .HasMany(e => e.Users)
                .WithMany(e => e.group)
                .Map(m => m.ToTable("UsersGroup"));

            modelBuilder.Entity<Roles>()
                .Property(e => e.Role)
                .IsFixedLength();

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Roles)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Services>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Services)
                .HasForeignKey(e => e.ServiceId)
                .WillCascadeOnDelete(false);
        }
    }
}
