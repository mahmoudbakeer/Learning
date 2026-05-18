using EF_Interceptors.Data;
using EF_Interceptors.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF_Interceptors.DataBaseHelper
{
    public static class DatabaseHelper
    {
        public static void ReCreateDataBase()
        {
            using var context = new AppDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static void PopulateDataBase()
        {
            using (var context = new AppDbContext())
            {
                context.Authors.Add(
                    new Author
                    {
                        Id = 1,
                        AuthorName = "Hadi",
                        Books = new List<Book>()
                        {
                            new Book
                            {
                                Id = 1,
                                AuthorId = 1,
                                BookName = "UnderTheWater",
                            },
                            new Book
                            {
                                Id = 2,
                                AuthorId = 1,
                                BookName = "AboveTheWater",
                            },
                            new Book
                            {
                                Id = 3,
                                AuthorId = 1,
                                BookName = "OnTheWater",
                            },
                        },
                    }
                );
                context.Authors.Add(
                    new Author
                    {
                        Id = 2,
                        AuthorName = "Mahmoud",
                        Books = new List<Book>()
                        {
                            new Book
                            {
                                Id = 4,
                                AuthorId = 2,
                                BookName = "Domain Driven Design",
                            },
                            new Book
                            {
                                Id = 5,
                                AuthorId = 2,
                                BookName = "DataBase Design For Dummies",
                            },
                            new Book
                            {
                                Id = 6,
                                AuthorId = 2,
                                BookName = "Depedency Injection .Net 10",
                            },
                        },
                    }
                );

                // save the Data
                context.SaveChanges();
            }
        }

        public static void PrintBooks()
        {
            Console.WriteLine("All Books in the system are : ");
            using (var context = new AppDbContext())
            {
                var authors = context.Authors.Include(author => author.Books);
                foreach (var item in authors)
                {
                    Console.WriteLine();
                    foreach (var book in item.Books)
                    {
                        Console.WriteLine(
                            $"BookId : {book.Id} - BookName : {book.BookName} - Author : {item.AuthorName} - IsDeleted : {book.IsDeleted}"
                        );
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
