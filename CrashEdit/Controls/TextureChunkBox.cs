using Crash;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class TextureChunkBox : UserControl
    {
        private TabControl tbcTabs;

        public TextureChunkBox(TextureChunk chunk)
        {
            tbcTabs = new TabControl();
            tbcTabs.Dock = DockStyle.Fill;
            {
                MysteryBox mystery = new MysteryBox(chunk.Data);
                mystery.Dock = DockStyle.Fill;
                TabPage page = new TabPage("Hex");
                page.Controls.Add(mystery);
                tbcTabs.TabPages.Add(page);
            }
            {
                Bitmap bitmap = new Bitmap(512,128,PixelFormat.Format16bppArgb1555);
                Rectangle brect = new Rectangle(Point.Empty,bitmap.Size);
                BitmapData bdata = bitmap.LockBits(brect,ImageLockMode.WriteOnly,PixelFormat.Format16bppArgb1555);
                try
                {
                    for (int y = 0;y < 128;y++)
                    {
                        for (int x = 0;x < 512;x++)
                        {
                            byte color = chunk.Data[x + y * 512];
                            color >>= 3;
                            short color16 = PixelConv.Pack1555(1,color,color,color);
                            System.Runtime.InteropServices.Marshal.WriteInt16(bdata.Scan0,x * 2 + y * bdata.Stride,color16);
                        }
                    }
                }
                finally
                {
                    bitmap.UnlockBits(bdata);
                }
                PictureBox picture = new PictureBox();
                picture.Dock = DockStyle.Fill;
                picture.Image = bitmap;
                TabPage page = new TabPage("Monochrome 8");
                page.Controls.Add(picture);
                tbcTabs.TabPages.Add(page);
                tbcTabs.SelectedTab = page;
            }
            {
                Bitmap bitmap = new Bitmap(256,128,PixelFormat.Format16bppArgb1555);
                Rectangle brect = new Rectangle(Point.Empty,bitmap.Size);
                BitmapData bdata = bitmap.LockBits(brect,ImageLockMode.WriteOnly,PixelFormat.Format16bppArgb1555);
                try
                {
                    for (int y = 0;y < 128;y++)
                    {
                        for (int x = 0;x < 256;x++)
                        {
                            short color = BitConv.FromInt16(chunk.Data,x * 2 + y * 512);
                            byte alpha;
                            byte red;
                            byte green;
                            byte blue;
                            PixelConv.Unpack1555(color,out alpha,out blue,out green,out red);
                            color = PixelConv.Pack1555(1,red,green,blue);
                            System.Runtime.InteropServices.Marshal.WriteInt16(bdata.Scan0,x * 2 + y * bdata.Stride,color);
                        }
                    }
                }
                finally
                {
                    bitmap.UnlockBits(bdata);
                }
                PictureBox picture = new PictureBox();
                picture.Dock = DockStyle.Fill;
                picture.Image = bitmap;
                TabPage page = new TabPage("BGR555");
                page.Controls.Add(picture);
                tbcTabs.TabPages.Add(page);
            }
            for (int paleteNum = 0; paleteNum < 16; paleteNum++)
            {
                {
                    Bitmap result = new Bitmap(512, 112, PixelFormat.Format8bppIndexed);
                    ColorPalette palette = result.Palette;
                    Color[] colors = palette.Entries;

                    // palettes are 256x1 so we create a test palete
                    int paleteWidth = 256;
                    int paleteHeight = 16;

                    for (int x = 0; x < paleteWidth; x++)
                    {
                        short color16 = BitConv.FromInt16(chunk.Data, (x + paleteNum * paleteWidth) * 2);
                        byte alpha, red, green, blue;

                        PixelConv.Unpack1555(color16, out alpha, out blue, out green, out red);
                        Color cl = Color.FromArgb(((alpha == 0) ? 0 : 255), red << 3, green << 3, blue << 3);
                        colors[x] = cl;
                    }

                    result.Palette = palette;

                    // Bitmap bitmap = new Bitmap(512,112,PixelFormat.Format16bppArgb1555);
                    Rectangle brect = new Rectangle(Point.Empty, result.Size);
                    BitmapData bdata = result.LockBits(brect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

                    try
                    {
                        for (int y = paleteHeight; y < 128; y++)
                        {
                            for (int x = 0; x < 512; x++)
                            {
                                byte color = chunk.Data[x + y * 512];
                                System.Runtime.InteropServices.Marshal.WriteByte(bdata.Scan0, x + (y - paleteHeight) * bdata.Stride, color);
                            }
                        }
                    }
                    finally
                    {
                        result.UnlockBits(bdata);
                    }
                    PictureBox picture = new PictureBox();
                    picture.Dock = DockStyle.Fill;
                    picture.Image = result;
                    TabPage page = new TabPage("Palette " + paleteNum);
                    page.Controls.Add(picture);
                    tbcTabs.TabPages.Add(page);
                    tbcTabs.SelectedTab = page;
                }
            }
            Controls.Add(tbcTabs);
        }
    }
}
