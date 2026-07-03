using System.ComponentModel.DataAnnotations;

namespace Domain.Base
{
    public abstract class Entity : IEntity
    {
        [Key]
        public virtual Guid Code { get; set; }

        public virtual DateTime? CreateTime { get; set; } = DateTime.Now;

        public virtual Guid? CreateUserCode { get; set; }

        public virtual DateTime? ModifyTime { get; set; }

        public virtual Guid? ModifyUserCode { get; set; }

        public virtual bool IsEnable { get; set; } = true;

        public virtual bool IsDelete { get; set; } = false;

    }
}
