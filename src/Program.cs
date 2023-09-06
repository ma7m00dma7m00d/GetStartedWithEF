// See https://aka.ms/new-console-template for more information
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

using (var db = new BloggingContext())
{
    // Bogus.Faker faker = new Bogus.Faker();

    // var blogList = new List<Blog>();
    // for (var i = 0; i < 10; i++)
    // {
    //     blogList.Add(new Blog
    //     {
    //         Url = faker.Internet.Url()
    //     });
    // }

    // db.Blogs.AddRange(blogList);
    // db.SaveChanges();

    // var postList = new List<Post>();

    // for (var i = 0; i < 30; i++)
    // {
    //     postList.Add(new Post
    //     {
    //         BlogId = blogList[Random.Shared.Next(blogList.Count)].BlogId,
    //         Title = faker.Lorem.Slug(3),
    //         Content = faker.Hacker.Phrase()
    //     });
    // }

    // db.Posts.AddRange(postList);
    // db.SaveChanges();

    var postsList = from post in db.Posts.Include(post => post.Blog)
                    where post.PostId > 21
                    select post;

    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(postsList, new System.Text.Json.JsonSerializerOptions
    {
        WriteIndented = true,
        ReferenceHandler =  ReferenceHandler.Preserve
    }));
}




public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public string DbPath { get; }

    public BloggingContext()
    {
        string dataDirectory = Path.Combine(Environment.CurrentDirectory, "Data");
        // var folder = Environment.SpecialFolder.LocalApplicationData;
        // var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(dataDirectory, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

[Table("Blogs")]
public class Blog
{
    [Key]
    public int BlogId { get; set; }

    [MinLength(5)]
    [MaxLength(255)]
    [DataType(DataType.Url)]
    [Required]
    public string Url { get; set; }

    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 2)]
    public string Title { get; set; }

    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}