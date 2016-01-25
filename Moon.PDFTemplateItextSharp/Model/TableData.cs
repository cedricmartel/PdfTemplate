using System.Collections;
using System.Collections.Generic;

namespace Moon.PDFTemplateItextSharp.Model
{
    /// <summary>
    /// Data for a templated tabel 
    /// </summary>
    public class TableData
    {
        /// <summary>
        /// table header data
        /// </summary>
        public Hashtable HeadData { get; set; }
		/// <summary>
		/// table data. Table won't show if null
		/// </summary>
		public List<Hashtable> LoopData { get; set; }
		/// <summary>
		/// data for footer
		/// </summary>
		public Hashtable FootData { get; set; }
    }
}
