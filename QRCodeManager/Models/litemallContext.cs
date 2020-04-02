using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QRCodeManager.Models
{
    public partial class LitemallContext : DbContext
    {
        public LitemallContext()
        {
        }

        public LitemallContext(DbContextOptions<LitemallContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LitemallAd> LitemallAd { get; set; }
        public virtual DbSet<LitemallAddress> LitemallAddress { get; set; }
        public virtual DbSet<LitemallAdmin> LitemallAdmin { get; set; }
        public virtual DbSet<LitemallBrand> LitemallBrand { get; set; }
        public virtual DbSet<LitemallCart> LitemallCart { get; set; }
        public virtual DbSet<LitemallCategory> LitemallCategory { get; set; }
        public virtual DbSet<LitemallCollect> LitemallCollect { get; set; }
        public virtual DbSet<LitemallComment> LitemallComment { get; set; }
        public virtual DbSet<LitemallCoupon> LitemallCoupon { get; set; }
        public virtual DbSet<LitemallCouponUser> LitemallCouponUser { get; set; }
        public virtual DbSet<LitemallFeedback> LitemallFeedback { get; set; }
        public virtual DbSet<LitemallFootprint> LitemallFootprint { get; set; }
        public virtual DbSet<LitemallGoods> LitemallGoods { get; set; }
        public virtual DbSet<LitemallGoodsAttribute> LitemallGoodsAttribute { get; set; }
        public virtual DbSet<LitemallGoodsProduct> LitemallGoodsProduct { get; set; }
        public virtual DbSet<LitemallGoodsSpecification> LitemallGoodsSpecification { get; set; }
        public virtual DbSet<LitemallGroupon> LitemallGroupon { get; set; }
        public virtual DbSet<LitemallGrouponRules> LitemallGrouponRules { get; set; }
        public virtual DbSet<LitemallIssue> LitemallIssue { get; set; }
        public virtual DbSet<LitemallKeyword> LitemallKeyword { get; set; }
        public virtual DbSet<LitemallLog> LitemallLog { get; set; }
        public virtual DbSet<LitemallOrder> LitemallOrder { get; set; }
        public virtual DbSet<LitemallOrderGoods> LitemallOrderGoods { get; set; }
        public virtual DbSet<LitemallPermission> LitemallPermission { get; set; }
        public virtual DbSet<LitemallRegion> LitemallRegion { get; set; }
        public virtual DbSet<LitemallRole> LitemallRole { get; set; }
        public virtual DbSet<LitemallSearchHistory> LitemallSearchHistory { get; set; }
        public virtual DbSet<LitemallStorage> LitemallStorage { get; set; }
        public virtual DbSet<LitemallSystem> LitemallSystem { get; set; }
        public virtual DbSet<LitemallTopic> LitemallTopic { get; set; }
        public virtual DbSet<LitemallUser> LitemallUser { get; set; }
        public virtual DbSet<LitemallUserFormid> LitemallUserFormid { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseMySQL("server=clarabeautynism.com;port=3306;user=qrcodemanager;password=p74bn5WS19qrcode;database=litemall");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<LitemallAd>(entity =>
            {
                entity.ToTable("litemall_ad", "litemall");

                entity.HasIndex(e => e.Enabled)
                    .HasName("enabled");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Enabled)
                    .HasColumnName("enabled")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasColumnName("link")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasColumnName("position")
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LitemallAddress>(entity =>
            {
                entity.ToTable("litemall_address", "litemall");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.AddressDetail)
                    .IsRequired()
                    .HasColumnName("address_detail")
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.AreaCode)
                    .HasColumnName("area_code")
                    .HasColumnType("char(6)");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.County)
                    .IsRequired()
                    .HasColumnName("county")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsDefault)
                    .HasColumnName("is_default")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code")
                    .HasColumnType("char(6)");

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasColumnName("province")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.Tel)
                    .IsRequired()
                    .HasColumnName("tel")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<LitemallAdmin>(entity =>
            {
                entity.ToTable("litemall_admin", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LastLoginIp)
                    .HasColumnName("last_login_ip")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.LastLoginTime).HasColumnName("last_login_time");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.RoleIds)
                    .HasColumnName("role_ids")
                    .HasMaxLength(127)
                    .IsUnicode(false)
                    .HasDefaultValueSql("[]");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(63)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LitemallBrand>(entity =>
            {
                entity.ToTable("litemall_brand", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Desc)
                    .IsRequired()
                    .HasColumnName("desc")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FloorPrice)
                    .HasColumnName("floor_price")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("0.00");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PicUrl)
                    .IsRequired()
                    .HasColumnName("pic_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("50");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallCart>(entity =>
            {
                entity.ToTable("litemall_cart", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Checked)
                    .HasColumnName("checked")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GoodsId)
                    .HasColumnName("goods_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GoodsName)
                    .HasColumnName("goods_name")
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.GoodsSn)
                    .HasColumnName("goods_sn")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasColumnType("smallint(5)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.PicUrl)
                    .HasColumnName("pic_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("0.00");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Specifications)
                    .HasColumnName("specifications")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<LitemallCategory>(entity =>
            {
                entity.ToTable("litemall_category", "litemall");

                entity.HasIndex(e => e.Pid)
                    .HasName("parent_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Desc)
                    .HasColumnName("desc")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IconUrl)
                    .HasColumnName("icon_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Keywords)
                    .IsRequired()
                    .HasColumnName("keywords")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("L1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.PicUrl)
                    .HasColumnName("pic_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Pid)
                    .HasColumnName("pid")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("50");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallCollect>(entity =>
            {
                entity.ToTable("litemall_collect", "litemall");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id");

                entity.HasIndex(e => e.ValueId)
                    .HasName("goods_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ValueId)
                    .HasColumnName("value_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<LitemallComment>(entity =>
            {
                entity.ToTable("litemall_comment", "litemall");

                entity.HasIndex(e => e.ValueId)
                    .HasName("id_value");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HasPicture)
                    .HasColumnName("has_picture")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.PicUrls)
                    .HasColumnName("pic_urls")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.Star)
                    .HasColumnName("star")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ValueId)
                    .HasColumnName("value_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<LitemallCoupon>(entity =>
            {
                entity.ToTable("litemall_coupon", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.Days)
                    .HasColumnName("days")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Desc)
                    .HasColumnName("desc")
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.Discount)
                    .HasColumnName("discount")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("0.00");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.GoodsType)
                    .HasColumnName("goods_type")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GoodsValue)
                    .HasColumnName("goods_value")
                    .HasMaxLength(1023)
                    .IsUnicode(false)
                    .HasDefaultValueSql("[]");

                entity.Property(e => e.Limit)
                    .HasColumnName("limit")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Min)
                    .HasColumnName("min")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("0.00");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.TimeType)
                    .HasColumnName("time_type")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallCouponUser>(entity =>
            {
                entity.ToTable("litemall_coupon_user", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.CouponId)
                    .HasColumnName("coupon_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UsedTime).HasColumnName("used_time");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<LitemallFeedback>(entity =>
            {
                entity.ToTable("litemall_feedback", "litemall");

                entity.HasIndex(e => e.Status)
                    .HasName("id_value");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.FeedType)
                    .IsRequired()
                    .HasColumnName("feed_type")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.HasPicture)
                    .HasColumnName("has_picture")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasColumnName("mobile")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PicUrls)
                    .HasColumnName("pic_urls")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(3)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(63)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LitemallFootprint>(entity =>
            {
                entity.ToTable("litemall_footprint", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GoodsId)
                    .HasColumnName("goods_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<LitemallGoods>(entity =>
            {
                entity.ToTable("litemall_goods", "litemall");

                entity.HasIndex(e => e.BrandId)
                    .HasName("brand_id");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("cat_id");

                entity.HasIndex(e => e.GoodsSn)
                    .HasName("goods_sn");

                entity.HasIndex(e => e.SortOrder)
                    .HasName("sort_order");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.BrandId)
                    .HasColumnName("brand_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Brief)
                    .HasColumnName("brief")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CounterPrice)
                    .HasColumnName("counter_price")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("0.00");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Detail)
                    .HasColumnName("detail")
                    .IsUnicode(false);

                entity.Property(e => e.Gallery)
                    .HasColumnName("gallery")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.GoodsSn)
                    .IsRequired()
                    .HasColumnName("goods_sn")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.IsHot)
                    .HasColumnName("is_hot")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsNew)
                    .HasColumnName("is_new")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsOnSale)
                    .HasColumnName("is_on_sale")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Keywords)
                    .HasColumnName("keywords")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.PicUrl)
                    .HasColumnName("pic_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RetailPrice)
                    .HasColumnName("retail_price")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("100000.00");

                entity.Property(e => e.ShareUrl)
                    .HasColumnName("share_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasColumnType("smallint(4)")
                    .HasDefaultValueSql("100");

                entity.Property(e => e.Unit)
                    .HasColumnName("unit")
                    .HasMaxLength(31)
                    .IsUnicode(false)
                    .HasDefaultValueSql("’件‘");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallGoodsAttribute>(entity =>
            {
                entity.ToTable("litemall_goods_attribute", "litemall");

                entity.HasIndex(e => e.GoodsId)
                    .HasName("goods_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Attribute)
                    .IsRequired()
                    .HasColumnName("attribute")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GoodsId)
                    .HasColumnName("goods_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LitemallGoodsProduct>(entity =>
            {
                entity.ToTable("litemall_goods_product", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GoodsId)
                    .HasColumnName("goods_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("0.00");

                entity.Property(e => e.Specifications)
                    .IsRequired()
                    .HasColumnName("specifications")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(125)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LitemallGoodsSpecification>(entity =>
            {
                entity.ToTable("litemall_goods_specification", "litemall");

                entity.HasIndex(e => e.GoodsId)
                    .HasName("goods_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GoodsId)
                    .HasColumnName("goods_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.PicUrl)
                    .IsRequired()
                    .HasColumnName("pic_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Specification)
                    .IsRequired()
                    .HasColumnName("specification")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LitemallGroupon>(entity =>
            {
                entity.ToTable("litemall_groupon", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.CreatorUserId)
                    .HasColumnName("creator_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GrouponId)
                    .HasColumnName("groupon_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Payed)
                    .HasColumnName("payed")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.RulesId)
                    .HasColumnName("rules_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ShareUrl)
                    .HasColumnName("share_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<LitemallGrouponRules>(entity =>
            {
                entity.ToTable("litemall_groupon_rules", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Discount)
                    .HasColumnName("discount")
                    .HasColumnType("decimal(63,0)");

                entity.Property(e => e.DiscountMember)
                    .HasColumnName("discount_member")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ExpireTime).HasColumnName("expire_time");

                entity.Property(e => e.GoodsId)
                    .HasColumnName("goods_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GoodsName)
                    .IsRequired()
                    .HasColumnName("goods_name")
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.PicUrl)
                    .HasColumnName("pic_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallIssue>(entity =>
            {
                entity.ToTable("litemall_issue", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Answer)
                    .HasColumnName("answer")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Question)
                    .HasColumnName("question")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallKeyword>(entity =>
            {
                entity.ToTable("litemall_keyword", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsDefault)
                    .HasColumnName("is_default")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsHot)
                    .HasColumnName("is_hot")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Keyword)
                    .IsRequired()
                    .HasColumnName("keyword")
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("100");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LitemallLog>(entity =>
            {
                entity.ToTable("litemall_log", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Admin)
                    .HasColumnName("admin")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Ip)
                    .HasColumnName("ip")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Result)
                    .HasColumnName("result")
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallOrder>(entity =>
            {
                entity.ToTable("litemall_order", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActualPrice)
                    .HasColumnName("actual_price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("smallint(6)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ConfirmTime).HasColumnName("confirm_time");

                entity.Property(e => e.Consignee)
                    .IsRequired()
                    .HasColumnName("consignee")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.CouponPrice)
                    .HasColumnName("coupon_price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.FreightPrice)
                    .HasColumnName("freight_price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.GoodsPrice)
                    .HasColumnName("goods_price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.GrouponPrice)
                    .HasColumnName("groupon_price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.IntegralPrice)
                    .HasColumnName("integral_price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasColumnName("mobile")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.OrderPrice)
                    .HasColumnName("order_price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.OrderSn)
                    .IsRequired()
                    .HasColumnName("order_sn")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.OrderStatus)
                    .HasColumnName("order_status")
                    .HasColumnType("smallint(6)");

                entity.Property(e => e.PayId)
                    .HasColumnName("pay_id")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.PayTime).HasColumnName("pay_time");

                entity.Property(e => e.ShipChannel)
                    .HasColumnName("ship_channel")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.ShipSn)
                    .HasColumnName("ship_sn")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.ShipTime).HasColumnName("ship_time");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<LitemallOrderGoods>(entity =>
            {
                entity.ToTable("litemall_order_goods", "litemall");

                entity.HasIndex(e => e.GoodsId)
                    .HasName("goods_id");

                entity.HasIndex(e => e.OrderId)
                    .HasName("order_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GoodsId)
                    .HasColumnName("goods_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.GoodsName)
                    .IsRequired()
                    .HasColumnName("goods_name")
                    .HasMaxLength(127)
                    .IsUnicode(false);

                entity.Property(e => e.GoodsSn)
                    .IsRequired()
                    .HasColumnName("goods_sn")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasColumnType("smallint(5)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.PicUrl)
                    .IsRequired()
                    .HasColumnName("pic_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("0.00");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Specifications)
                    .IsRequired()
                    .HasColumnName("specifications")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallPermission>(entity =>
            {
                entity.ToTable("litemall_permission", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Permission)
                    .HasColumnName("permission")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallRegion>(entity =>
            {
                entity.ToTable("litemall_region", "litemall");

                entity.HasIndex(e => e.Code)
                    .HasName("agency_id");

                entity.HasIndex(e => e.Pid)
                    .HasName("parent_id");

                entity.HasIndex(e => e.Type)
                    .HasName("region_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.Pid)
                    .HasColumnName("pid")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<LitemallRole>(entity =>
            {
                entity.ToTable("litemall_role", "litemall");

                entity.HasIndex(e => e.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Desc)
                    .HasColumnName("desc")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.Enabled)
                    .HasColumnName("enabled")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallSearchHistory>(entity =>
            {
                entity.ToTable("litemall_search_history", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.From)
                    .IsRequired()
                    .HasColumnName("from")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.Keyword)
                    .IsRequired()
                    .HasColumnName("keyword")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<LitemallStorage>(entity =>
            {
                entity.ToTable("litemall_storage", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Size)
                    .HasColumnName("size")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LitemallSystem>(entity =>
            {
                entity.ToTable("litemall_system", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.KeyName)
                    .IsRequired()
                    .HasColumnName("key_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.KeyValue)
                    .IsRequired()
                    .HasColumnName("key_value")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallTopic>(entity =>
            {
                entity.ToTable("litemall_topic", "litemall");

                entity.HasIndex(e => e.Id)
                    .HasName("topic_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .IsUnicode(false);

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Goods)
                    .HasColumnName("goods")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.PicUrl)
                    .HasColumnName("pic_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("0.00");

                entity.Property(e => e.ReadCount)
                    .HasColumnName("read_count")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("1k");

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("100");

                entity.Property(e => e.Subtitle)
                    .HasColumnName("subtitle")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");
            });

            modelBuilder.Entity<LitemallUser>(entity =>
            {
                entity.ToTable("litemall_user", "litemall");

                entity.HasIndex(e => e.Username)
                    .HasName("user_name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Avatar)
                    .IsRequired()
                    .HasColumnName("avatar")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("date");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LastLoginIp)
                    .IsRequired()
                    .HasColumnName("last_login_ip")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.LastLoginTime).HasColumnName("last_login_time");

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasColumnName("mobile")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasColumnName("nickname")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.SessionKey)
                    .IsRequired()
                    .HasColumnName("session_key")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UserLevel)
                    .HasColumnName("user_level")
                    .HasColumnType("tinyint(3)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.WeixinOpenid)
                    .IsRequired()
                    .HasColumnName("weixin_openid")
                    .HasMaxLength(63)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LitemallUserFormid>(entity =>
            {
                entity.ToTable("litemall_user_formid", "litemall");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnName("add_time");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ExpireTime).HasColumnName("expire_time");

                entity.Property(e => e.FormId)
                    .IsRequired()
                    .HasColumnName("formId")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.Isprepay)
                    .HasColumnName("isprepay")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.OpenId)
                    .IsRequired()
                    .HasColumnName("openId")
                    .HasMaxLength(63)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnName("update_time");

                entity.Property(e => e.UseAmount)
                    .HasColumnName("useAmount")
                    .HasColumnType("int(2)");
            });
        }
    }
}
