using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Data
{
    public partial class Co_PartnershipContext : DbContext
    {
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<CompanyFinancialAccount> CompanyFinancialAccount { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<PersonalFinancialAccount> PersonalFinancialAccount { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionItem> TransactionItem { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<WishList> WishList { get; set; }

        public Co_PartnershipContext(DbContextOptions<Co_PartnershipContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address1)
                    .HasColumnName("Address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Number)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_Id");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.Property(e => e.Zip)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Address_Transaction");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Address_User");
            });

            modelBuilder.Entity<CompanyFinancialAccount>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CoOpShare).HasColumnType("money");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.MemberShare).HasColumnType("money");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.Property(e => e.UnitType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateProcessed).HasColumnType("datetime");

                entity.Property(e => e.DateSent).HasColumnType("datetime");

                entity.Property(e => e.Message1)
                    .HasColumnName("Message")
                    .IsUnicode(false);

                entity.Property(e => e.ReceiverId).HasColumnName("Receiver_Id");

                entity.Property(e => e.SenderId).HasColumnName("Sender_Id");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessageReceiver)
                    .HasForeignKey(d => d.ReceiverId)
                    .HasConstraintName("FK_Message_User");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSender)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Message_Sender");
            });

            modelBuilder.Entity<PersonalFinancialAccount>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("User_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.PersonalFinancialAccount)
                    .HasForeignKey<PersonalFinancialAccount>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonalFinancialAccount_User");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DateProcessed).HasColumnType("datetime");

                entity.Property(e => e.OwnerId).HasColumnName("Owner_Id");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.RecipientId).HasColumnName("Recipient_Id");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.TransactionOwner)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Transaction_Owner");

                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.TransactionRecipient)
                    .HasForeignKey(d => d.RecipientId)
                    .HasConstraintName("FK_Transaction_Recepient");
            });

            modelBuilder.Entity<TransactionItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ItemId).HasColumnName("Item_Id");

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_Id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.TransactionItem)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_TransactionItem_Item");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.TransactionItem)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TransactionItem_Transaction");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ExtId)
                    .HasColumnName("Ext_Id")
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WishList>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ItemId).HasColumnName("Item_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_WishList_Item");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_WishList_User");
            });
        }
    }
}
