using Crash;

namespace CrashEdit
{
    public sealed class PaletteEntryController : MysteryMultiItemEntryController
    {
        private PaletteEntry t18entry;

        public PaletteEntryController(EntryChunkController entrychunkcontroller,PaletteEntry t18entry) : base(entrychunkcontroller,t18entry)
        {
            this.t18entry = t18entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Palette ({0} - {1})",t18entry.EName, t18entry.EID.ToString("X16"));
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public PaletteEntry PaletteEntry
        {
            get { return t18entry; }
        }
    }
}
