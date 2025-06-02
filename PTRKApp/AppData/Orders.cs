namespace PTRKApp.AppData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public int groupId { get; set; }

        [StringLength(500)]
        public string Customer { get; set; }

        [StringLength(500)]
        public string Deadline { get; set; }

        public int ServiceId { get; set; }

        public string StartWorkDate { get; set; }

        public virtual group group { get; set; }

        public virtual Services Services { get; set; }
    }
}
