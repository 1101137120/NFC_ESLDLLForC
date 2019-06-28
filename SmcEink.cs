using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EslLibCom
{
    class SmcEink
    {
        public SmcEink() { }

        //**************

        public Bitmap ConvertImageToImage(Bitmap mbmp, Image img, int x, int y, int width, int Heigh)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                graphic.DrawImage(img, x, y, width, Heigh);
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }

        public Bitmap ConvertTextToImage(Bitmap mbmp, TextBox textbox, Color textcolor, int x, int y)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                FontFamily fontFamily = new FontFamily("細明體");
                Font font = new Font(fontFamily, textbox.Font.Size, textbox.Font.Style, GraphicsUnit.Point);
                StringFormat stringFormat = new StringFormat();
                if (textbox.TextAlign == HorizontalAlignment.Center)
                {
                    stringFormat.Alignment = StringAlignment.Center;
                    graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), (float)(x + textbox.Width / 2), (float)y, stringFormat);
                }
                else if (textbox.TextAlign != HorizontalAlignment.Right)
                {
                    graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), (float)x, (float)y);
                }
                else
                {
                    stringFormat.Alignment = StringAlignment.Far;
                    graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), (float)(x + textbox.Width), (float)y, stringFormat);
                }
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }

        public Bitmap ConvertTextToImage(Bitmap mbmp, Label label, Color textcolor, int x, int y)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                FontFamily fontFamily = new FontFamily("細明體");
                Font font = new Font(fontFamily, label.Font.Size, label.Font.Style, GraphicsUnit.Point);
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                graphic.DrawString(label.Text, font, new SolidBrush(label.ForeColor), (float)x, (float)y);
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }

        public Bitmap ConvertTextToImage(Bitmap mbmp, string txt, Font font1, Color textcolor, int x, int y)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                graphic.DrawString(txt, font1, new SolidBrush(textcolor), (float)x, (float)y);
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }

        public Bitmap ConvertTextToImage(Bitmap mbmp, string txt, Font font1, Color textcolor, int x, int y, StringFormat drawFormat)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                graphic.DrawString(txt, font1, new SolidBrush(textcolor), (float)x, (float)y, drawFormat);
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }


        //**************

        public Bitmap ConvertPanelToImage(Bitmap mbmp, Panel textbox, Color textcolor, int x, int y)
        {


          /*  using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //  graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                FontFamily fontFamily = new FontFamily("Calibri");
                Font font = new Font(fontFamily, textbox.Font.Size, textbox.Font.Style, GraphicsUnit.Point);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Far;
                int mar = 3;
                Point[] aaa = new Point[5];
                aaa[0] = new Point(x, y); //left
                aaa[1] = new Point(x + textbox.Width, y); //right
                aaa[2] = new Point(x + textbox.Width, y + textbox.Height); //right b
                aaa[3] = new Point(x, y + textbox.Height); //left b
                aaa[4] = new Point(x, y); //left


                graphic.DrawLines(Pens.Black, aaa);
                graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), (float)x, (float)y, stringFormat);
                graphic.Flush();
                graphic.Dispose();
            }*/

            foreach (Control controlIN in textbox.Controls)
            {
               // Console.WriteLine("COUNTPANEL" + controlIN.Name);
                if (controlIN is TextBox)
                {
                    int x2 = ((TextBox)controlIN).Location.X;
                    int y2 = ((TextBox)controlIN).Location.Y;
                    int width = ((TextBox)controlIN).Width;
                    int height = ((TextBox)controlIN).Height;
                //    Console.WriteLine(string.Concat(new object[] { x, ",", y, "  w:", width, ", h:", height }));
                    mbmp = ConvertPanelTextToImage(mbmp, (TextBox)controlIN, Color.Black, x2, y2, x, y);

                }
                else if (controlIN is Label)
                {
                    int x1 = ((Label)controlIN).Location.X;
                    int y1 = ((Label)controlIN).Location.Y;
                    int width2 = ((Label)controlIN).Width;
                    int height2 = ((Label)controlIN).Height;
                  //  Console.WriteLine(string.Concat(new object[] { "Label ", x1, ",", y1, "  w:", width2, ", h:", height2 }));
                    mbmp = ConvertPanelLabelToImage(mbmp, (Label)controlIN, Color.Black, x1, y1, x, y);
                }
            }
            return mbmp;
        }


        public Bitmap ConvertPanelLabelToImage(Bitmap mbmp, Label textbox, Color textcolor, int x, int y, int xPanel, int yPanel)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                FontFamily fontFamily = new FontFamily("Calibri");
                Font font = new Font(fontFamily, textbox.Font.Size, textbox.Font.Style, GraphicsUnit.Point);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Far;
                if (textbox.Name == "panel6labelDemo")
                {
                    /* Console.WriteLine("DDDDDDDD" + textbox.Image);
                     string str = System.AppDomain.CurrentDomain.BaseDirectory;
                     Console.WriteLine("str" + str);
                     string filename = str + "circle2.png";
                     Bitmap image1 = (Bitmap)Image.FromFile(@filename, true);
                     graphic.DrawImage(image1, new Point(xPanel + (int)(x), yPanel + (int)y));*/
                    /// 设置绘图的颜色
                    Brush greenBrush = new SolidBrush(Color.Red);//把这个变成与你背景一样的颜色
                    int radius = 17;
                    // 绘制圆，(0, 0)为左上角的坐标，radius为直径
                    graphic.FillEllipse(greenBrush, xPanel + (float)(x + textbox.Width / 2 - 4), yPanel + (float)y + 1, radius, radius);
                    graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), xPanel + (float)(x + textbox.Width / 2 + 13), yPanel + (float)y, stringFormat);
                }
                else
                {

                    if (textbox.Name == "Label62Demo" || textbox.Name == "Label63Demo" || textbox.Name == "Label64Demo")
                    {
                        if (textbox.Text != "") {
                            float floatDpi = float.Parse(textbox.Tag.ToString(), CultureInfo.InvariantCulture.NumberFormat);
                            Label Slash = new Label();
                            Slash.BorderStyle = BorderStyle.None;
                            Slash.Font = new Font("Calibri", 9.2f * floatDpi, FontStyle.Regular, GraphicsUnit.Point, 0);
                            Slash.Name = "Slash";
                            Slash.ForeColor = Color.Black;
                            //  Label19Demo.Size = new Size(58, 15);
                            Slash.TabIndex = 229;
                            Slash.Text += "/";
                            Slash.AutoSize = true;
                            graphic.DrawString(Slash.Text, font, new SolidBrush(Slash.ForeColor), xPanel + (float)(x)+ (textbox.Width/3.8F), yPanel + (float)y, stringFormat);
                        }

                    }
                        graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), xPanel + (float)(x + textbox.Width), yPanel + (float)y, stringFormat);
                }

                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }


        public Bitmap ConvertBarToImage(Bitmap mbmp, Bitmap img, int x, int y)
        {
        //    Console.WriteLine("ConvertBarToImageW" + img.Width);
        //    Console.WriteLine("ConvertBarToImageH" + img.Height);
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if ((pixel.R + pixel.B + pixel.G) / 3 < 180)
                    {
                        mbmp.SetPixel(i + x, j + y, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return mbmp;
        }


        public Bitmap ConvertPanelTextToImage(Bitmap mbmp, TextBox textbox, Color textcolor, int x, int y, int xPanel, int yPanel)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                FontFamily fontFamily = new FontFamily("Calibri");
                Font font = new Font(fontFamily, textbox.Font.Size, textbox.Font.Style, GraphicsUnit.Point);
                StringFormat stringFormat = new StringFormat();
                if (textbox.TextAlign == HorizontalAlignment.Center)
                {
                    stringFormat.Alignment = StringAlignment.Center;
                    graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), xPanel + (float)(x + textbox.Width / 2) + 2, yPanel + (float)y, stringFormat);
                }
                else if (textbox.TextAlign != HorizontalAlignment.Right)
                {
                    graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), xPanel + (float)x, yPanel + (float)y);
                }
                else
                {
                    stringFormat.Alignment = StringAlignment.Far;
                    graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), xPanel + (float)(x + textbox.Width), yPanel + (float)y, stringFormat);
                }
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }




        public Bitmap ConvertTextToImageDemo(Bitmap mbmp, Label label, Color textcolor, int x, int y)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                FontFamily fontFamily = new FontFamily("Calibri");
                Font font = new Font(fontFamily, label.Font.Size, label.Font.Style, GraphicsUnit.Point);
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                graphic.DrawString(label.Text, font, new SolidBrush(label.ForeColor), (float)x, (float)y);
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }



        public Bitmap ConvertTextToImageDemo(Bitmap mbmp, TextBox textbox, Color textcolor, int x, int y)
        {
            using (Graphics graphic = Graphics.FromImage(mbmp))
            {
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                FontFamily fontFamily = new FontFamily("Calibri");
                Font font = new Font(fontFamily, textbox.Font.Size, textbox.Font.Style, GraphicsUnit.Point);
                StringFormat stringFormat = new StringFormat();
                if (textbox.BorderStyle == BorderStyle.FixedSingle)
                {
                  /*  Point[] aaa = new Point[5];
                    aaa[0] = new Point(x, y); //left
                    aaa[1] = new Point(x + textbox.Width, y); //right
                    aaa[2] = new Point(x + textbox.Width, y + textbox.Height); //right b
                    aaa[3] = new Point(x, y + textbox.Height); //left b
                    aaa[4] = new Point(x, y); //left

                    Pen pen = new Pen(Color.Black, 0.2F);
                    pen.Width = 0.2F;
                    graphic.DrawLines(pen, aaa);*/
                }
                if (textbox.TextAlign == HorizontalAlignment.Center)
                {
                    stringFormat.Alignment = StringAlignment.Center;
                    graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), (float)(x + textbox.Width / 2), (float)(y + 2), stringFormat);
                }
                else if (textbox.TextAlign != HorizontalAlignment.Right)
                {
                    graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), (float)x, (float)y);
                }
                else
                {
                    stringFormat.Alignment = StringAlignment.Far;
                    graphic.DrawString(textbox.Text, font, new SolidBrush(textbox.ForeColor), (float)(x + textbox.Width), (float)y, stringFormat);
                }
                graphic.Flush();
                graphic.Dispose();
            }
            return mbmp;
        }



        public Bitmap ConvertBoxToImage(Bitmap mbmp, Label label, Color textcolor, int x, int y)
        {
            using (Graphics graphics = Graphics.FromImage(mbmp))
            {

                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

                SolidBrush blackPen = new SolidBrush(label.BackColor);
                Rectangle rect = new Rectangle(x, y, 65, 19);
                graphics.FillRectangle(blackPen, rect);


                graphics.DrawString(label.Text, label.Font, new SolidBrush(textcolor), x, y);
                graphics.Flush();
                graphics.Dispose();
            }
            return mbmp;
        }



        //**************
        public Bitmap ConvertImageToImage(Bitmap mbmp, Bitmap img, int x, int y)
        {

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color color = img.GetPixel(i, j);
                    int ttt = (color.R + color.B + color.G) / 3;
                    if (ttt < 180)
                    {
                        mbmp.SetPixel(i + x, j + y, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return mbmp;
        }
    }
}
