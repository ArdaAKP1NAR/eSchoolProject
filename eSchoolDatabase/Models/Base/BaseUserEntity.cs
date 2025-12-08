namespace eSchoolDatabase.Models.Base
{

    public class BaseUserEntity :BaseEntity
    {
        public string IdentityNumber { get; set; } = default!;
    }
}
