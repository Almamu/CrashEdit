using Crash;

namespace CrashEdit
{
    public sealed class DemoEntryController : MysteryUniItemEntryController
    {
        private DemoEntry demoentry;

        public DemoEntryController(EntryChunkController entrychunkcontroller,DemoEntry demoentry) : base(entrychunkcontroller,demoentry)
        {
            this.demoentry = demoentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Demo ({0} - {1})",demoentry.EName, demoentry.EID.ToString("X16"));
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public DemoEntry DemoEntry
        {
            get { return demoentry; }
        }
    }
}
