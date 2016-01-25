using System;
using System.Collections.Generic;
using System.Text;

namespace Moon.PDFTemplate
{
    /// <summary>
    /// Group or rows. 
    /// Containts a list of rows.
    /// Normally its a section into the final document.
    /// <para/>Attributes: NONE
    /// </summary>
	public class RowGroup
    {
        /// <summary>
        /// Constructor.
        /// </summary>
		public RowGroup() { }
        
		private List<Row> rows = new List<Row>();
        private float height = 0;
        private float y = -1;

        /// <summary>
        /// Group height
        /// </summary>
        public float Height
        {
            get { return height; }
        }
        
        /// <summary>
        /// Group initial Y position
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        
        /// <summary>
        /// Rows into
        /// </summary>
        public List<Row> Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        /// <summary>
        /// Add a new row.
        /// </summary>
        /// <param name="row"></param>
        public void AddRow(Row row)
        {
            rows.Add(row);
            height += row.Height;
        }

    }
}
