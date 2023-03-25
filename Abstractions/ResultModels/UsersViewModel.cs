namespace Timeoff.ResultModels
{
    public record UsersViewModel
    {
        public string CompanyName { get; init; } = null!;

        public int? DepartmentId { get; init; }

        public IEnumerable<ListItem> Departments { get; init; } = null!;

        public IEnumerable<UserInfoResult> Users { get; init; } = null!;
    }
}