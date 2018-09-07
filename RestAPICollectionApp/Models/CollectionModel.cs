using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestAPICollectionApp.Models
{
    public class CollectionModel
    {
        public int CollectionModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<Item> Items { get; set; }
        
        
    }
}