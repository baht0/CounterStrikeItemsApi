using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CounterStrikeItemsApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    HexColor = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollectionTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exteriors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exteriors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GraffitiColors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    HexColor = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraffitiColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalPlayers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalPlayers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Qualities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    HexColor = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SteamUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SteamId = table.Column<string>(type: "text", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteamUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subtypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collections_CollectionTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CollectionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_SteamUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SteamUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypeSubtypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubtypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypeSubtypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTypeSubtypes_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTypeSubtypes_Subtypes_SubtypeId",
                        column: x => x.SubtypeId,
                        principalTable: "Subtypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemCommons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubtypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    TournamentId = table.Column<Guid>(type: "uuid", nullable: true),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProfessionalPlayerId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCommons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCommons_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemCommons_ItemTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemCommons_ProfessionalPlayers_ProfessionalPlayerId",
                        column: x => x.ProfessionalPlayerId,
                        principalTable: "ProfessionalPlayers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemCommons_Subtypes_SubtypeId",
                        column: x => x.SubtypeId,
                        principalTable: "Subtypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemCommons_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemCommons_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Founds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemCommonId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContainerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Founds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Founds_ItemCommons_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "ItemCommons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Founds_ItemCommons_ItemCommonId",
                        column: x => x.ItemCommonId,
                        principalTable: "ItemCommons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    ItemNameId = table.Column<long>(type: "bigint", nullable: true),
                    ImageId = table.Column<string>(type: "text", nullable: true),
                    ItemCommonId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExteriorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    QualityId = table.Column<Guid>(type: "uuid", nullable: true),
                    GraffitiColorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Exteriors_ExteriorId",
                        column: x => x.ExteriorId,
                        principalTable: "Exteriors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_GraffitiColors_GraffitiColorId",
                        column: x => x.GraffitiColorId,
                        principalTable: "GraffitiColors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_ItemCommons_ItemCommonId",
                        column: x => x.ItemCommonId,
                        principalTable: "ItemCommons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Qualities_QualityId",
                        column: x => x.QualityId,
                        principalTable: "Qualities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemOrdersHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HighestBuyOrder = table.Column<long>(type: "bigint", nullable: true),
                    LowestSellOrder = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemOrdersHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemOrdersHistories_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collections_Slug",
                table: "Collections",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collections_TypeId",
                table: "Collections",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionTypes_Slug",
                table: "CollectionTypes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exteriors_Slug",
                table: "Exteriors",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Founds_ContainerId",
                table: "Founds",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Founds_ItemCommonId",
                table: "Founds",
                column: "ItemCommonId");

            migrationBuilder.CreateIndex(
                name: "IX_GraffitiColors_Slug",
                table: "GraffitiColors",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCommons_CollectionId",
                table: "ItemCommons",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCommons_ProfessionalPlayerId",
                table: "ItemCommons",
                column: "ProfessionalPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCommons_Slug",
                table: "ItemCommons",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCommons_SubtypeId",
                table: "ItemCommons",
                column: "SubtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCommons_TeamId",
                table: "ItemCommons",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCommons_TournamentId",
                table: "ItemCommons",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCommons_TypeId",
                table: "ItemCommons",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrdersHistories_ItemId",
                table: "ItemOrdersHistories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ExteriorId",
                table: "Items",
                column: "ExteriorId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_GraffitiColorId",
                table: "Items",
                column: "GraffitiColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemCommonId",
                table: "Items",
                column: "ItemCommonId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_QualityId",
                table: "Items",
                column: "QualityId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Slug",
                table: "Items",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_Slug",
                table: "ItemTypes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypeSubtypes_ItemTypeId",
                table: "ItemTypeSubtypes",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypeSubtypes_SubtypeId",
                table: "ItemTypeSubtypes",
                column: "SubtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalPlayers_Slug",
                table: "ProfessionalPlayers",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Qualities_Slug",
                table: "Qualities",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtypes_Slug",
                table: "Subtypes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Slug",
                table: "Teams",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_Slug",
                table: "Tournaments",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Founds");

            migrationBuilder.DropTable(
                name: "ItemOrdersHistories");

            migrationBuilder.DropTable(
                name: "ItemTypeSubtypes");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "SteamUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Exteriors");

            migrationBuilder.DropTable(
                name: "GraffitiColors");

            migrationBuilder.DropTable(
                name: "ItemCommons");

            migrationBuilder.DropTable(
                name: "Qualities");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "ProfessionalPlayers");

            migrationBuilder.DropTable(
                name: "Subtypes");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "CollectionTypes");
        }
    }
}
