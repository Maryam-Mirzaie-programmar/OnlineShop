//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datalayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Product_Gallery = new HashSet<Product_Gallery>();
            this.Product_SelectedGroups = new HashSet<Product_SelectedGroups>();
            this.Product_Tags = new HashSet<Product_Tags>();
        }
    
        public int ProductID { get; set; }
        public string ProductTitle { get; set; }
        public string ProductShortDescription { get; set; }
        public string ProductText { get; set; }
        public int ProductPrice { get; set; }
        public string ProductImageName { get; set; }
        public System.DateTime ProductCreateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Gallery> Product_Gallery { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_SelectedGroups> Product_SelectedGroups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Tags> Product_Tags { get; set; }
    }
}