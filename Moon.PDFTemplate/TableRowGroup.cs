using System.Collections.Generic;

namespace Moon.PDFTemplate
{
	/// <summary>
	/// Table row group.
	/// Contains a list of TableRows.
	/// Itsn't a element.
	/// </summary>
	public class TableRowGroup
	{
		private readonly List<TableRow> tableRows = new List<TableRow>();
		
		/// <summary>
		/// List of table rows.
		/// </summary>
		public List<TableRow> TableRows
		{
			get { return tableRows; }
		}
		
		/// <summary>
		/// Add a new table row.
		/// </summary>
		/// <param name="tableRow"></param>
		public void AddTableRow(TableRow tableRow)
		{
			tableRows.Add(tableRow);
		}
	}
}
