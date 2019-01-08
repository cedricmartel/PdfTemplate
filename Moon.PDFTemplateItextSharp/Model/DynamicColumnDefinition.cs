
namespace Moon.PDFTemplateItextSharp.Model
{
    /// <summary>
    /// This class modelizes a dynamic column definition, that will be injected in template at execution time
    /// </summary>
    public class DynamicColumnDefinition
    {
        /// <summary>
        /// Width for this column, must be >= 1
        /// </summary>
        public int CellWidth { get; set; } = 1;

        /// <summary>
        /// define the template for header ex:
        /// "<tablecell border="Top, Bottom" backgroundcolor="#9BCFF9"><textbox text = "Montant" align="right"></textbox></tablecell>"
        /// </summary>
        public string HeaderTemplate { get; set; }

        /// <summary>
        /// define the template for data cells
        /// </summary>
        public string DataTemplate { get; set; }

        /// <summary>
        /// define the template for footer cells
        /// </summary>
        public string FooterTemplate { get; set; }
    }
}
