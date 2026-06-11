namespace Domain.Entities
{
    public partial class SysDictionary
    {
        public virtual SysDictionary Parent { get; set; }
        public virtual ICollection<SysDictionary> Children { get; set; } = new HashSet<SysDictionary>();
    }
}
