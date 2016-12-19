using Crash;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldModelEntryViewer : UserControl
    {
        private TabControl tbcTabs;

        public OldModelEntryViewer(OldModelEntry chunk)
        {
            tbcTabs = new TabControl();
            tbcTabs.Dock = DockStyle.Fill;
            {
                MysteryBox mystery = new MysteryBox(chunk.Info);
                mystery.Dock = DockStyle.Fill;
                TabPage page = new TabPage("Model Info");
                page.Controls.Add(mystery);
                tbcTabs.TabPages.Add(page);
            }
            {
                byte[] polygonsData = new byte[chunk.Polygons.Count * 8];
                for (int i = 0; i < chunk.Polygons.Count; i++)
                {
                    chunk.Polygons[i].Save().CopyTo(polygonsData, i * 8);
                }

                MysteryBox mystery = new MysteryBox(polygonsData);
                mystery.Dock = DockStyle.Fill;
                TabPage page = new TabPage("Model polygons");
                page.Controls.Add(mystery);
                tbcTabs.TabPages.Add(page);
            }
            Controls.Add(tbcTabs);
        }
    }
}
