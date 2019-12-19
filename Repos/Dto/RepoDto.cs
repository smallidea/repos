using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repos.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class RepoDto
    {
        public string CodeNo { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        public string Content { get; set; }

        public int Id { get; set; }

        public string GitUrl { get; set; }

        public bool IsCheck { get; set; } = false;
    }
}
