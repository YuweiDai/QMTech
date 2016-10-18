namespace QMTech.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        Description = c.String(),
                        MetaKeywords = c.String(maxLength: 400),
                        MetaDescription = c.String(),
                        MetaTitle = c.String(maxLength: 400),
                        ParentCategoryId = c.Int(nullable: false),
                        PictureId = c.Int(nullable: false),
                        HasDiscountsApplied = c.Boolean(nullable: false),
                        SubjectToAcl = c.Boolean(nullable: false),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Product_Category_Mapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        IsFeaturedProduct = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductTypeId = c.Int(nullable: false),
                        ParentGroupedProductId = c.Int(nullable: false),
                        VisibleIndividually = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 400),
                        ShortDescription = c.String(),
                        FullDescription = c.String(),
                        AdminComment = c.String(),
                        ProductTemplateId = c.Int(nullable: false),
                        MetaKeywords = c.String(maxLength: 400),
                        MetaDescription = c.String(),
                        MetaTitle = c.String(maxLength: 400),
                        AllowCustomerReviews = c.Boolean(nullable: false),
                        ApprovedRatingSum = c.Int(nullable: false),
                        NotApprovedRatingSum = c.Int(nullable: false),
                        ApprovedTotalReviews = c.Int(nullable: false),
                        NotApprovedTotalReviews = c.Int(nullable: false),
                        SubjectToAcl = c.Boolean(nullable: false),
                        Sku = c.String(maxLength: 400),
                        ManufacturerPartNumber = c.String(maxLength: 400),
                        IsGiftCard = c.Boolean(nullable: false),
                        GiftCardTypeId = c.Int(nullable: false),
                        IsRecurring = c.Boolean(nullable: false),
                        RecurringCycleLength = c.Int(nullable: false),
                        RecurringCyclePeriodId = c.Int(nullable: false),
                        RecurringTotalCycles = c.Int(nullable: false),
                        IsRental = c.Boolean(nullable: false),
                        RentalPriceLength = c.Int(nullable: false),
                        RentalPricePeriodId = c.Int(nullable: false),
                        ManageInventoryMethodId = c.Int(nullable: false),
                        StockQuantity = c.Int(nullable: false),
                        DisplayStockAvailability = c.Boolean(nullable: false),
                        DisplayStockQuantity = c.Boolean(nullable: false),
                        MinStockQuantity = c.Int(nullable: false),
                        LowStockActivityId = c.Int(nullable: false),
                        NotifyAdminForQuantityBelow = c.Int(nullable: false),
                        OrderMinimumQuantity = c.Int(nullable: false),
                        OrderMaximumQuantity = c.Int(nullable: false),
                        AllowedQuantities = c.String(maxLength: 1000),
                        DisableBuyButton = c.Boolean(nullable: false),
                        DisableWishlistButton = c.Boolean(nullable: false),
                        CallForPrice = c.Boolean(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OldPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ProductCost = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SpecialPrice = c.Decimal(precision: 18, scale: 4),
                        SpecialPriceStartDateTime = c.DateTime(),
                        SpecialPriceEndDateTime = c.DateTime(),
                        HasDiscountsApplied = c.Boolean(nullable: false),
                        AvailableStartDateTime = c.DateTime(),
                        AvailableEndDateTime = c.DateTime(),
                        DisplayOrder = c.Int(nullable: false),
                        Published = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        Store_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.Store_Id, cascadeDelete: true)
                .Index(t => t.Store_Id);
            
            CreateTable(
                "dbo.Product_Picture_Mapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        PictureId = c.Int(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.PictureId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.PictureId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PictureBinary = c.Binary(),
                        MimeType = c.String(),
                        SeoFilename = c.String(),
                        AltAttribute = c.String(),
                        TitleAttribute = c.String(),
                        IsNew = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductReview",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        Title = c.String(),
                        ReviewText = c.String(),
                        Rating = c.Int(nullable: false),
                        HelpfulYesTotal = c.Int(nullable: false),
                        HelpfulNoTotal = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        CustomerGuid = c.Guid(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductReviewHelpfulness",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductReviewId = c.Int(nullable: false),
                        WasHelpful = c.Boolean(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductReview", t => t.ProductReviewId, cascadeDelete: true)
                .Index(t => t.ProductReviewId);
            
            CreateTable(
                "dbo.ProductTag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        Test = c.String(),
                        MetaKeywords = c.String(maxLength: 400),
                        MetaDescription = c.String(),
                        MetaTitle = c.String(maxLength: 400),
                        DisplayOrder = c.Int(nullable: false),
                        CompanyAddress = c.String(nullable: false, maxLength: 400),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Store_Picture_Mapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        PictureId = c.Int(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        IsLogo = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.PictureId, cascadeDelete: true)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId)
                .Index(t => t.PictureId);
            
            CreateTable(
                "dbo.StoreTag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Product_ProductTag_Mapping",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        ProductTag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.ProductTag_Id })
                .ForeignKey("dbo.Product", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.ProductTag", t => t.ProductTag_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.ProductTag_Id);
            
            CreateTable(
                "dbo.StoreTagStores",
                c => new
                    {
                        StoreTag_Id = c.Int(nullable: false),
                        Store_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StoreTag_Id, t.Store_Id })
                .ForeignKey("dbo.StoreTag", t => t.StoreTag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Store", t => t.Store_Id, cascadeDelete: true)
                .Index(t => t.StoreTag_Id)
                .Index(t => t.Store_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Product_Category_Mapping", "ProductId", "dbo.Product");
            DropForeignKey("dbo.StoreTagStores", "Store_Id", "dbo.Store");
            DropForeignKey("dbo.StoreTagStores", "StoreTag_Id", "dbo.StoreTag");
            DropForeignKey("dbo.Store_Picture_Mapping", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Store_Picture_Mapping", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.Product", "Store_Id", "dbo.Store");
            DropForeignKey("dbo.Store", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.Product_ProductTag_Mapping", "ProductTag_Id", "dbo.ProductTag");
            DropForeignKey("dbo.Product_ProductTag_Mapping", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.ProductReviewHelpfulness", "ProductReviewId", "dbo.ProductReview");
            DropForeignKey("dbo.ProductReview", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductReview", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Product_Picture_Mapping", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product_Picture_Mapping", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.Product_Category_Mapping", "CategoryId", "dbo.Category");
            DropIndex("dbo.StoreTagStores", new[] { "Store_Id" });
            DropIndex("dbo.StoreTagStores", new[] { "StoreTag_Id" });
            DropIndex("dbo.Product_ProductTag_Mapping", new[] { "ProductTag_Id" });
            DropIndex("dbo.Product_ProductTag_Mapping", new[] { "Product_Id" });
            DropIndex("dbo.Store_Picture_Mapping", new[] { "PictureId" });
            DropIndex("dbo.Store_Picture_Mapping", new[] { "StoreId" });
            DropIndex("dbo.Store", new[] { "Category_Id" });
            DropIndex("dbo.ProductReviewHelpfulness", new[] { "ProductReviewId" });
            DropIndex("dbo.ProductReview", new[] { "ProductId" });
            DropIndex("dbo.ProductReview", new[] { "CustomerId" });
            DropIndex("dbo.Product_Picture_Mapping", new[] { "PictureId" });
            DropIndex("dbo.Product_Picture_Mapping", new[] { "ProductId" });
            DropIndex("dbo.Product", new[] { "Store_Id" });
            DropIndex("dbo.Product_Category_Mapping", new[] { "CategoryId" });
            DropIndex("dbo.Product_Category_Mapping", new[] { "ProductId" });
            DropTable("dbo.StoreTagStores");
            DropTable("dbo.Product_ProductTag_Mapping");
            DropTable("dbo.StoreTag");
            DropTable("dbo.Store_Picture_Mapping");
            DropTable("dbo.Store");
            DropTable("dbo.ProductTag");
            DropTable("dbo.ProductReviewHelpfulness");
            DropTable("dbo.Customers");
            DropTable("dbo.ProductReview");
            DropTable("dbo.Pictures");
            DropTable("dbo.Product_Picture_Mapping");
            DropTable("dbo.Product");
            DropTable("dbo.Product_Category_Mapping");
            DropTable("dbo.Category");
        }
    }
}
