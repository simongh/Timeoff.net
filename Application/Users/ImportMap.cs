namespace Timeoff.Application.Users
{
    internal class ImportMap : CsvHelper.Configuration.ClassMap<ImportModel>
    {
        public ImportMap()
        {
            Map(m => m.Email).Name("Email");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.Team).Name("Team");
        }
    }
}