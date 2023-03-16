namespace Timeoff.ResultModels
{
    public record DepartmentsViewModel
    {
        public IEnumerable<DepartmentResult> Departments { get; init; }

        public IEnumerable<ListItem> Users { get; init; }

        public IEnumerable<double> Allowances
        {
            get
            {
                for (double i = 0; i <= 50; i += 0.5)
                {
                    yield return i;
                }
            }
        }

        public FlashResult? Result { get; set; }

        public string GetName(int userId)
        {
            return Users.First(u => u.Id == userId).Value;
        }
    }
}