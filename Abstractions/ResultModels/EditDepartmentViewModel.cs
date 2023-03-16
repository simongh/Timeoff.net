namespace Timeoff.ResultModels
{
    public record EditDepartmentViewModel : Types.DepartmentModel
    {
        public int Id { get; init; }

        public IEnumerable<ListItem> Users { get; init; }

        public FlashResult? Result { get; set; }

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
    }
}