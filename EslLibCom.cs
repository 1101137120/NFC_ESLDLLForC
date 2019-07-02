using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;   //作為 COM 元件的專案必需引用
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;
using ZXing;

namespace EslLibCom
{

    [Guid("D642F8C5-6B4B-435E-B9DD-A0E04B00D745")]
    public interface EslLibCOMInterface
    {
        [DispId(1)]
        bool portOpen(int portName, int baurate);

        [DispId(2)]
        bool portIsOpen();

        [DispId(3)]
        Object EslSeting();

        [DispId(4)]
        Object EslSeting2Color();

        [DispId(8)]
        bool sendWriteEslClose();

        [DispId(9)]
        Object eslUUIDRead();






    }

    [Guid("93575AF9-8E3E-4430-A191-8868C6814B1F")]
    [ClassInterface(ClassInterfaceType.None)] //表示沒有為該class產生COM介面
    [ProgId("EslLibCom.EslLibCOMFunc")]
    public class EslLibCOMFunc : EslLibCOMInterface
    {
        private NfcEslLib EslLib = new NfcEslLib();
        Bitmap bmp;
        Socket bcSocket;
        private List<Socket> clientSockets = new List<Socket>();
        Object ddt = null;
        //开始监听
        
        public bool portOpen(int portName, int baurate)
        {
            string portNameS = "COM" + portName;
            string baurateS = baurate.ToString();
            bool dd = EslLib.portOpen(portNameS, baurateS);
            // Console.WriteLine("dd  "+dd);
            if(dd)
                EslLib.onSMCEslReceiveEvent += new EventHandler(OnSMCEslReceiveEvent);

            return dd;
        }

        public bool portIsOpen()
        {
            return EslLib.portIsOpen();
        }



        public Object EslSeting()
        {
            if (img())
            {
                if (eslImagePix())
                {
                    ddt = null;
                    EslLib.EslSeting();
                    while (ddt == null)
                    {
                        Console.Write(ddt);
                    };
                    while (ddt != null)
                    {


                        if (ddt.ToString().Contains("status:False"))
                            break;
                        if (ddt.ToString().Contains("msgId:4"))
                            break;
                        if (ddt.ToString().Contains("msgId:5"))
                        {
                            ddt = "msgId:2;status:False";
                            break;
                        }
                    };
                }
                else
                {
                    ddt = "msgId:6,stasus:False";
                }
            }
            else
            {
                ddt = "msgId:5,stasus:False";
            }
            return ddt;
        }

        public Object EslSeting2Color()
        {
            if (img())
            {
                if (eslImagePix2Color())
                {
                    ddt = null;
                    EslLib.EslSeting2Color();
                    while (ddt == null)
                    {
                        Console.Write(ddt);
                    };
                    while (ddt != null)
                    {
                       // Console.WriteLine("ddt "+ddt);

                        if (ddt.ToString().Contains("status:False"))
                            break;
                        if (ddt.ToString().Contains("msgId:4"))
                            break;
                        if (ddt.ToString().Contains("msgId:5"))
                        {
                            ddt = "msgId:2;status:False";
                            break;
                        }
                           
                    };
                }
                else
                {
                    ddt = "msgId:6,stasus:False";
                }
            }
            else
            {
                ddt = "msgId:5,stasus:False";
            }
            return ddt;
        }

        private bool eslImagePix()
        {
            return EslLib.eslImagePix(bmp);
        }

        private bool eslImagePix2Color()
        {
            return EslLib.eslImagePix2Color(bmp);
        }

        private bool eslImageLockBit(Bitmap bmp)
        {
            return EslLib.eslImageLockBit(bmp);
        }

        public Object eslUUIDRead()
        {
         //   Console.WriteLine("eslUUIDRead INININ" );
            ddt = null;
            EslLib.eslUUIDRead();
            
            do
            {
                Console.Write(ddt);
            } while (ddt==null);

          // Console.WriteLine("eslUUIDRead  DLL "+ddt);
            return ddt;
        }

        public bool sendWriteEslClose()
        {
            return EslLib.sendWriteEslClose();
        }

        private void SMCEslReceiveEventNow()
        {

            try {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces(); //get all network interfaces of the computer
                bcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //broadcast socket
                IPEndPoint myLocalEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 48899);
                bcSocket.Bind(myLocalEndPoint);
                bcSocket.Listen(10);
                bcSocket.BeginAccept(new AsyncCallback(acceptCallback), null);

                //开始监听
                EslLib.onSMCEslReceiveEvent += new EventHandler(OnSMCEslReceiveEvent);
            } catch (Exception ex) {
                Console.WriteLine("EX    "+ex);
            }



            //OnSMCEslReceiveEvent(new SMCEslReceiveEventArgs());
        }



        private  void acceptCallback(IAsyncResult result)
        { //if the buffer is old, then there might already be something there...
            Socket socket = null;
            try
            {
                socket = bcSocket.EndAccept(result); // The ObjectDisposedException will come here... thus, it is to be expected!
                                                         //Do something as you see it needs on client acceptance such as listing
                clientSockets.Add(socket); //may be needed later
               // socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(receiveCallback), socket);
               // serverSocket.BeginAccept(new AsyncCallback(acceptCallback), null); //to receive another client
            }
            catch (Exception e)
            { // this exception will happen when "this" is be disposed...        
              //Do something here
                Console.WriteLine(e.ToString());
            }
        }

        public class txtData
        {
            public string tag;
            public Color tagColor = Color.Black;
        }

        private bool img() {
            try {
                txtData tag1_1 = new txtData();

                txtData tag1_2 = new txtData();
                txtData tag1_3 = new txtData();
                txtData tag2_1 = new txtData();
                txtData tag2_2 = new txtData();
                txtData tag3_1 = new txtData();
                txtData tag3_2 = new txtData();
                txtData tag3_3 = new txtData();
                txtData tag4_1 = new txtData();
                txtData tag4_2 = new txtData();
                txtData tag4_3 = new txtData();
                txtData tag4_4 = new txtData();
                txtData tag4_5 = new txtData();
                txtData tag4_6 = new txtData();
                txtData tag4_7 = new txtData();
                txtData tag4_8 = new txtData();
                txtData tag4_9 = new txtData();
                txtData tag4_10 = new txtData();
                txtData tag4_11 = new txtData();
                txtData tag4_12 = new txtData();
                txtData tag4_13 = new txtData();
                txtData tag4_14 = new txtData();
                txtData tag4_15 = new txtData();
                txtData tag5_1 = new txtData();
                txtData tag5_2 = new txtData();
                txtData tag6_1 = new txtData();
                txtData tag6_2 = new txtData();
                txtData tag6_3 = new txtData();
                txtData tag6_4 = new txtData();

                Panel panel29 = new Panel();
                Panel panel3Demo = new Panel();
                Panel panel4Demo = new Panel();
                Panel panel5Demo = new Panel();
                Panel panel6Demo = new Panel();
                Panel panel7Demo = new Panel();
                Label panel6labelDemo = new Label();
                Label Label7Demo = new Label();
                Label Label6Demo = new Label();
                Label Label62Demo = new Label();
                Label Label63Demo = new Label();
                Label Label64Demo = new Label();
                Label Label5Demo = new Label();
                Label Label4Demo = new Label();
                Label Label3Demo = new Label();
                Label Label18Demo = new Label();
                Label Label19Demo = new Label();
                Label Label17Demo = new Label();
                Label Label2Demo = new Label();
                Label Label1Demo = new Label();
                Label label5Demo = new Label();
                Label label13Demo = new Label();
                PictureBox pictureBoxa = new PictureBox();
                // SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                Label Label8Demo = new Label();
                Label Label16Demo = new Label();
                TextBox scanDirTextBox = new TextBox();


                tag1_1.tag = "EBUF7E8.1";
                tag1_2.tag = "EBUF7E8.1";
                tag1_3.tag = "C1TMJV85156A-M004";
                tag2_1.tag = "Info:";
                tag2_2.tag = "3";
                tag3_1.tag = "PSM";
                tag3_2.tag = "SECURITY";
                tag3_3.tag = "SSSHR";
                tag4_1.tag = "0.028,L,";
                tag4_2.tag = "193,";
                tag4_3.tag = "AJBX3-1,";
                tag4_4.tag = "BK#331";
                tag4_5.tag = "Tone=";
                tag4_6.tag = "C,";
                tag4_7.tag = "CCD=";
                tag4_8.tag = "0.72, ";
                tag4_9.tag = "PD=";
                tag4_10.tag = "23.08%";
                tag4_11.tag = "Pel=";
                tag4_12.tag = "36,";
                tag4_13.tag = "DD-BO, ";
                tag4_14.tag = "CLIP=";
                tag4_15.tag = "10/13 23:00";
                tag5_1.tag = "AAEEMD";
                tag5_2.tag = "(UL_MOSI_40_C)";
                tag6_1.tag = "QA1";
                tag6_2.tag = "ASI";
                tag6_3.tag = "IPRO";
                tag6_4.tag = "MPM";

                panel29.BackColor = System.Drawing.SystemColors.ControlLightLight;
                panel29.Name = "panel29";
                panel29.Size = new System.Drawing.Size(296, 128);
                panel29.TabIndex = 108;

                panel3Demo.Location = new Point(170, 1);
                panel3Demo.Size = new Size(103, 48);
                panel3Demo.TabIndex = 120;
                // panel4Demo.BorderStyle = BorderStyle.FixedSingle;
                panel4Demo.Location = new Point(128, 50);
                panel4Demo.AutoSize = true;
                panel4Demo.Size = new Size(150, 47);
                panel4Demo.TabIndex = 120;
                //  panel5Demo.BorderStyle = BorderStyle.FixedSingle;
                panel5Demo.Location = new Point(2, 92);
                panel5Demo.Size = new Size(98, 30);
                panel5Demo.TabIndex = 120;

                //    panel6Demo.BorderStyle = BorderStyle.FixedSingle;
                panel6Demo.Location = new Point(246, 98);
                panel6Demo.Size = new Size(72, 23);
                panel6Demo.TabIndex = 120;

                panel7Demo.Location = new Point(105, 100);
                panel7Demo.Size = new Size(130, 30);
                panel7Demo.TabIndex = 120;


                panel29.Controls.Add(panel3Demo);
                panel29.Controls.Add(panel4Demo);
                panel29.Controls.Add(panel5Demo);
                panel29.Controls.Add(panel6Demo);
                panel29.Controls.Add(panel7Demo);

                //  panel29.Controls.Add(Label7Demo);

                panel29.Controls.Add(Label5Demo);
                panel29.Controls.Add(Label4Demo);

                // panel1.Controls.Add(Label3Demo);
                // panel1.Controls.Add(Label2);
                //  panel1.Controls.Add(Label8Demo);
                // panel1.Controls.Add(Label16Demo);
                // panel1.Controls.Add(Label17Demo);
                //panel1.Controls.Add(Label1);
                // panel1.Controls.Add(label5Demo);
                //panel1.Controls.Add(label3Demo);
                panel29.Controls.Add(pictureBoxa);

                panel3Demo.Controls.Add(Label2Demo);
                panel3Demo.Controls.Add(Label16Demo);
                panel3Demo.Controls.Add(Label17Demo);
                panel4Demo.Controls.Add(Label1Demo);
                panel4Demo.Controls.Add(Label3Demo);
                panel4Demo.Controls.Add(Label19Demo);
                panel4Demo.Controls.Add(Label18Demo);
                panel4Demo.Controls.Add(Label8Demo);
                panel5Demo.Controls.Add(label5Demo);
                panel5Demo.Controls.Add(Label7Demo);
                panel6Demo.Controls.Add(label13Demo);
                panel6Demo.Controls.Add(panel6labelDemo);
                panel7Demo.Controls.Add(Label6Demo);
                panel7Demo.Controls.Add(Label62Demo);
                panel7Demo.Controls.Add(Label63Demo);
                panel7Demo.Controls.Add(Label64Demo);



                Label7Demo.BorderStyle = BorderStyle.None;
                Label7Demo.Font = new Font("Calibri", 9.5f, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label7Demo.Location = new Point(2, 15);
                Label7Demo.ForeColor = tag5_2.tagColor;
                Label7Demo.Name = "Label7Demo";
                // Label7Demo.Size = new Size(73, 15);
                Label7Demo.TabIndex = 150;
                Label7Demo.Text = tag5_2.tag;
                Label7Demo.AutoSize = true;


                Label6Demo.BorderStyle = BorderStyle.None;
                Label6Demo.Font = new Font("Calibri", 9.2f, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label6Demo.Location = new Point(0, 5);
                Label6Demo.Name = "Label6Demo";
                Label6Demo.ForeColor = tag6_1.tagColor;

                // Label6Demo.Size = new Size(20, 30);
                Label6Demo.TabIndex = 29;
                //      Label6Demo.TextAlign = HorizontalAlignment.Center;
                Label6Demo.AutoSize = true;
                Label6Demo.Text = tag6_1.tag;
                //Label6Demo.BackColor = Color.Blue;

                Label62Demo.BorderStyle = BorderStyle.None;
                Label62Demo.Font = new Font("Calibri", 9.2f, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label62Demo.Location = new Point(30, 5);
                Label62Demo.Name = "Label62Demo";
                Label62Demo.ForeColor = tag6_2.tagColor;
                Label62Demo.Size = new Size(30, 30);
                Label62Demo.TabIndex = 29;
                Label62Demo.AutoSize = true;
                // Label62Demo.BackColor = Color.Blue;
                //      Label62Demo.TextAlign = HorizontalAlignment.Center;
                Label62Demo.Text = tag6_2.tag;
                Label62Demo.Tag = 1;

                Label63Demo.BorderStyle = BorderStyle.None;
                Label63Demo.Font = new Font("Calibri", 9.2f, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label63Demo.Location = new Point(60, 5);
                Label63Demo.Name = "Label63Demo";
                Label63Demo.ForeColor = tag6_3.tagColor;
                //    Label63Demo.Size = new Size(142, 50);
                Label63Demo.TabIndex = 29;
                Label63Demo.AutoSize = true;
                //  Label63Demo.BackColor = Color.Blue;
                //      Label63Demo.TextAlign = HorizontalAlignment.Center;
                Label63Demo.Text = tag6_3.tag;
                Label63Demo.Tag = 1;

                Label64Demo.BorderStyle = BorderStyle.None;
                Label64Demo.Font = new Font("Calibri", 9.2f, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label64Demo.Location = new Point(90, 5);
                Label64Demo.Name = "Label64Demo";
                Label64Demo.ForeColor = tag6_4.tagColor;
                //    Label64Demo.Size = new Size(142, 50);
                Label64Demo.TabIndex = 29;
                Label64Demo.AutoSize = true;
                //      Label64Demo.TextAlign = HorizontalAlignment.Center;
                Label64Demo.Text = tag6_4.tag;
                // Label64Demo.BackColor = Color.Blue;
                Label64Demo.Tag = 1;


                //  Label5Demo.BorderStyle = BorderStyle.FixedSingle;
                Label5Demo.Font = new Font("Calibri", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label5Demo.Location = new Point(0, 75);
                Label5Demo.Name = "Label5Demo";
                // Label5Demo.Size = new Size(144, 10);
                Label5Demo.TabIndex = 28;
                Label5Demo.ForeColor = tag1_3.tagColor;
                Label5Demo.AutoSize = true;
                //  Label5Demo.TextAlign = HorizontalAlignment.Center;
                Label5Demo.Text = tag1_3.tag;


                //  Label4Demo.BorderStyle = BorderStyle.FixedSingle;
                Label4Demo.Font = new Font("Calibri", 20.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
                Label4Demo.Location = new Point(0, 38);
                Label4Demo.Name = "Label4Demo";
                Label4Demo.Size = new Size(130, 10);
                Label4Demo.TabIndex = 27;
                Label4Demo.ForeColor = tag1_2.tagColor;
                Label4Demo.Text = tag1_2.tag;
                Label4Demo.AutoSize = true;
                // Label4Demo.TextAlign = HorizontalAlignment.Center;
                /*  Label3Demo.BorderStyle = BorderStyle.None;
                  Label3Demo.Font = new Font("Calibri", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
                  Label3Demo.ForeColor = Color.Red;
                  Label3Demo.Location = new Point(4, 24);
                  Label3Demo.Name = "Label3Demo";
                  Label3Demo.Size = new Size(139, 15);
                  Label3Demo.TabIndex = 26;
                  Label3Demo.Text = "瑞新電子股份有限公司";*/
                Label2Demo.BorderStyle = BorderStyle.None;
                Label2Demo.Font = new Font("Calibri", 12, FontStyle.Bold, GraphicsUnit.Point, 0);
                Label2Demo.ForeColor = tag3_1.tagColor;
                Label2Demo.Name = "Label2Demo";
                // Label2Demo.Size = new Size(68, 20);
                Label2Demo.TabIndex = 25;
                Label2Demo.Text = tag3_1.tag;
                Label2Demo.AutoSize = true;
                int c = panel3Demo.Width / 2 - Label2Demo.Width / 2;
                Label2Demo.Location = new Point(c, -3);
                //  Label2Demo.TextAlign = HorizontalAlignment.Center;

                label5Demo.AutoSize = true;
                label5Demo.Font = new Font("Calibri", 9.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
                label5Demo.Location = new Point(15, 0);
                label5Demo.ForeColor = tag5_1.tagColor;
                label5Demo.Name = "label5Demo";
                //    label5Demo.Size = new Size(25, 10);
                label5Demo.TabIndex = 21;
                label5Demo.Text = tag5_1.tag;



                label13Demo.AutoSize = true;
                label13Demo.Font = new Font("Calibri", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
                label13Demo.Location = new Point(3, 5);
                label13Demo.Name = "Info";
                //  label3Demo.Size = new Size(10, 14);
                label13Demo.ForeColor = tag2_1.tagColor;
                label13Demo.TabIndex = 19;
                label13Demo.Text = tag2_1.tag;
                panel6labelDemo.AutoSize = true;
                string str = System.AppDomain.CurrentDomain.BaseDirectory;
                //   Console.WriteLine("str" + str);
                //  string filename = str + "circle.jpg";
                //  panel6labelDemo.Image = Image.FromFile(@filename);
                //  panel6labelDemo.BackColor = Color.Red;
                panel6labelDemo.Font = new Font("Calibri", 12, FontStyle.Bold, GraphicsUnit.Point, 0);
                panel6labelDemo.Location = new Point(27, 3);
                panel6labelDemo.Name = "panel6labelDemo";
                panel6labelDemo.Size = new Size(19, 19);
                panel6labelDemo.TabIndex = 100;
                panel6labelDemo.Text = tag2_2.tag;
                panel6labelDemo.ForeColor = Color.White;
                panel6labelDemo.TextAlign = ContentAlignment.MiddleCenter;
                //  panel6labelDemo.Paint += new PaintEventHandler(panel6labelDemo_Paint);





                Label3Demo.BorderStyle = BorderStyle.None;
                Label3Demo.Font = new Font("Calibri", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label3Demo.Location = new Point(0, 3);
                Label3Demo.Name = "Label3Demo";
                //  Label3Demo.Size = new Size(84, 15);
                Label3Demo.ForeColor = tag4_1.tagColor;
                Label3Demo.TabIndex = 26;
                Label3Demo.Text += (tag4_1.tag + tag4_2.tag + tag4_3.tag);
                Label3Demo.AutoSize = true;


                Label18Demo.BorderStyle = BorderStyle.None;
                Label18Demo.Font = new Font("Calibri", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label18Demo.ForeColor = tag4_4.tagColor;
                Label18Demo.Location = new Point(104, 3);
                Label18Demo.Name = "Label18Demo";
                //   Label18Demo.Size = new Size(30, 15);
                Label18Demo.TabIndex = 229;
                Label18Demo.Text += (tag4_4.tag);
                Label18Demo.AutoSize = true;
                // Label18Demo.TextAlign = HorizontalAlignment.Center;

                Label1Demo.BorderStyle = BorderStyle.None;
                Label1Demo.Font = new Font("Calibri", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label1Demo.Location = new Point(0, 15);
                Label1Demo.Name = "Label1Demo";
                Label1Demo.ForeColor = tag4_5.tagColor;
                // Label1Demo.Size = new Size(139, 15);
                Label1Demo.TabIndex = 24;
                Label1Demo.Text += (tag4_5.tag + tag4_6.tag + tag4_7.tag + tag4_8.tag + tag4_9.tag + tag4_10.tag);
                Label1Demo.AutoSize = true;
                //Label1Demo.TextAlign = HorizontalAlignment.Center;


                Label8Demo.BorderStyle = BorderStyle.None;
                Label8Demo.Location = new Point(0, 28);
                Label8Demo.Font = new Font("Calibri", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label8Demo.Name = "Label8Demo";
                Label8Demo.ForeColor = tag4_11.tagColor;
                //   Label8Demo.Size = new Size(87, 15);
                Label8Demo.TabIndex = 8;
                Label8Demo.Text += (tag4_11.tag + tag4_12.tag + tag4_13.tag + tag4_14.tag);
                Label8Demo.AutoSize = true;
                // Label8Demo.TextAlign = HorizontalAlignment.Center;

                Label19Demo.BorderStyle = BorderStyle.None;
                Label19Demo.Font = new Font("Calibri", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
                Label19Demo.Location = new Point(105, 28);
                Label19Demo.Name = "Label19Demo";
                Label19Demo.ForeColor = tag4_15.tagColor;
                //  Label19Demo.Size = new Size(58, 15);
                Label19Demo.TabIndex = 229;
                Label19Demo.Text += (tag4_15.tag);
                Label19Demo.AutoSize = true;
                //    Label19Demo.TextAlign = HorizontalAlignment.Center;

                Label16Demo.BorderStyle = BorderStyle.None;

                Label16Demo.Font = new Font("Calibri", 12, FontStyle.Bold, GraphicsUnit.Point, 0);
                Label16Demo.ForeColor = Color.Red;
                Label16Demo.Name = "Label16Demo";
                Label16Demo.ForeColor = tag3_2.tagColor;
                //    Label16Demo.Size = new Size(68, 15);
                Label16Demo.TabIndex = 55;
                Label16Demo.Text += (tag3_2.tag);
                Label16Demo.AutoSize = true;
                int b = panel3Demo.Width / 2 - Label16Demo.Width / 2;
                Label16Demo.Location = new Point(b, 12);
                //  Label16Demo.TextAlign = HorizontalAlignment.Center;



                Label17Demo.BorderStyle = BorderStyle.None;

                Label17Demo.Font = new Font("Calibri", 12, FontStyle.Bold, GraphicsUnit.Point, 0);
                Label17Demo.ForeColor = Color.Red;
                Label17Demo.Name = "Label17Demo";
                //  Label17Demo.Size = new Size(112, 15);
                pictureBoxa.Location = new Point(25, 10);
                Label17Demo.ForeColor = tag3_3.tagColor;
                Label17Demo.TabIndex = 56;
                Label17Demo.Text += (tag3_3.tag);
                Label17Demo.AutoSize = true;
                int a = panel3Demo.Width / 2 - Label17Demo.Width / 2;
                Label17Demo.Location = new Point(a, 28);
                pictureBoxa.Name = "pictureBoxa";
                pictureBoxa.Size = new Size(146, 30);

                // Bitmap bar = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                BarcodeWriter barcode_w = new BarcodeWriter();
                barcode_w.Format = BarcodeFormat.CODE_39;
                barcode_w.Options.Width = pictureBoxa.Width;
                barcode_w.Options.Height = pictureBoxa.Height;
                barcode_w.Options.PureBarcode = true;
                
                bmp = setESLimageDemo_29(panel29, tag1_1.tag);
                return true;
            } catch (Exception ex){
                return false;
            }
            
        }

        private Bitmap setESLimageDemo_29(Panel panel1, string tag1_1)
        {
            Bitmap bmp = new Bitmap(296, 128);
            try {
                SmcEink mSmcEink = new SmcEink();
                
                using (Graphics graphics = Graphics.FromImage(bmp))
                {
                    graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, 296, 128);
                }

                BarcodeWriter barcodeWriter = new BarcodeWriter()
                {
                    Format = BarcodeFormat.CODE_39
                };

                foreach (Control control in panel1.Controls)
                {
                    if (control is CheckBox)
                    {
                        ((CheckBox)control).Checked = true;
                    }
                    else if (control is Label)
                    {
                        int x = ((Label)control).Location.X;
                        int y = ((Label)control).Location.Y;
                        int width = ((Label)control).Width;
                        int height = ((Label)control).Height;
                        //      Console.WriteLine(string.Concat(new Object[] { x, ",", y, "  w:", width, ", h:", height }));
                        bmp = mSmcEink.ConvertTextToImageDemo(bmp, (Label)control, Color.White, x, y);
                    }
                    else if (control is Panel)
                    {

                        int xPanel = ((Panel)control).Location.X;
                        int yPanel = ((Panel)control).Location.Y;
                        int widthPanel = ((Panel)control).Width;
                        int heightPanel = ((Panel)control).Height;
                        //Console.WriteLine("controlINPanel");


                        bmp = mSmcEink.ConvertPanelToImage(bmp, (Panel)control, Color.Black, xPanel, yPanel);

                    }
                    else if (control is PictureBox)
                    {
                        int num = ((PictureBox)control).Location.X;
                        int y1 = ((PictureBox)control).Location.Y;
                        int width1 = ((PictureBox)control).Width;
                        int height1 = ((PictureBox)control).Height;
                        Bitmap bar93 = new Bitmap(width1, height1);

                        barcodeWriter.Options.Width = width1;
                        barcodeWriter.Options.Height = height1;

                        barcodeWriter.Options.Margin = 0;
                        barcodeWriter.Options.PureBarcode = true;
                        bar93 = barcodeWriter.Write(tag1_1);
                        bmp = mSmcEink.ConvertBarToImage(bmp, bar93, num, y1);
                    }
                    else if (control is Label)
                    {
                        int x1 = ((Label)control).Location.X;
                        int num1 = ((Label)control).Location.Y;
                        int width2 = ((Label)control).Width;
                        int height2 = ((Label)control).Height;
                        //  Console.WriteLine(string.Concat(new Object[] { "Label ", x1, ",", num1, "  w:", width2, ", h:", height2 }));
                        bmp = mSmcEink.ConvertTextToImageDemo(bmp, (Label)control, Color.Black, x1, num1);
                    }
                }

            } catch (Exception ex) {
                //Console.WriteLine("exe  "+ex);
            }



            return bmp;
        }


        private void OnSMCEslReceiveEvent(Object sender ,EventArgs e)
            {


            int msgId = (e as NfcEslLib.SMCEslReceiveEventArgs).msgId;
            bool status = (e as NfcEslLib.SMCEslReceiveEventArgs).status;
            int Index = (e as NfcEslLib.SMCEslReceiveEventArgs).Index;
            byte[] data = (e as NfcEslLib.SMCEslReceiveEventArgs).data;

            string NFCID = ByteArrayToString(data);
            if (msgId == 1 && status == true)
                ddt = "msgId:" + msgId + ",status:" + status + ",NFCID:" + NFCID;
            else
                ddt = "msgId:" + msgId + ",status:" + status;
           

            //  Console.WriteLine("ddt " + ddt);
            string result = "msgId:" + msgId + ",Index:" + Index + ",status:" + status + ",data:" + ByteArrayToString(data);
            byte[] msg = Encoding.ASCII.GetBytes(result);
            foreach (Socket socket in clientSockets)
                    socket.Send(msg); //send everything to all clients as bytes
           


        }

        byte[] ObjectToByteArray(Object obj)
        {
           // Console.WriteLine(obj);
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            //Console.WriteLine("dddddd "+ms.ToArray().Length);
            return ms.ToArray();
            
        }

        private string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }



    [Serializable]
    public class SMCEslReceiveEventArgs : EventArgs
    {
        public int msgId;
        public bool status;
        public int Index;
        public byte[] data;
    }
}
