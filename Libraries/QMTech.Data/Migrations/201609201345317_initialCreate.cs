namespace QMTech.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailAccount",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 255),
                        DisplayName = c.String(maxLength: 255),
                        Host = c.String(nullable: false, maxLength: 255),
                        Port = c.Int(nullable: false),
                        Username = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 255),
                        EnableSsl = c.Boolean(nullable: false),
                        UseDefaultCredentials = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageTemplate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        BccEmailAddresses = c.String(maxLength: 200),
                        Subject = c.String(maxLength: 1000),
                        Body = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        AttachedDownloadId = c.Int(nullable: false),
                        EmailAccountId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QueuedEmail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PriorityId = c.Int(nullable: false),
                        From = c.String(nullable: false, maxLength: 500),
                        FromName = c.String(maxLength: 500),
                        To = c.String(nullable: false, maxLength: 500),
                        ToName = c.String(maxLength: 500),
                        ReplyTo = c.String(maxLength: 500),
                        ReplyToName = c.String(maxLength: 500),
                        CC = c.String(maxLength: 500),
                        Bcc = c.String(maxLength: 500),
                        Subject = c.String(maxLength: 1000),
                        Body = c.String(),
                        AttachmentFilePath = c.String(),
                        AttachmentFileName = c.String(),
                        AttachedDownloadId = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        SentTries = c.Int(nullable: false),
                        SentOnUtc = c.DateTime(),
                        EmailAccountId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmailAccount", t => t.EmailAccountId, cascadeDelete: true)
                .Index(t => t.EmailAccountId);
            
            CreateTable(
                "dbo.ActivityLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityLogTypeId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Comment = c.String(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityLogType", t => t.ActivityLogTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.ActivityLogTypeId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.ActivityLogType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SystemKeyword = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 200),
                        Enabled = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 1000),
                        UserName = c.String(maxLength: 1000),
                        Active = c.Boolean(nullable: false),
                        CustomerGuid = c.Guid(nullable: false),
                        MobilePhone = c.String(),
                        LastIpAddress = c.String(),
                        LastActivityDate = c.DateTime(nullable: false),
                        LastLoginDate = c.DateTime(),
                        AdminComment = c.String(),
                        Password = c.String(),
                        PasswordFormatId = c.Int(nullable: false),
                        PasswordSalt = c.String(),
                        IsSystemAccount = c.Boolean(nullable: false),
                        SystemName = c.String(maxLength: 400),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Male = c.Boolean(nullable: false),
                        MobileNumber = c.String(),
                        FullAddress = c.String(),
                        Door = c.String(),
                        City = c.String(),
                        Lng = c.Double(nullable: false),
                        Lat = c.Double(nullable: false),
                        CustomAttributes = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Active = c.Boolean(nullable: false),
                        IsSystemRole = c.Boolean(nullable: false),
                        SystemName = c.String(maxLength: 255),
                        PurchasedWithProductId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PermissionRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SystemName = c.String(),
                        Category = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExternalAuthenticationRecord",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Email = c.String(),
                        ExternalIdentifier = c.String(),
                        ExternalDisplayIdentifier = c.String(),
                        OAuthToken = c.String(),
                        OAuthAccessToken = c.String(),
                        ProviderSystemName = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.RewardPointsHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        PointsBalance = c.Int(nullable: false),
                        UsedAmount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Message = c.String(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogLevelId = c.Int(nullable: false),
                        ShortMessage = c.String(nullable: false),
                        FullMessage = c.String(),
                        IpAddress = c.String(maxLength: 200),
                        CustomerId = c.Int(),
                        PageUrl = c.String(),
                        ReferrerUrl = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Value = c.String(nullable: false, maxLength: 2000),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        Description = c.String(),
                        MetaKeywords = c.String(maxLength: 400),
                        MetaDescription = c.String(),
                        MetaTitle = c.String(maxLength: 400),
                        ParentId = c.Int(nullable: false),
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
                "dbo.Store",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        Address = c.String(maxLength: 400),
                        DisplayOrder = c.Int(nullable: false),
                        Lon = c.Single(nullable: false),
                        Lat = c.Single(nullable: false),
                        Tel = c.String(),
                        Description = c.String(),
                        Noticement = c.String(),
                        Stars = c.Single(nullable: false),
                        MetaKeywords = c.String(maxLength: 400),
                        MetaDescription = c.String(),
                        MetaTitle = c.String(maxLength: 400),
                        SelfSupport = c.Boolean(nullable: false),
                        Person = c.String(),
                        PersonTel = c.String(),
                        SalesNumber = c.String(),
                        AllOpened = c.Boolean(nullable: false),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        Published = c.Boolean(nullable: false),
                        Closed = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        StoreCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreCategory", t => t.StoreCategory_Id, cascadeDelete: true)
                .Index(t => t.StoreCategory_Id);
            
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
                        ProductCategory_Id = c.Int(nullable: false),
                        Store_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategory", t => t.ProductCategory_Id, cascadeDelete: true)
                .ForeignKey("dbo.Store", t => t.Store_Id, cascadeDelete: true)
                .Index(t => t.ProductCategory_Id)
                .Index(t => t.Store_Id);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        Description = c.String(),
                        MetaKeywords = c.String(maxLength: 400),
                        MetaDescription = c.String(),
                        MetaTitle = c.String(maxLength: 400),
                        HasDiscountsApplied = c.Boolean(nullable: false),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.ProductId);
            
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
                "dbo.Store_Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagId = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                        Store_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .ForeignKey("dbo.Store", t => t.Store_Id)
                .Index(t => t.StoreId)
                .Index(t => t.Store_Id);
            
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HashId = c.String(),
                        Subject = c.String(),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerAddresses",
                c => new
                    {
                        Customer_Id = c.Int(nullable: false),
                        Address_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Customer_Id, t.Address_Id })
                .ForeignKey("dbo.Customer", t => t.Customer_Id, cascadeDelete: true)
                .ForeignKey("dbo.Addresses", t => t.Address_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id)
                .Index(t => t.Address_Id);
            
            CreateTable(
                "dbo.PermissionRecordCustomerRoles",
                c => new
                    {
                        PermissionRecord_Id = c.Int(nullable: false),
                        CustomerRole_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PermissionRecord_Id, t.CustomerRole_Id })
                .ForeignKey("dbo.PermissionRecords", t => t.PermissionRecord_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerRole", t => t.CustomerRole_Id, cascadeDelete: true)
                .Index(t => t.PermissionRecord_Id)
                .Index(t => t.CustomerRole_Id);
            
            CreateTable(
                "dbo.Customer_CustomerRole_Mapping",
                c => new
                    {
                        Customer_Id = c.Int(nullable: false),
                        CustomerRole_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Customer_Id, t.CustomerRole_Id })
                .ForeignKey("dbo.Customer", t => t.Customer_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerRole", t => t.CustomerRole_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id)
                .Index(t => t.CustomerRole_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Store", "StoreCategory_Id", "dbo.StoreCategory");
            DropForeignKey("dbo.Store_Tags", "Store_Id", "dbo.Store");
            DropForeignKey("dbo.Store_Tags", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Store_Picture_Mapping", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Store_Picture_Mapping", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.Product", "Store_Id", "dbo.Store");
            DropForeignKey("dbo.Product_ProductTag_Mapping", "ProductTag_Id", "dbo.ProductTag");
            DropForeignKey("dbo.Product_ProductTag_Mapping", "Product_Id", "dbo.Product");
            DropForeignKey("dbo.ProductReviewHelpfulness", "ProductReviewId", "dbo.ProductReview");
            DropForeignKey("dbo.ProductReview", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductReview", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Product_Picture_Mapping", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product_Picture_Mapping", "PictureId", "dbo.Pictures");
            DropForeignKey("dbo.Product", "ProductCategory_Id", "dbo.ProductCategory");
            DropForeignKey("dbo.Log", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.ActivityLog", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.RewardPointsHistory", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.ExternalAuthenticationRecord", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Customer_CustomerRole_Mapping", "CustomerRole_Id", "dbo.CustomerRole");
            DropForeignKey("dbo.Customer_CustomerRole_Mapping", "Customer_Id", "dbo.Customer");
            DropForeignKey("dbo.PermissionRecordCustomerRoles", "CustomerRole_Id", "dbo.CustomerRole");
            DropForeignKey("dbo.PermissionRecordCustomerRoles", "PermissionRecord_Id", "dbo.PermissionRecords");
            DropForeignKey("dbo.CustomerAddresses", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.CustomerAddresses", "Customer_Id", "dbo.Customer");
            DropForeignKey("dbo.ActivityLog", "ActivityLogTypeId", "dbo.ActivityLogType");
            DropForeignKey("dbo.QueuedEmail", "EmailAccountId", "dbo.EmailAccount");
            DropIndex("dbo.Product_ProductTag_Mapping", new[] { "ProductTag_Id" });
            DropIndex("dbo.Product_ProductTag_Mapping", new[] { "Product_Id" });
            DropIndex("dbo.Customer_CustomerRole_Mapping", new[] { "CustomerRole_Id" });
            DropIndex("dbo.Customer_CustomerRole_Mapping", new[] { "Customer_Id" });
            DropIndex("dbo.PermissionRecordCustomerRoles", new[] { "CustomerRole_Id" });
            DropIndex("dbo.PermissionRecordCustomerRoles", new[] { "PermissionRecord_Id" });
            DropIndex("dbo.CustomerAddresses", new[] { "Address_Id" });
            DropIndex("dbo.CustomerAddresses", new[] { "Customer_Id" });
            DropIndex("dbo.Store_Tags", new[] { "Store_Id" });
            DropIndex("dbo.Store_Tags", new[] { "StoreId" });
            DropIndex("dbo.Store_Picture_Mapping", new[] { "PictureId" });
            DropIndex("dbo.Store_Picture_Mapping", new[] { "StoreId" });
            DropIndex("dbo.ProductReviewHelpfulness", new[] { "ProductReviewId" });
            DropIndex("dbo.ProductReview", new[] { "ProductId" });
            DropIndex("dbo.ProductReview", new[] { "CustomerId" });
            DropIndex("dbo.Product_Picture_Mapping", new[] { "PictureId" });
            DropIndex("dbo.Product_Picture_Mapping", new[] { "ProductId" });
            DropIndex("dbo.Product", new[] { "Store_Id" });
            DropIndex("dbo.Product", new[] { "ProductCategory_Id" });
            DropIndex("dbo.Store", new[] { "StoreCategory_Id" });
            DropIndex("dbo.Log", new[] { "CustomerId" });
            DropIndex("dbo.RewardPointsHistory", new[] { "CustomerId" });
            DropIndex("dbo.ExternalAuthenticationRecord", new[] { "CustomerId" });
            DropIndex("dbo.ActivityLog", new[] { "CustomerId" });
            DropIndex("dbo.ActivityLog", new[] { "ActivityLogTypeId" });
            DropIndex("dbo.QueuedEmail", new[] { "EmailAccountId" });
            DropTable("dbo.Product_ProductTag_Mapping");
            DropTable("dbo.Customer_CustomerRole_Mapping");
            DropTable("dbo.PermissionRecordCustomerRoles");
            DropTable("dbo.CustomerAddresses");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.Store_Tags");
            DropTable("dbo.Store_Picture_Mapping");
            DropTable("dbo.ProductTag");
            DropTable("dbo.ProductReviewHelpfulness");
            DropTable("dbo.ProductReview");
            DropTable("dbo.Pictures");
            DropTable("dbo.Product_Picture_Mapping");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Product");
            DropTable("dbo.Store");
            DropTable("dbo.StoreCategory");
            DropTable("dbo.Setting");
            DropTable("dbo.Log");
            DropTable("dbo.RewardPointsHistory");
            DropTable("dbo.ExternalAuthenticationRecord");
            DropTable("dbo.PermissionRecords");
            DropTable("dbo.CustomerRole");
            DropTable("dbo.Addresses");
            DropTable("dbo.Customer");
            DropTable("dbo.ActivityLogType");
            DropTable("dbo.ActivityLog");
            DropTable("dbo.QueuedEmail");
            DropTable("dbo.MessageTemplate");
            DropTable("dbo.EmailAccount");
        }
    }
}
