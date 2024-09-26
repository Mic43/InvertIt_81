namespace InvertItService.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "InvertIt.Challenges",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Difficulty = c.Int(nullable: false),
                        PointsCalculatorType = c.Int(nullable: false),
                        Board_Size = c.Int(nullable: false),
                        Board_MovesCollection = c.String(),
                        Board_MovesCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "InvertIt.Games",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PointsAwarded = c.Int(nullable: false),
                        GameState = c.Int(nullable: false),
                        MovesCount = c.Int(nullable: false),
                        PlayDuration = c.Time(nullable: false, precision: 7),
                        Challenge_Id = c.Guid(),
                        UserAccount_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("InvertIt.Challenges", t => t.Challenge_Id)
                .ForeignKey("InvertIt.UserAccounts", t => t.UserAccount_Id)
                .Index(t => t.Challenge_Id)
                .Index(t => t.UserAccount_Id);
            
            CreateTable(
                "InvertIt.UserAccounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
                        Username = c.String(),
                        Salt = c.Binary(),
                        SaltedAndHashedPassword = c.Binary(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
                        Deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CreatedAt, clustered: true);
            
            CreateTable(
                "InvertIt.UserRankings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserAccountId = c.String(maxLength: 128),
                        PointsEasy = c.Int(nullable: false),
                        PointsMedium = c.Int(nullable: false),
                        PointsHard = c.Int(nullable: false),
                        ChallengesFinished = c.Int(nullable: false),
                        ChallengesLeft = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("InvertIt.UserAccounts", t => t.UserAccountId)
                .Index(t => t.UserAccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("InvertIt.UserRankings", "UserAccountId", "InvertIt.UserAccounts");
            DropForeignKey("InvertIt.Games", "UserAccount_Id", "InvertIt.UserAccounts");
            DropForeignKey("InvertIt.Games", "Challenge_Id", "InvertIt.Challenges");
            DropIndex("InvertIt.UserRankings", new[] { "UserAccountId" });
            DropIndex("InvertIt.UserAccounts", new[] { "CreatedAt" });
            DropIndex("InvertIt.Games", new[] { "UserAccount_Id" });
            DropIndex("InvertIt.Games", new[] { "Challenge_Id" });
            DropTable("InvertIt.UserRankings");
            DropTable("InvertIt.UserAccounts",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
            DropTable("InvertIt.Games");
            DropTable("InvertIt.Challenges");
        }
    }
}
