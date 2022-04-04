using System; 
using NLog.Web;
using System.IO;
using System.Linq;


namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            try
            {

                Console.WriteLine("Select an Option: \n Press 1 to display blogs \n Press 2 to add blog \n Press 3 to create a post \n Press 4 to display posts");
                string userChoice = Console.ReadLine();

                if (userChoice == "1")
                {
                    var db = new BloggingContext();
                    // Display all Blogs from the database
                    var query = db.Blogs.OrderBy(b => b.Name);

                    Console.WriteLine("All blogs in the database:");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name);
                    }
                } else if (userChoice == "2")
                {
                    // Create and save a new Blog
                    Console.Write("Enter a name for a new Blog: ");
                    var name = Console.ReadLine();

                    var blog = new Blog { Name = name };

                    var db = new BloggingContext();
                    db.AddBlog(blog);
                    logger.Info("Blog added - {name}", name);
                } else if (userChoice =="3"){
                    //Prompt user to select Blog they are posting to
                    Console.WriteLine("What blog would you like to post to (select blog ID)?");
                    var db = new BloggingContext();
                    var query2 = db.Blogs.OrderBy(b => b.BlogId);
                    foreach (var item in query2)
                    {
                        Console.Write("Blog ID: " + item.BlogId + " | ");
                        Console.WriteLine("Blog Name: " + item.Name);
                    }
                    Console.WriteLine("Blog ID to add post to: ");
                    int blogID = Int32.Parse(Console.ReadLine());
                    //Console.WriteLine("Enter post ID: ");
                    //int postID = Int32.Parse(Console.ReadLine());
                    Console.WriteLine("Enter post title: ");
                    string title = Console.ReadLine();
                    Console.WriteLine("Enter post content: ");
                    string content = Console.ReadLine();
                    //Once blog is selected, post details can be entered
                    var post = new Post {
                        BlogId = blogID,
                        //PostId = postID,
                        Title = title,
                        Content = content
                     };
                    //posts should be saved to the post table
                    db.AddPost(post);

                    //user errors must be handled 

                } else if (userChoice == "4"){
                    //prompt user to select blog whose posts they want to view
                    Console.WriteLine("Which blog posts would you like to view? (Enter blog ID): ");
                    var db = new BloggingContext();
                    var query2 = db.Blogs.OrderBy(b => b.BlogId);
                    foreach (var item in query2)
                    {
                        Console.Write("Blog ID: " + item.BlogId + " | ");
                        Console.WriteLine("Blog Name: " + item.Name);
                    }
                    int blogChoice = Int32.Parse(Console.ReadLine());
                    //all posts related to selected blog should be displayed as well as # of posts
                    var query3 = db.Posts.Where(p => p.BlogId==blogChoice).OrderByDescending(p => p.PostId);
                    //for each post, display the blog name, post title, and post content
                    foreach (var item in query3)
                    {
                        Console.Write("Blog name: " + item.Blog.Name);
                        Console.Write(", Blog post: " + item.Title);
                        Console.WriteLine(", Blog content: " + item.Content);
                    }

                } else {
                    Console.WriteLine("Invalid input.");
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}