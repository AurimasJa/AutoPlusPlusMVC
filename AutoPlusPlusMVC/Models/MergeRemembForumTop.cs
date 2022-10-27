using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPlusPlusMVC.Models
{
    public class MergeRemembForumTop
    {
        public Forum_topic forum_Topic { get; set; }
        public Forum_topic_remembrance forum_Topic_Remembrances { get; set; }
        public Forum_answer forum_Answer { get; set; }
    }
}
