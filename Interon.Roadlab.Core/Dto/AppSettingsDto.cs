using System.Collections.Generic;

namespace Interon.Roadlab.Core.Dto

{
   
    public class TestCategoryDto
    {
        public int  Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class TestDto
    {
        public int Id { get; set; }
        public int TestCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AppSettingsDto
    {
       
        public string PlaystoreLink { get; set; }
        public string AppleLink { get; set; }
       
        public string VersionString { get; set; }
        public string BuildString { get; set; }
        public bool ForceUpdate { get; set; }
        public bool ForceReinstall { get; set; }


    }

}