using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldModelEntryController : EntryController
    {
        private OldModelEntry oldmodelentry;

        public OldModelEntryController(EntryChunkController entrychunkcontroller,OldModelEntry oldmodelentry) : base(entrychunkcontroller,oldmodelentry)
        {
            this.oldmodelentry = oldmodelentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Model ({0} - {1})",oldmodelentry.EName, oldmodelentry.EID.ToString("X16"));
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        // MONO USERS
        // Comment out this function
        protected override Control CreateEditor()
        {
            return new OldModelEntryViewer(oldmodelentry);
        }

        public OldModelEntry OldModelEntry
        {
            get { return oldmodelentry; }
        }
    }
}
