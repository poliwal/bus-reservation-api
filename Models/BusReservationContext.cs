using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BusReservation.Models
{
    public partial class BusReservationContext : DbContext
    {
        public BusReservationContext()
        {
        }

        public BusReservationContext(DbContextOptions<BusReservationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<BusSeatNo> BusSeatNos { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<PassengerDetail> PassengerDetails { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<bus> Buses { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-GBJB52G;Database=BusReservation;Trusted_Connection=True;");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.AdminId).HasMaxLength(20);

                entity.Property(e => e.AdminPass).HasMaxLength(20);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.DateOfBooking)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReturnDate).HasColumnType("date");

                entity.Property(e => e.SecurityDeposit).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.TotalFare).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Bus)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.BusId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_B_B_BID");

                entity.HasOne(d => d.CidNavigation)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.Cid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_C_B_CID");
            });

            modelBuilder.Entity<BusSeatNo>(entity =>
            {
                entity.HasKey(e => e.SeatId)
                    .HasName("PK__BusSeatN__311713F3361D722E");

                entity.ToTable("BusSeatNo");

                entity.HasOne(d => d.Bus)
                    .WithMany(p => p.BusSeatNos)
                    .HasForeignKey(d => d.BusId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_B_Bs_BID");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Cid)
                    .HasName("PK__Customer__C1FFD861393517FE");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.ContactNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Fname)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsAuthorized).HasDefaultValueSql("((0))");

                entity.Property(e => e.Lname)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Wallet).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<PassengerDetail>(entity =>
            {
                entity.HasKey(e => e.PassId)
                    .HasName("PK__Passenge__C6740AA8588D0B4F");

                entity.Property(e => e.Page).HasColumnName("PAge");

                entity.Property(e => e.Pname)
                    .HasMaxLength(30)
                    .HasColumnName("PName");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.PassengerDetails)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Bo_PD_BID");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => e.ScId)
                    .HasName("PK__Schedule__ACB791DA2DEA73AF");

                entity.Property(e => e.ScDate).HasColumnType("date");

                entity.HasOne(d => d.Bus)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.BusId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_S_B_BID");
            });

            modelBuilder.Entity<bus>(entity =>
            {
                entity.Property(e => e.BusName).HasMaxLength(30);

                entity.Property(e => e.Destination).HasMaxLength(30);

                entity.Property(e => e.DriverName).HasMaxLength(30);

                entity.Property(e => e.Fare).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Source).HasMaxLength(30);

                entity.Property(e => e.Via).HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
