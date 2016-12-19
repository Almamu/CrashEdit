using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldModelInfo
    {
        const int TEXTURE_DATA_OFFSET = 0x14;

        public OldModelInfo(byte[] data, List<OldModelPolygon> polygons)
        {
            this.PolyCount = BitConv.FromInt32(data, 0);
            this.ScaleX = BitConv.FromInt32(data, 0x4);
            this.ScaleY = BitConv.FromInt32(data, 0x8);
            this.ScaleZ = BitConv.FromInt32(data, 0xC);

            for (int i = 0; i < this.PolyCount; i++)
            {
                short textInfoOffset = (short) (polygons[i].FlagTex & 0x7FFF);
                int texInfoA = BitConv.FromInt32(data[TEXTURE_DATA_OFFSET + (textInfoOffset * 4)]);
                bool textured = ((texInfoA >> 31) & 1) == 1;
            }
        }

        public int PolyCount { get; private set; }
        public int ScaleX { get; private set; }
        public int ScaleY { get; private set; }
        public int ScaleZ { get; private set; }
    }

    public sealed class OldModelEntry : Entry
    {
        private byte[] info;
        private List<OldModelPolygon> polygons;
        private OldModelInfo processedInformation;

        public OldModelEntry(byte[] info,IEnumerable<OldModelPolygon> polygons,int eid, int size) : base(eid, size)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            if (polygons == null)
                throw new ArgumentNullException("polygons");
            this.info = info;
            this.polygons = new List<OldModelPolygon>(polygons);
            this.processedInformation = new OldModelInfo(info, this.polygons);
        }

        public override int Type
        {
            get { return 2; }
        }

        public byte[] Info
        {
            get { return info; }
        }

        public OldModelInfo ProcessedInformation
        {
            get { return processedInformation; }
        }

        public IList<OldModelPolygon> Polygons
        {
            get { return polygons; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [2][];
            items[0] = info;
            items[1] = new byte [polygons.Count * 8];
            for (int i = 0;i < polygons.Count;i++)
            {
                polygons[i].Save().CopyTo(items[1],i * 8);
            }
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
