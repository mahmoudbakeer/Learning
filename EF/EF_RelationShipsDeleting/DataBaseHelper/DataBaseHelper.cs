using EF_RelationShipsDeleting.Data;
using EF_RelationShipsDeleting.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF_RelationShipsDeleting.DataBaseHelper
{
    public static class DatabaseHelper
    {
        public static void RecreateCleanDatabase()
        {
            using var context = new AppDbContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static void PopulateDatabase()
        {
            using (var context = new AppDbContext())
            {
                context.Authors.Add(
                    new Author
                    {
                        Id = 1,
                        AuthorName = "Hadi",
                        Books = new List<Book>
                        {
                            new Book
                            {
                                Id = 1,
                                Price = 100m,
                                BookName =
                                    "Domain-Driven Design: Tackling Complexity in the Heart of Software",
                            },
                            new Book
                            {
                                Id = 2,
                                Price = 101m,
                                BookName =
                                    "Domain-Driven Design Reference: Definitions and Pattern Summaries",
                            },
                        },
                    }
                );

                context.AuthorV2s.Add(
                    new AuthorV2
                    {
                        Id = 1,
                        AuthorName = "Hadi",
                        BookV2s = new List<BookV2>
                        {
                            new BookV2
                            {
                                Id = 1,
                                BookName =
                                    "Domain-Driven Design: Tackling Complexity in the Heart of Software",
                            },
                            new BookV2
                            {
                                Id = 2,
                                BookName =
                                    "Domain-Driven Design Reference: Definitions and Pattern Summaries",
                            },
                        },
                    }
                );

                context.SaveChanges();
            }
        }

        public static Book GetDisconnectedBook()
        {
            using var tempContext = new AppDbContext();
            return tempContext.Books.Find(2);
        }

        public static Author GetDisconnectedAuthorAndBooks()
        {
            using var tempContext = new AppDbContext();
            return tempContext.Authors.Include(x => x.Books).Single();
        }
    }
}
