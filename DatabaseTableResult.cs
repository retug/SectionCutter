using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionCutter
{
    public class DatabaseTableResult
    {
        public string [] FieldKeyList { get; set; }
        public int TableVersion { get; set; }
        public string [] FieldKeysIncluded { get; set; }
        public int NumberRecords { get; set; }
        public string [] TableData { get; set;  }

    }

    public class DatabaseTableInfo
    {
        public bool FillImportLog { get; set; }
        public int NumFatalErrors { get; set; }

        public int NumErrorMsgs { get; set; }
        public int NumWarnMsgs { get; set; }
        public int NumInfoMsgs { get; set; }
        public string ImportLog { get; set; }

    }
}
