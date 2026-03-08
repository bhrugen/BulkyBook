using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Alba Solver", 1, "A thrilling adventure deep in the heart of an uncharted jungle, where ancient secrets and hidden treasures await those brave enough to seek them.", "JNG7777770001", null, 45.0, 40.0, 30.0, 35.0, "Store of the Jungle" },
                    { 2, "Bob Roarman", 3, "An insightful exploration of wealth, ambition, and the relentless passage of time. A story that challenges what it truly means to be rich.", "MNT8888880001", null, 55.0, 50.0, 40.0, 45.0, "Money and Time" },
                    { 3, "Jessica Alban", 1, "A mysterious tale set around a remote lake where strange occurrences lead a young detective to uncover a decades-old secret buried beneath the water.", "LKE9999990001", null, 35.0, 30.0, 25.0, 28.0, "Secret of the Lake" },
                    { 4, "Kristen Lober", 2, "A captivating journey through the cosmos, blending science and imagination as humanity reaches beyond the stars for the very first time.", "SPC1010100001", null, 65.0, 60.0, 50.0, 55.0, "Moon and Planets" },
                    { 5, "Laura Goldberg", 3, "At the edge of an ancient forest, dawn reveals more than just light. A poetic novel about renewal, loss, and the quiet strength found in nature.", "FRD1111110001", null, 30.0, 27.0, 20.0, 24.0, "Forest and Dawn" },
                    { 6, "Elara Vance", 1, "Deep within a forgotten grove, the trees hold memories of those who came before. A hauntingly beautiful story of legacy and belonging.", "WGR1212120001", null, 42.0, 38.0, 30.0, 34.0, "The Whispering Grove" },
                    { 7, "Anya Ravenwood", 2, "A brilliant cryptographer stumbles upon an ancient code that could rewrite history. But cracking it means confronting a conspiracy centuries in the making.", "CIP1313130001", null, 50.0, 45.0, 35.0, 40.0, "The Forgotten Cipher" },
                    { 8, "Elara Vance", 3, "When silence falls over a once-thriving orchard, one woman returns to her childhood home to uncover the truth behind its abandonment and her family's past.", "ORC1414140001", null, 38.0, 34.0, 26.0, 30.0, "The Silent Orchard" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
