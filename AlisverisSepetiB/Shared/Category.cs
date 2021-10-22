using System.Collections.Generic;

namespace AlisverisSepetiB.Shared
{
    public class Category
    {
        public int id { get; set; }
        public string categoryName { get; set; }
        
        public List<Category>? SubCategory { get; set; }
    }
}
