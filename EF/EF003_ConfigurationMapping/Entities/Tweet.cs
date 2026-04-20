using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF003_ConfigurationMapping.Entities
{
    //[Table("tblTweets")]


    public class Tweet
    {
        // [Column("Id")]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TweetText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
