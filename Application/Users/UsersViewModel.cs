namespace Timeoff.Application.Users
{
    public record UsersViewModel
    {
        public string CompanyName { get; init; } = null!;

        public int? DepartmentId { get; init; }

        public IEnumerable<ResultModels.ListItem> Departments { get; init; } = null!;

        public IEnumerable<ResultModels.UserInfoResult> Users { get; init; } = null!;
    }
}