namespace Stories.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class STR_RateSubject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public STR_RateSubject()
        {
            STR_RateOfStory = new HashSet<STR_RateOfStory>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Caption { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STR_RateOfStory> STR_RateOfStory { get; set; }
    }
}
