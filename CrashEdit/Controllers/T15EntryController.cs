using Crash;

namespace CrashEdit
{
    public sealed class T15EntryController : MysteryUniItemEntryController
    {
        private T15Entry t15entry;

        public T15EntryController(EntryChunkController entrychunkcontroller,T15Entry t15entry) : base(entrychunkcontroller,t15entry)
        {
            this.t15entry = t15entry;
            Node.Text = string.Format("T15 Entry ({0})",t15entry.EIDString);
            Node.ImageKey = "t15entry";
            Node.SelectedImageKey = "t15entry";
        }

        public T15Entry T15Entry
        {
            get { return t15entry; }
        }
    }
}
