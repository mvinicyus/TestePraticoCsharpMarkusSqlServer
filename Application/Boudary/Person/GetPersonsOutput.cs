
namespace Application.Boudary.Person
{
    public class GetPersonsOutput
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; internal set; }
        public IEnumerable<string[]> Data { get; set; }
    }
}
