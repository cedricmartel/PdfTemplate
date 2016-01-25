using System;
using System.IO;

namespace fr.cedricmartel.SampleItextSharp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void EmptyOutput_OnClick(object sender, EventArgs e)
        {
            var di = new DirectoryInfo(Server.MapPath("Output"));
            int nbFilesDeleted = 0;
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
                nbFilesDeleted ++;
            }
            EmptyOutputResult.Text = nbFilesDeleted + " files removed";
        }
    }
}