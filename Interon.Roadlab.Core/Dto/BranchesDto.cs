using System.Collections.Generic;

namespace Interon.Roadlab.Core.Dto
{
    public class BranchesDto
    {
        public List<string> Ids { get; set; } = new List<string>();
        public List<BranchDto> branchDtos { get; set; } = new List<BranchDto>();
    }
}
