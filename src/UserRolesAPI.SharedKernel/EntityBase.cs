namespace UserRolesAPI.SharedKernel;

public abstract class EntityBase    // this can be modified to EntityBase<TId> to support multiple key types (e.g. Guid)
{
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? Updated { get; set; } = DateTime.Now;
}
