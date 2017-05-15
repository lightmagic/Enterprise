using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace NewsMedia.Models
{
    public class ArticleContext : DbContext
    {
        public ArticleContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Article> ArticleCollection { get; set; }
    }
    [Table("Article")]
    public class Article
    {
        public int articleId { get; set; }
        public string Title { get; set; }
        public string subHeader { get; set; }
        public string Content { get; set; }
        public DateTime dateCreated { get; set; }
        public string imageArticle { get; set; }
        public bool breakingNews { get; set; }
        public int UserId { get; set; }
        public int categoryId { get; set; }
    }

    public class ArticleCreationModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The Header must not be over 50 characters long")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "SubHeader")]
        public string subHeader { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public string imageArticle { get; set; }

        [Display(Name = "Is the article a breaking news?")]
        public bool breakingNews { get; set; }

        [Display(Name = "Category")]
        public string categories { get; set; }

        public string username { get; set; }
    }

    [Table("Category")]
    public class Category
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
    }
}
