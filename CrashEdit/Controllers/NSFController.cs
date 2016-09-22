using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class NSFController : Controller
    {
        private NSF nsf;
        private GameVersion gameversion;
        public short chunkid;

        public NSFController(NSF nsf, GameVersion gameversion)
        {
            this.nsf = nsf;
            this.gameversion = gameversion;
            chunkid = -1;
            foreach (Chunk chunk in nsf.Chunks)
            {
                chunkid += 2;
                if (chunk is NormalChunk)
                {
                    AddNode(new NormalChunkController(this, (NormalChunk)chunk));
                }
                else if (chunk is TextureChunk)
                {
                    AddNode(new TextureChunkController(this, (TextureChunk)chunk));
                }
                else if (chunk is OldSoundChunk)
                {
                    AddNode(new OldSoundChunkController(this, (OldSoundChunk)chunk));
                }
                else if (chunk is SoundChunk)
                {
                    AddNode(new SoundChunkController(this, (SoundChunk)chunk));
                }
                else if (chunk is WavebankChunk)
                {
                    AddNode(new WavebankChunkController(this, (WavebankChunk)chunk));
                }
                else if (chunk is SpeechChunk)
                {
                    AddNode(new SpeechChunkController(this, (SpeechChunk)chunk));
                }
                else if (chunk is UnprocessedChunk)
                {
                    AddNode(new UnprocessedChunkController(this, (UnprocessedChunk)chunk));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            AddMenu("Add Chunk - Normal", Menu_Add_NormalChunk);
            AddMenu("Add Chunk - Sound", Menu_Add_SoundChunk);
            AddMenu("Add Chunk - Wavebank", Menu_Add_WavebankChunk);
            AddMenu("Add Chunk - Speech", Menu_Add_SpeechChunk);
            AddMenuSeparator();
            AddMenu("Fix Nitro Detonators", Menu_Fix_Detonator);
            AddMenu("Fix Box Count", Menu_Fix_BoxCount);
            AddMenuSeparator();
            AddMenu("Show all level geometry", Menu_LoadFullScenery);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "NSF File";
            Node.ImageKey = "nsf";
            Node.SelectedImageKey = "nsf";
        }

        public NSF NSF
        {
            get { return nsf; }
        }

        public GameVersion GameVersion
        {
            get { return gameversion; }
        }

        private void Menu_Add_NormalChunk()
        {
            NormalChunk chunk = new NormalChunk();
            nsf.Chunks.Add(chunk);
            NormalChunkController controller = new NormalChunkController(this, chunk);
            AddNode(controller);
            chunkid += 2;
        }

        private void Menu_Add_SoundChunk()
        {
            SoundChunk chunk = new SoundChunk();
            nsf.Chunks.Add(chunk);
            SoundChunkController controller = new SoundChunkController(this, chunk);
            AddNode(controller);
            chunkid += 2;
        }

        private void Menu_Add_WavebankChunk()
        {
            WavebankChunk chunk = new WavebankChunk();
            nsf.Chunks.Add(chunk);
            WavebankChunkController controller = new WavebankChunkController(this, chunk);
            AddNode(controller);
            chunkid += 2;
        }

        private void Menu_Add_SpeechChunk()
        {
            SpeechChunk chunk = new SpeechChunk();
            nsf.Chunks.Add(chunk);
            SpeechChunkController controller = new SpeechChunkController(this, chunk);
            AddNode(controller);
            chunkid += 2;
        }

        private void Menu_Fix_Detonator()
        {
            List<Entity> nitros = new List<Entity>();
            List<Entity> detonators = new List<Entity>();
            foreach (Chunk chunk in nsf.Chunks)
            {
                if (chunk is EntryChunk)
                {
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is NewZoneEntry)
                        {
                            foreach (Entity entity in ((NewZoneEntry)entry).Entities)
                            {
                                if (entity.Type == 34)
                                {
                                    if (entity.Subtype == 18 && entity.ID.HasValue)
                                    {
                                        nitros.Add(entity);
                                    }
                                    else if (entity.Subtype == 24)
                                    {
                                        detonators.Add(entity);
                                    }
                                }
                            }
                        }
                        if (entry is ZoneEntry)
                        {
                            foreach (Entity entity in ((ZoneEntry)entry).Entities)
                            {
                                if (entity.Type == 34)
                                {
                                    if (entity.Subtype == 18 && entity.ID.HasValue)
                                    {
                                        nitros.Add(entity);
                                    }
                                    else if (entity.Subtype == 24)
                                    {
                                        detonators.Add(entity);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Entity detonator in detonators)
            {
                detonator.Victims.Clear();
                foreach (Entity nitro in nitros)
                {
                    detonator.Victims.Add(new EntityVictim((short)nitro.ID.Value));
                }
            }
        }

        private void Menu_Fix_BoxCount()
        {
            int boxcount = 0;
            List<Entity> willys = new List<Entity>();
            foreach (Chunk chunk in nsf.Chunks)
            {
                if (chunk is EntryChunk)
                {
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is NewZoneEntry)
                        {
                            foreach (Entity entity in ((NewZoneEntry)entry).Entities)
                            {
                                if (entity.Type == 0 && entity.Subtype == 0)
                                {
                                    willys.Add(entity);
                                }
                                else if (entity.Type == 34)
                                {
                                    switch (entity.Subtype)
                                    {
                                        case 5:
                                        case 7:
                                        case 15:
                                        case 24:
                                            break;
                                        default:
                                            boxcount++;
                                            break;
                                    }
                                }
                            }
                        }
                        if (entry is ZoneEntry)
                        {
                            foreach (Entity entity in ((ZoneEntry)entry).Entities)
                            {
                                if (entity.Type == 0 && entity.Subtype == 0)
                                {
                                    willys.Add(entity);
                                }
                                else if (entity.Type == 34)
                                {
                                    switch (entity.Subtype)
                                    {
                                        case 5:
                                        case 7:
                                        case 15:
                                        case 24:
                                        case 28:
                                            break;
                                        default:
                                            boxcount++;
                                            break;
                                    }
                                }
                                else if (entity.Type == 36)
                                {
                                    if (entity.Subtype == 1)
                                    {
                                        boxcount++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (Entity willy in willys)
            {
                if (willy.BoxCount.HasValue)
                {
                    willy.BoxCount = new EntitySetting(0, boxcount);
                }
            }
        }

        private void Menu_LoadFullScenery()
        {
            if (gameversion == GameVersion.Crash1Beta1995 || gameversion == GameVersion.Crash1BetaMAR08 || gameversion == GameVersion.Crash1BetaMAY11)
            {
                List<ProtoSceneryEntry> entries = new List<ProtoSceneryEntry>();

                foreach (Chunk chunk in nsf.Chunks)
                {
                    if (chunk is EntryChunk)
                    {
                        EntryChunk entryChunk = chunk as EntryChunk;

                        foreach (Entry entry in entryChunk.Entries)
                        {
                            if (entry is ProtoSceneryEntry)
                            {
                                entries.Add(entry as ProtoSceneryEntry);
                            }
                        }
                    }
                }

                Form form = new Form { Width = 800, Height = 600 };
                ProtoSceneryEntryViewer viewer = new ProtoSceneryEntryViewer(entries) { Dock = DockStyle.Fill };
                form.Controls.Add(viewer);
                form.Show();
            }
            else if (gameversion == GameVersion.Crash1)
            {
                List<OldSceneryEntry> entries = new List<OldSceneryEntry>();

                foreach (Chunk chunk in nsf.Chunks)
                {
                    if (chunk is EntryChunk)
                    {
                        EntryChunk entryChunk = chunk as EntryChunk;

                        foreach (Entry entry in entryChunk.Entries)
                        {
                            if (entry is OldSceneryEntry)
                            {
                                entries.Add(entry as OldSceneryEntry);
                            }
                        }
                    }
                }

                Form form = new Form { Width = 800, Height = 600 };
                OldSceneryEntryViewer viewer = new OldSceneryEntryViewer(entries) { Dock = DockStyle.Fill };
                form.Controls.Add(viewer);
                form.Show();
            }
            else if(gameversion == GameVersion.Crash2)
            {
                List<SceneryEntry> entries = new List<SceneryEntry>();

                foreach (Chunk chunk in nsf.Chunks)
                {
                    if (chunk is EntryChunk)
                    {
                        EntryChunk entryChunk = chunk as EntryChunk;

                        foreach (Entry entry in entryChunk.Entries)
                        {
                            if (entry is SceneryEntry)
                            {
                                entries.Add(entry as SceneryEntry);
                            }
                        }
                    }
                }

                Form form = new Form { Width = 800, Height = 600 };
                SceneryEntryViewer viewer = new SceneryEntryViewer(entries) { Dock = DockStyle.Fill };
                form.Controls.Add(viewer);
                form.Show();
            }
            else if (gameversion == GameVersion.Crash3)
            {
                List<NewSceneryEntry> entries = new List<NewSceneryEntry>();

                foreach (Chunk chunk in nsf.Chunks)
                {
                    if (chunk is EntryChunk)
                    {
                        EntryChunk entryChunk = chunk as EntryChunk;

                        foreach (Entry entry in entryChunk.Entries)
                        {
                            if (entry is NewSceneryEntry)
                            {
                                entries.Add(entry as NewSceneryEntry);
                            }
                        }
                    }
                }

                Form form = new Form { Width = 800, Height = 600 };
                NewSceneryEntryViewer viewer = new NewSceneryEntryViewer(entries) { Dock = DockStyle.Fill };
                form.Controls.Add(viewer);
                form.Show();
            }
            else
            {
                MessageBox.Show("The specified game version is not supported", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
