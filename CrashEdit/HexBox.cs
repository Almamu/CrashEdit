using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class HexBox : Control
    {
        private int offset;
        private int position;
        private int? input;
        private byte[] data;

        public HexBox()
        {
            offset = 0;
            position = 0;
            input = null;
            data = new byte [0];
            TabStop = true;
            SetStyle(ControlStyles.Selectable,true);
            DoubleBuffered = true;
        }

        public byte[] Data
        {
            //get { return data; }
            set { data = value; }
        }

        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                if (position >= data.Length)
                {
                    position = data.Length - 1;
                }
                if (position < 0)
                {
                    position = 0;
                }
                while (offset < position / 16 - 15)
                {
                    offset++;
                }
                while (offset > position / 16)
                {
                    offset--;
                }
                Invalidate();
            }
        }

        public void MoveUp()
        {
            Position -= 16;
        }

        public void MoveUpPage()
        {
            Position -= 256;
        }

        public void MoveDown()
        {
            Position += 16;
        }

        public void MoveDownPage()
        {
            Position += 256;
        }

        public void MoveLeft()
        {
            Position--;
        }

        public void MoveRight()
        {
            Position++;
        }

        public void MoveHome()
        {
            Position = 0;
            offset = 0;
        }

        public void MoveHomeLine()
        {
            Position -= Position % 16;
        }

        public void MoveEnd()
        {
            Position = int.MaxValue;
            offset = Position / 16 - 15;
            if (offset < 0)
            {
                offset = 0;
            }
        }

        public void MoveEndLine()
        {
            Position = Position - Position % 16 + 15;
        }

        public void InputNibble(int value)
        {
            if (input == null)
            {
                input = value;
            }
            else
            {
                data[position] = (byte)((input << 4) | value);
                input = null;
            }
            Invalidate();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Home:
                case Keys.End:
                case Keys.Space:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    InputNibble(e.KeyCode - Keys.D0);
                    break;
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                    InputNibble(e.KeyCode - Keys.NumPad0);
                    break;
                case Keys.A:
                    InputNibble(0xA);
                    break;
                case Keys.B:
                    InputNibble(0xB);
                    break;
                case Keys.C:
                    InputNibble(0xC);
                    break;
                case Keys.D:
                    InputNibble(0xD);
                    break;
                case Keys.E:
                    InputNibble(0xE);
                    break;
                case Keys.F:
                    InputNibble(0xF);
                    break;
                case Keys.Up:
                    MoveUp();
                    break;
                case Keys.Down:
                    MoveDown();
                    break;
                case Keys.Left:
                    MoveLeft();
                    break;
                case Keys.Right:
                    MoveRight();
                    break;
                case Keys.PageUp:
                    MoveUpPage();
                    break;
                case Keys.PageDown:
                    MoveDownPage();
                    break;
                case Keys.Home:
                    if (e.Control)
                    {
                        MoveHome();
                    }
                    else
                    {
                        MoveHomeLine();
                    }
                    break;
                case Keys.End:
                    if (e.Control)
                    {
                        MoveEnd();
                    }
                    else
                    {
                        MoveEndLine();
                    }
                    break;
                case Keys.Space:
                    data[position] = 0;
                    Invalidate();
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Pen borderpen = Pens.Black;
            Brush brush = Brushes.Navy;
            Brush backbrush = Brushes.White;
            Brush hibackbrush = Brushes.LightGreen;
            Brush selbrush = Brushes.White;
            Brush selbackbrush = Brushes.Navy;
            Brush deadselbackbrush = Brushes.DarkGray;
            Brush inputselbackbrush = Brushes.Red;
            Brush voidbrush = Brushes.DarkMagenta;
            Font font = new Font(FontFamily.GenericMonospace,8);
            Font selfont = new Font(FontFamily.GenericMonospace,10);
            StringFormat format = new StringFormat();
            StringFormat selformat = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            selformat.Alignment = StringAlignment.Center;
            selformat.LineAlignment = StringAlignment.Center;
            int hstep = (Width - 1) / 16;
            int vstep = (Height - 1) / 16;
            int width = hstep * 16;
            int height = vstep * 16;
            int xsel = position % 16;
            int ysel = position / 16;
            for (int y = 0;y < 16;y++)
            {
                for (int x = 0;x < 16;x++)
                {
                    Font curfont;
                    Brush curbrush;
                    Brush curbackbrush;
                    StringFormat curformat;
                    Rectangle rect = new Rectangle();
                    rect.X = hstep * x + 1;
                    rect.Y = vstep * y + 1;
                    rect.Width = hstep - 1;
                    rect.Height = vstep - 1;
                    string text;
                    if (x == xsel && y + offset == ysel)
                    {
                        curfont = selfont;
                        curbrush = selbrush;
                        curformat = selformat;
                        if (input == null)
                        {
                            curbackbrush = Focused ? selbackbrush : deadselbackbrush;
                            text = data[x + (offset + y) * 16].ToString("X2");
                        }
                        else
                        {
                            curbackbrush = Focused ? inputselbackbrush : deadselbackbrush;
                            text = ((int)input).ToString("X");
                        }
                    }
                    else if (x + (offset + y) * 16 < data.Length)
                    {
                        curfont = font;
                        curbrush = brush;
                        curbackbrush = (data[x + (offset + y) * 16] != 0) ? hibackbrush : backbrush;
                        curformat = format;
                        text = data[x + (offset + y) * 16].ToString("X2");
                    }
                    else
                    {
                        curfont = null;
                        curbrush = null;
                        curbackbrush = voidbrush;
                        curformat = null;
                        text = "";
                    }
                    e.Graphics.FillRectangle(curbackbrush,rect);
                    if (x + (offset + y) * 16 < data.Length)
                    {
                        e.Graphics.DrawString(text,curfont,curbrush,rect,curformat);
                    }
                }
            }
            for (int i = 0; i < 17; i++)
            {
                e.Graphics.DrawLine(borderpen,hstep * i,0,hstep * i,height);
                e.Graphics.DrawLine(borderpen,0,vstep * i,width,vstep * i);
            }
        }
    }
}
