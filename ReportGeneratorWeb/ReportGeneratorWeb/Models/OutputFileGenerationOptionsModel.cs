using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportGeneratorWeb.Models
{
    public class OutputFileGenerationOptionsModel
    {
        public OutputFileGenerationOptionsModel()
        {

        }

        public OutputFileGenerationOptionsModel(int worksheet, int row, int column)
        {
            Worksheet = worksheet;
            Row = row;
            Column = column;
        }

        public int Worksheet { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
