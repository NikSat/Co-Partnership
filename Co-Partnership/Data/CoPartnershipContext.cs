using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Co_Partnership.Models.Database;

namespace Co_Partnership.Data
{
    public partial class CoPartnershipContext : DbContext
    {
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Fund> Fund { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Offer> Offer { get; set; }
        public virtual DbSet<OfferedItems> OfferedItems { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<PersonalFund> PersonalFund { get; set; }
        public virtual DbSet<Phone> Phone { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<WishList> WishList { get; set; }

        public CoPartnershipContext(DbContextOptions<CoPartnershipContext> options)
            :base(options)
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

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.Zip)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Address_User");
            });

            modelBuilder.Entity<Fund>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CoopShare).HasColumnType("money");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.MemberShare).HasColumnType("money");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Image).IsUnicode(false);

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

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Message1)
                    .HasColumnName("Message")
                    .IsUnicode(false);

                entity.HasOne(d => d.ReceiverNavigation)
                    .WithMany(p => p.MessageReceiverNavigation)
                    .HasForeignKey(d => d.Receiver)
                    .HasConstraintName("FK_Message_User1");

                entity.HasOne(d => d.SenderNavigation)
                    .WithMany(p => p.MessageSenderNavigation)
                    .HasForeignKey(d => d.Sender)
                    .HasConstraintName("FK_Message_User");
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AdminId).HasColumnName("Admin_Id");

                entity.Property(e => e.FoundId).HasColumnName("Found_Id");

                entity.Property(e => e.MemberId).HasColumnName("Member_Id");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.OfferAdmin)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK_Offer_User");

                entity.HasOne(d => d.Found)
                    .WithMany(p => p.Offer)
                    .HasForeignKey(d => d.FoundId)
                    .HasConstraintName("FK_Offer_Fund");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.OfferMember)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_Offer_User1");
            });

            modelBuilder.Entity<OfferedItems>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ItemId).HasColumnName("Item_Id");

                entity.Property(e => e.OfferId).HasColumnName("Offer_Id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OfferedItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_OfferedItems_Item");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.OfferedItems)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OfferedItems_Offer");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AddressId).HasColumnName("Address_Id");

                entity.Property(e => e.ClientId).HasColumnName("Client_Id");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.FoundId).HasColumnName("Found_Id");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Order_Address");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Order_User");

                entity.HasOne(d => d.Found)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.FoundId)
                    .HasConstraintName("FK_Order_Fund");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ItemId).HasColumnName("Item_Id");

                entity.Property(e => e.OrderId).HasColumnName("Order_Id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_OrderItem_Item");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderItem_Order");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.FundId).HasColumnName("Fund_Id");

                entity.Property(e => e.PfId).HasColumnName("PF_Id");

                entity.HasOne(d => d.Fund)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.FundId)
                    .HasConstraintName("FK_Payment_Fund");
            });

            modelBuilder.Entity<PersonalFund>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("money");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.PersonalFund)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_PersonalFund_User");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.PersonalFund)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_PersonalFund_Payment");
            });

            modelBuilder.Entity<Phone>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Phone)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Phone_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ExtId).HasColumnName("Ext_Id");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserType).HasMaxLength(50);
            });

            modelBuilder.Entity<WishList>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ItemId).HasColumnName("Item_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_WishList_Item");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_WishList_User");
            });
        }
    }
}
