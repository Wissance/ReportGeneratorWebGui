
namespace ReportGeneratorWeb.Models
{
    public class ReportFile
    {
        public ReportFile()
        {
        }

        public ReportFile(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; set; }
        public string Path { get; set; }
    }
}
