using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WEB_MMT.Models;

public partial class QuizzContext : DbContext
{
    public QuizzContext()
    {
    }

    public QuizzContext(DbContextOptions<QuizzContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<TblAdmin> TblAdmins { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblQuestion> TblQuestions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-PJSSBNVP\\SQLEXPRESS;Initial Catalog=Quizz;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__options__92C7A1DF075E816B");

            entity.ToTable("options");

            entity.HasIndex(e => e.OptionName, "UQ__options__A696DB678D0EF709").IsUnique();

            entity.Property(e => e.OptionId).HasColumnName("OptionID");
            entity.Property(e => e.iduser).HasColumnName("iduser");
            entity.Property(e => e.OptionDate).HasColumnType("datetime");
            entity.Property(e => e.OptionName).HasMaxLength(250);
            entity.Property(e => e.users_score).HasColumnName("users_score");
            entity.Property(e => e.cat_id).HasColumnName("cat_id");

            entity.HasOne(d => d.iduserNavigation).WithMany(p => p.Options)
                .HasForeignKey(d => d.iduser)
                .HasConstraintName("FK_Options_User");
            entity.HasOne(d => d.catidNavigation).WithMany(p => p.Options)
                .HasForeignKey(d => d.cat_id)
                .HasConstraintName("FK_options_category");
        });

        modelBuilder.Entity<TblAdmin>(entity =>
        {
            entity.HasKey(e => e.Adid).HasName("PK__tbl_admi__56B503F0440417CF");

            entity.ToTable("tbl_admin");

            entity.HasIndex(e => e.Adname, "UQ__tbl_admi__E49D922936CDB45E").IsUnique();

            entity.Property(e => e.Adid).HasColumnName("adid");
            entity.Property(e => e.Adname)
                .HasMaxLength(20)
                .HasColumnName("adname");
            entity.Property(e => e.Adpass)
                .HasMaxLength(200)
                .HasColumnName("adpass");
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CatId).HasName("PK__tbl_cate__DD5DDDBD6C8B3C5D");

            entity.ToTable("tbl_category");

            entity.Property(e => e.CatId).HasColumnName("cat_id");
            entity.Property(e => e.adid).HasColumnName("adid");
            entity.Property(e => e.CatName)
                .HasMaxLength(50)
                .HasColumnName("cat_name");

            entity.HasOne(d => d.FKadid).WithMany(p => p.TblCategories)
                .HasForeignKey(d => d.adid)
                .HasConstraintName("FK_category_admin");
        });

        modelBuilder.Entity<TblQuestion>(entity =>
        {
            entity.HasKey(e => e.Questionid).HasName("PK__tbl_ques__62C2216A99B1CC75");

            entity.ToTable("tbl_questions");

            entity.HasIndex(e => e.QuestionA, "UQ__tbl_ques__2E88D1B349C264D8").IsUnique();

            entity.HasIndex(e => e.QuestionB, "UQ__tbl_ques__2E88D1B40B7870E0").IsUnique();

            entity.HasIndex(e => e.QuestionC, "UQ__tbl_ques__2E88D1B5FEA688C2").IsUnique();

            entity.HasIndex(e => e.QuestionD, "UQ__tbl_ques__2E88D1B6AD9D7577").IsUnique();

            entity.HasIndex(e => e.QuestionCorrect, "UQ__tbl_ques__B55A9B53E55AA075").IsUnique();

            entity.HasIndex(e => e.Questiontext, "UQ__tbl_ques__E5914BAC60C847A4").IsUnique();

            entity.Property(e => e.Questionid).HasColumnName("questionid");
            entity.Property(e => e.cat_id).HasColumnName("cat_id");
            entity.Property(e => e.QuestionA).HasMaxLength(250);
            entity.Property(e => e.QuestionB).HasMaxLength(250);
            entity.Property(e => e.QuestionC).HasMaxLength(250);
            entity.Property(e => e.QuestionCorrect).HasMaxLength(250);
            entity.Property(e => e.QuestionD).HasMaxLength(250);
            entity.Property(e => e.Questiontext)
                .HasMaxLength(20)
                .HasColumnName("questiontext");

            entity.HasOne(d => d.FKcat_id).WithMany(p => p.TblQuestions)
                .HasForeignKey(d => d.cat_id)
                .HasConstraintName("FK_question_category");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__users__2A50F1CE337A3F2F");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164E5651D9C").IsUnique();

            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasColumnName("email");
            entity.Property(e => e.Nameuser)
                .HasMaxLength(250)
                .HasColumnName("nameuser");
            entity.Property(e => e.Passwords)
                .HasMaxLength(250)
                .HasColumnName("passwords");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
