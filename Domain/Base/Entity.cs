using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Interface.IAggregateRoots
{
    public abstract class Entity : IEntity
    {
        [Key]
        public virtual Guid Code { get; set; }

        public virtual DateTime? CreateTime { get; set; }

        public virtual Guid? CreateUserCode { get; set; }

        public virtual DateTime? ModifyTime { get; set; }

        public virtual Guid? ModifyUserCode { get; set; }

        public virtual bool IsEnable { get; set; } = true;

        public virtual bool IsDelete { get; set; } = false;

    }
}
