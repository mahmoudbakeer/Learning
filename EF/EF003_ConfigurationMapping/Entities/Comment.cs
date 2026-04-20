using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EF003_ConfigurationMapping.Entities
{
    // three ways to configure the mapping in EF
    // 1. by convention
    // 2. by using annotation 'it make the code dirty'
    // 3.use fluent api
    // 4. use grouping configuration

    // the table in the data base called tblComments
    //[Table("tblComments")] // the first one 
    // as the same you can use the [Column(columnName)]
    public class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public int TweetId { get; set; }
        public DateTime CreatedAt {  get; set; }
    }
}
