using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace EslLibCom
{


    public class NfcEslLib

    {
        private SerialPort port = new SerialPort();
        public event EventHandler onSMCEslReceiveEvent;
        private List<byte> packet = new List<byte>();
        delegate void Display(Byte[] buffer);// UI讀取用
        //private  byte[] blackRandomN = new byte[16] { 0x36, 0x5A, 0xC5, 0x7A, 0x29, 0xB3, 0x1D, 0x8E, 0x3B, 0x59, 0x97, 0xF1, 0xC2, 0x4E, 0xD4, 0xA3 };
        //private  byte[] redRandomN = new byte[16] { 0x5C, 0x99, 0xF5, 0x12, 0xD6, 0x3A, 0x38, 0x5C, 0x49, 0xE4, 0xAA, 0x67, 0x91, 0xBD, 0x83, 0x2F };

        public int msg_ReadEslName = 1;
        public int msg_WriteEslData = 2;
        public int msg_SetEslDefault = 3;
        public int msg_WriteEslDataFinish = 4;
        public int msg_WriteEslClose = 5;
        public int msg_WriteEslPackageIndex = 6;

        private string t, r;
        private int total_count = 0;
        private int substringcount = 32;
        private int count = 0;
        private string UUIDPacket = "040116";
        private string WriteBlackPacket = "150127" + "00";
        private string WriteBlackEPacket = "150127" + "ff" + "ffffffffffffffffffffffffffffffff";
        private string WriteRed4Packet = "150127" + "40";
        private string WriteRed8Packet = "150127" + "80";
        private string WriteRedFPacket = "150127" + "ff";
        private string WriteRedEPacket = "150127" + "ff" + "ffffffffffffffffffffffffffffffff";
        private string EslClosePacket = "05011701";
        private string EslSetingPacket = "090126" + "f80829c001";
        private string EslSeting2ColorPacket = "090126" + "f8082Ac001";
        private byte tag0 ;
        private byte tag1;
        
        Boolean Red = false;


        public class SMCEslReceiveEventArgs: EventArgs
    {
        public int  msgId;
        public bool status;
        public int Index;
        public byte[] data;
    }


        public bool portOpen(string portName, string baurate)
        {
            try
            {
                //Console.WriteLine("portName"+ portName + "baurate"+ baurate);
                int com = Convert.ToInt32(baurate);
                port.PortName = portName;
                this.port.BaudRate = com;            // baud rate = 9600
                this.port.Parity = Parity.None;       // Parity = none
                this.port.StopBits = StopBits.One;    // stop bits = one
                this.port.DataBits = 8;               // data bits = 8
                this.port.WriteTimeout = 1000;
                this.port.ReadTimeout = 1000;
                this.port.Handshake = Handshake.None;
                // 設定 PORT 接收事件
                port.DataReceived += new SerialDataReceivedEventHandler(port1_DataReceived);

                // 打開 PORT
                port.Open();
                return true;
            }
            catch (Exception ex)
            {
                port.Close();
                return false;
            }
        }


        public bool portIsOpen()
        {
                if (this.port == null)
                {
                    return false;
                }
            return this.port.IsOpen;
        }


        //接收UART資料
        private void port1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // stopwatchtext.Stop();
            //     Console.WriteLine("start" + stopwatchtext.Elapsed.Milliseconds + "ms");
            while (port.BytesToRead != 0)
            {


                packet.Add((byte)port.ReadByte());
            }
            byte[] bArrary = packet.ToArray();
            if (bArrary.Length > 1)
            {
                DisplayTextString(bArrary);


            }
        }

        public bool EslSeting()
        {
            try {
                string com = EslSetingPacket;
                byte[] bcom = iCheckSum(StringToByteArray(com));

                SendData(bcom);
                return true;
            }
            catch (Exception ex) {
                return false;
            }

        }

        public bool EslSeting2Color()
        {


            try
            {
                string com = EslSeting2ColorPacket;
                byte[] bcom = iCheckSum(StringToByteArray(com));

                SendData(bcom);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        

        public bool eslImagePix(Bitmap bmp) {
            try
            {
                string bit = "";
                string rbit = "";
                string totaldata = "";
                string totalreddata = "";
                count = 0;
                total_count = 0;
                Red = true;
                Color color;
                for (int i = bmp.Width - 1; i >= 0; i--)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        color = bmp.GetPixel(i, j);
                        if (color.ToArgb() == Color.Black.ToArgb())
                        {
                            bit = bit + "0";
                        }
                        else
                        {
                            bit = bit + "1";
                        }

                        if (color.ToArgb() == Color.Red.ToArgb())
                        {
                            rbit += "1";
                        }
                        else
                        {
                            rbit += "0";
                        }

                        if (bit.Length == 8)
                        {
                            totaldata = totaldata + Convert.ToInt32(bit, 2).ToString("X2");
                            totalreddata = totalreddata + Convert.ToInt32(rbit, 2).ToString("X2");
                            bit = "";
                            rbit = "";
                        }
                    }
                }
                t = totaldata;
                r = totalreddata;

                for (int i = t.Length; i < 5792; i++)//2896
                {
                    t = t + "0";
                    r = r + "0";
                }
                t = blackEnCode(t);
                r = redEnCode(r);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public bool eslImagePix2Color(Bitmap bmp)
        {
            try
            {
                string bit = "";
                string rbit = "";
                string totaldata = "";
                string totalreddata = "";
                count = 0;
                total_count = 0;
                Red = false;
                Color color;
                for (int i = bmp.Width - 1; i >= 0; i--)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        color = bmp.GetPixel(i, j);
                        if (color.ToArgb() == Color.Black.ToArgb())
                        {
                            bit = bit + "0";
                        }
                        else
                        {
                            bit = bit + "1";
                        }

                        if (color.ToArgb() == Color.Red.ToArgb())
                        {
                            rbit += "1";
                        }
                        else
                        {
                            rbit += "0";
                        }

                        if (bit.Length == 8)
                        {
                            totaldata = totaldata + Convert.ToInt32(bit, 2).ToString("X2");
                            totalreddata = totalreddata + Convert.ToInt32(rbit, 2).ToString("X2");
                            bit = "";
                            rbit = "";
                        }
                    }
                }
                t = totaldata;
                r = totalreddata;

                for (int i = t.Length; i < 5792; i++)//2896
                {
                    t = t + "0";
                    r = r + "0";
                }
                t = blackEnCode(t);
                r = redEnCode(r);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           

        }

        public bool eslImageLockBit(Bitmap bmp) {

            try
            {
                string bit = "";
                string rbit = "";
                string totaldata = "";
                string totalreddata = "";
                count = 0;
                total_count = 0;
                Red = true;
                Color color;
                bmp = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format24bppRgb);
                int wide = bmp.Width;
                int height = bmp.Height;
                Rectangle rect = new Rectangle(0, 0, wide, height);

                //將srcBitmap鎖定到系統內的記憶體的某個區塊中，並將這個結果交給BitmapData類別的srcBimap
                BitmapData srcBmData = bmp.LockBits(rect, ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);


                //位元圖中第一個像素數據的地址。它也可以看成是位圖中的第一個掃描行
                //目的是設兩個起始旗標srcPtr、dstPtr，為srcBmData、dstBmData的掃描行的開始位置
                System.IntPtr srcPtr = srcBmData.Scan0;

                //將Bitmap對象的訊息存放到byte中
                int src_bytes = srcBmData.Stride * height;
                byte[] srcValues​​ = new byte[src_bytes];


                //複製GRB信息到byte中
                System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues​​, 0, src_bytes);


                for (int j = wide - 1; j >= 0; j--)
                    for (int i = 0; i < height; i++)
                    {
                        int k = 3 * j;
                        color = Color.FromArgb(srcValues​​[i * srcBmData.Stride + k + 2], srcValues​​[i * srcBmData.Stride + k + 1], srcValues​​[i * srcBmData.Stride + k]);

                        if (color.ToArgb() == Color.Black.ToArgb())
                        {
                            bit = bit + "0";
                        }
                        else
                        {
                            bit = bit + "1";
                        }

                        if (color.ToArgb() == Color.Red.ToArgb())
                        {
                            rbit += "1";
                        }
                        else
                        {
                            rbit += "0";
                        }

                        if (bit.Length == 8)
                        {
                            totaldata = totaldata + Convert.ToInt32(bit, 2).ToString("X2");
                            totalreddata = totalreddata + Convert.ToInt32(rbit, 2).ToString("X2");
                            bit = "";
                            rbit = "";
                        }
                        //   Console.WriteLine(red + "   " + wide + "   "+x);



                    }
                bmp.UnlockBits(srcBmData);


                t = totaldata;
                r = totalreddata;

                for (int i = t.Length; i < 5792; i++)//2896
                {
                    t = t + "0";
                    r = r + "0";
                }
                t = blackEnCode(t);
                r = redEnCode(r);
                return true;
            }
            catch (Exception ex) {
                return false;
            }
            
        }


        private string blackEnCode(string data)
        {
            byte[] bytes = StringToByteArray(data);
            byte[] blackRandomN = new byte[16] { 0x36, 0x5A, 0xC5, 0x7A, 0x29, 0xB3, 0x1D, 0x8E, 0x3B, 0x59, 0x97, 0xF1, 0xC2, 0x4E, 0xD4, 0xA3 };
            byte[] bytesend = new byte[0];
            
            byte xx = new byte();
            string aaa = "";
            int count16 = 0;

            int tol = (tag0 + tag1) % 256;

            if (tol == 0)
                xx = 0x55;
            else
                xx = Convert.ToByte(tol);


            for (int i = 0; i < bytes.Length; i++)
            {


                if (count16 == 16)
                {
                    count16 = 0;
                }
                aaa = aaa + ((byte)(((bytes[i])) ^ (xx + blackRandomN[count16]))).ToString("X2");
                count16++;
            }

            return aaa;
        }


        private  string redEnCode(string data)
        {
            byte[] bytes = StringToByteArray(data);
            byte[] redRandomN = new byte[16] { 0x5C, 0x99, 0xF5, 0x12, 0xD6, 0x3A, 0x38, 0x5C, 0x49, 0xE4, 0xAA, 0x67, 0x91, 0xBD, 0x83, 0x2F };
            byte[] bytesend = new byte[0];
            string aaa = "";
            int count16 = 0;
            byte xx = new byte();
            int tol = (tag0 + tag1) % 256;

            if (tol == 0)
                xx = 0x55;
            else
                xx = Convert.ToByte(tol);


            for (int i = 0; i < bytes.Length; i++)
            {

                if (count16 == 16)
                {
                    count16 = 0;
                }
                aaa = aaa + ((byte)(((bytes[i])) ^ (xx + redRandomN[count16]))).ToString("X2");
                count16++;
            }

            return aaa;
        }


        public bool eslUUIDRead()
        {
            try {
                string com = UUIDPacket;
                byte[] bcom = iCheckSum(StringToByteArray(com));
                SendData(bcom);
                return true;
            } catch (Exception ex) {
                return false;
            }
        }

        private void sendWriteBlackData(string data)
        {

          
            string com = WriteBlackPacket + data;
          //  Console.WriteLine("writeblack" + com);
            byte[] bcom = iCheckSum(StringToByteArray(com));
            SendData(bcom);
        }

        private void sendWriteBlackDataEnd()
        {


            string com = WriteBlackEPacket;
            byte[] bcom = iCheckSum(StringToByteArray(com));
            SendData(bcom);
        }

        private void sendWriteRedData4(string data)
        {


            string com = WriteRed4Packet + data;
            byte[] bcom = iCheckSum(StringToByteArray(com));
            SendData(bcom);
        }

        private void sendWriteRedData8(string data)
        {

            string com = WriteRed8Packet + data;
            byte[] bcom = iCheckSum(StringToByteArray(com));
            SendData(bcom);
        }

        private void sendWriteRedDataF(string data)
        {


            string com = WriteRedFPacket + data;
            byte[] bcom = iCheckSum(StringToByteArray(com));
            SendData(bcom);
        }

        public bool sendWriteEslClose()
        {
            try {
                string com = EslClosePacket;
                byte[] bcom = iCheckSum(StringToByteArray(com));
                SendData(bcom);
                return true;
            } catch (Exception ex) {
                return false;
            }

        }



        private void sendWriteRedDataEnd()
        {

            string com = WriteRedEPacket;
            byte[] bcom = iCheckSum(StringToByteArray(com));
            SendData(bcom);
        }

        private void SendData(byte[] data)
        {

            if (port.IsOpen)
            {
             
                port.Write(data, 0, data.Count());

            }

        }


        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private static byte[] iCheckSum(byte[] data)
        {
            byte[] bytes = new byte[data.Length + 1];
            int intValue = 0;
            for (int i = 0; i < data.Length; i++)
            {
                intValue = intValue + (int)data[i];
                bytes[i] = data[i];
            }
            byte[] intBytes = BitConverter.GetBytes(intValue);
            Array.Reverse(intBytes);
            bytes[data.Length] = (byte)(intBytes[intBytes.Length - 1] ^ (byte)0xff);
            //  Console.WriteLine(ByteArrayToString(bytes) + "");
            return bytes;
        }


        private void DisplayTextString(byte[] RX)
        {
            packet.Clear();
            //05012700d2
            //05012600d3
            string data = null;
           /* foreach (byte ddd in RX) {
                data = data + ddd.ToString("X2");
            }
            Console.WriteLine("data"+data);*/


            if (RX[0] == (byte)0x05 && RX[2] == (byte)0x26 && RX[3] == (byte)0x00)
            {
                System.Threading.Thread.Sleep(2000);
                
                SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                obj.msgId = msg_SetEslDefault;
                obj.status = true;
                obj.data = RX;
                onSMCEslReceiveEvent(this, obj);
                sendWriteBlackData(t.Substring(0, substringcount));

            }
            else if (RX[0] == (byte)0x05 && RX[2] == (byte)0x26 && RX[3] != (byte)0x00)
            {

                SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                obj.msgId = msg_SetEslDefault;
                obj.status = false;
                obj.data = RX;
                onSMCEslReceiveEvent(this, obj);
            }

            if (RX[0] == (byte)0x05 && RX[2] == (byte)0x27 && RX[3] == (byte)0x00)
            {
              //  Console.WriteLine("count===" + count + "tLENGTH" + t.Length);
                if (count < (t.Length / 32))
                {
                    //   texMessageBox.Text = "黑 寫入成功:" + count + "\r\n";
                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    obj.msgId = msg_WriteEslPackageIndex;
                    obj.Index = count;
                    obj.data = RX;
                    obj.status = true;
                    onSMCEslReceiveEvent(this, obj);

                    sendWriteBlackData(t.Substring(count * substringcount, substringcount));
                    count++;
                }
                else if (count == (t.Length / 32))
                {
                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    obj.msgId = msg_WriteEslPackageIndex;
                    obj.Index = count;
                    obj.status = true;
                    obj.data = RX;
                    onSMCEslReceiveEvent(this, obj);

                    total_count = 0;
                    count++;
                    if (Red == false)
                    {
                        sendWriteBlackDataEnd();
                        // texMessageBox.Text = texMessageBox.Text + "傳送結束開始更新電子紙" + "\r\n";
                    }
                }
                if (count > (t.Length / 32) && total_count == 0 && Red == true)
                {
                    //  texMessageBox.Text = "紅 寫入成功:" + total_count  + "\r\n";
                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    obj.msgId = msg_WriteEslPackageIndex;
                    obj.Index = count;
                    obj.status = true;
                    obj.data = RX;
                    onSMCEslReceiveEvent(this, obj);

                    sendWriteRedData4(r.Substring(total_count * substringcount, substringcount));
                    count++;
                    total_count++;
                }
                else if (count > (t.Length / 32) && total_count < ((r.Length / 32) - 1) && Red == true)
                {

                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    obj.msgId = msg_WriteEslPackageIndex;
                    obj.Index = count;
                    obj.status = true;
                    obj.data = RX;
                    onSMCEslReceiveEvent(this, obj);

                    ///   texMessageBox.Text = "紅 寫入成功:" + total_count  + "\r\n";
                    ///   
                    sendWriteRedData8(r.Substring(total_count * substringcount, substringcount));
                    count++;
                    total_count++;
                }
                else if (count > (t.Length / 32) && total_count == ((r.Length / 32) - 1) && Red == true)
                {
                    //  texMessageBox.Text = "紅 寫入成功:" + total_count  + "\r\n";
                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    obj.msgId = msg_WriteEslPackageIndex;
                    obj.Index = count;
                    obj.status = true;
                    obj.data = RX;
                    onSMCEslReceiveEvent(this, obj);

                    sendWriteRedDataF(r.Substring(total_count * substringcount, substringcount));
                    count++;
                    total_count++;

                }
                else if (count > (t.Length / 32) && total_count == (r.Length / 32) && Red == true)
                {

                    //  texMessageBox.Text = "紅 寫入成功 :" + total_count  + "\r\n";

                    //   texMessageBox.Text = texMessageBox.Text + "傳送秒數:" + stopwatchtext.Elapsed.Milliseconds + "ms" + "\r\n";
                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    obj.msgId = msg_WriteEslPackageIndex;
                    obj.Index = count;
                    obj.status = true;
                    obj.data = RX;
                    onSMCEslReceiveEvent(this, obj);

                    sendWriteRedDataEnd();
                    count++;
                    total_count++;



                }

                else if (count > (t.Length / 32) && total_count > (r.Length / 32) && Red == true)
                {
                  //  count = 0;
                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    obj.msgId = msg_WriteEslDataFinish;
                    obj.data = RX;
                    obj.status = true;
                    onSMCEslReceiveEvent(this, obj);

                }
                else if (count > (t.Length / 32) &&  Red == false)
                {
                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    obj.msgId = msg_WriteEslDataFinish;
                    obj.data = RX;
                    obj.status = true;
                    onSMCEslReceiveEvent(this, obj);
                }
                else
                {
                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    obj.msgId = msg_WriteEslData;
                    obj.data = RX;
                    obj.status = true;
                    onSMCEslReceiveEvent(this, obj);
                }



            }
            else if (RX[0] == (byte)0x05 && RX[2] == (byte)0x27)
            {
                //error

                //sendWriteEslClose();
                SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                obj.msgId = msg_WriteEslData;
                obj.status = false;
                obj.data = RX;
                onSMCEslReceiveEvent(this, obj);
            }
            else if (RX[0] == (byte)0x05 && RX[2] == (byte)0x17 && RX[3] == (byte)0x01)
            {
                
                SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                obj.msgId = msg_WriteEslClose;
                obj.status = true;
                obj.data = RX;
                onSMCEslReceiveEvent(this, obj);
            }
            else
            {

                if ( RX[2] == (byte)0x16 && RX[3] == (byte)0x01)
                {

                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    tag0 = RX[4];
                    tag1 = RX[5];
                    obj.msgId = msg_ReadEslName;
                    obj.data = RX;
                    obj.status = true;
                    onSMCEslReceiveEvent(this, obj);
                }
                else if ( RX[2] == (byte)0x16 && RX[3] != (byte)0x01)
                {
                    SMCEslReceiveEventArgs obj = new SMCEslReceiveEventArgs();
                    obj.msgId = msg_ReadEslName;
                    obj.data = RX;
                    obj.status = false;
                    onSMCEslReceiveEvent(this, obj);
                }

            }


        }
    }
}
