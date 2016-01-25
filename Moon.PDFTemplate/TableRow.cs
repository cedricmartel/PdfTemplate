using System;
using System.Collections.Generic;

using System.Text;

namespace Moon.PDFTemplate
{
	/// <summary>
	/// Table row. Contains a list of table cells. Isnt' a element.
	/// 
	/// </summary>
	public class TableRow
	{
		private List<TableCell> tableCells = new List<TableCell>();
		
		/// <summary>
		/// List of table cells
		/// </summary>
		public List<TableCell> TableCells
		{
			get { return tableCells; }
		}
		
		/// <summary>
		/// Add a new table cell
		/// </summary>
		/// <param name="tableCell"></param>
		public void AddTableCell(TableCell tableCell)
		{
			tableCells.Add(tableCell);
		}
	}
}
