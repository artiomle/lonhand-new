using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace TempGetGUI_Raspberry
{
    public class LonHandAPI
    {
        private enum enumCmd { OPEN = 1, CLOSE = 0, STATUS = 2, GET_NAME = 3, CLOSE_ALL = 4, STATUS_ALL = 5 };
        static TcpClient m_oTcpClient = new TcpClient();
        const int m_iLonHandPort = 8899;
        static byte[] baOpen = new byte[8] { 0x55, 0xaa, 0x00, 0x03, 0x00, 0x02, 0x01, 0x06 };
        static byte[] baClose = new byte[8] { 0x55, 0xaa, 0x00, 0x03, 0x00, 0x01, 0x01, 0x05 };
        static byte[] baCloseResponse = new byte[9] { 0xaa, 0x55, 0x00, 0x04, 0x00, 0x81, 0x01, 0x00, 0x86 };
        static byte[] baOpenResponse = new byte[9] { 0xaa, 0x55, 0x00, 0x04, 0x00, 0x82, 0x01, 0x01, 0x88 };
        static byte[] baStatusRequest = new byte[7] { 0x55, 0xaa, 0x00, 0x02, 0x00, 0x0a, 0x0c };
        static byte[] baStatusClose = new byte[8] { 0xaa, 0x55, 0x00, 0x03, 0x00, 0x8a, 0x00, 0x8d };
        static byte[] baStatusOpen = new byte[8] { 0xaa, 0x55, 0x00, 0x03, 0x00, 0x8a, 0x00, 0x8e };
        static byte[] baStatusOpen2 = new byte[8] { 0xaa, 0x55, 0x00, 0x03, 0x00, 0x8a, 0x01, 0x8e };
        static byte[] baNameGetRequest = new byte[7] { 0x55, 0xaa, 0x00, 0x02, 0x00, 0x63, 0x65 };

        // <summary>
        // 1 on
        // 0 off
        // -1 offline
        // </summary>
        // <param name="IP"></param>
        // <returns></returns>

        public int Device_Status_Get(string IP)
        {
            byte[] baRes = null;
            int iRet = -1;
            baRes = Request(IP, m_iLonHandPort, enumCmd.STATUS);
            if (baRes != null)
            {
                if (ByteArrayToString(baRes) == ByteArrayToString(baStatusClose))
                {
                    log("Device is OFF");
                    iRet = 0;
                }
                else if (ByteArrayToString(baRes) == ByteArrayToString(baStatusOpen) || ByteArrayToString(baRes) == ByteArrayToString(baStatusOpen2))
                {
                    log("Device is ON");
                    iRet = 1;
                }
                else
                {
                    log("Device status unkonwn");
                    iRet = -1;
                }
            }
            else
                iRet = -1;

            return iRet;
        }


        public bool Device_Status_Set(string IP, bool IsOpen)
        {
            byte[] baRes = null;
            bool bRes = false;
            enumCmd eCmd = enumCmd.OPEN;
            if (IsOpen)
                eCmd = enumCmd.OPEN;
            else
                eCmd = enumCmd.CLOSE;

            baRes = Request(IP, m_iLonHandPort, eCmd);
            if (baRes == null)
                bRes = false;
            else
            {
                if (IsOpen)
                {
                    if (ByteArrayToString(baRes) == ByteArrayToString(baOpenResponse))
                    {
                        log("Device Turned ON successfully");
                        bRes = true;
                    }
                    else
                    {
                        log("Unknown response recieved from device");
                        bRes = false;
                    }
                }
                else
                {
                    if (ByteArrayToString(baRes) == ByteArrayToString(baCloseResponse))
                    {
                        log("Device Turned OFF successfully");
                        bRes = true;
                    }
                    else
                    {
                        log("Unknown response recieved from device");
                        bRes = false;
                    }
                }
            }
            return bRes;
        }


        private static byte[] Request(string IP, int Port, enumCmd enumCommand)
        {
            return Request(IP, Port, enumCommand, 1500, 1500);
        }


        private static byte[] Request(string IP, int Port, enumCmd enumCommand, int ReceiveTimeout, int SendTimeout)
        {
            byte[] baRes = null;
            Stream stm = null;
            Stopwatch stopwatch = new Stopwatch();
            byte[] ba = new byte[8];
            if (enumCommand == enumCmd.OPEN)
                ba = baOpen;
            else if (enumCommand == enumCmd.CLOSE)
                ba = baClose;
            else if (enumCommand == enumCmd.STATUS)
                ba = baStatusRequest;
            else if (enumCommand == enumCmd.GET_NAME)
                ba = baNameGetRequest;
        
            m_oTcpClient = new TcpClient();
            stopwatch.Start();

            try
            {
                var result = m_oTcpClient.BeginConnect(IP, Port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(SendTimeout));

                if (!success)
                {
                    throw new Exception("Failed connecting to Device");
                }

                
                m_oTcpClient.EndConnect(result);

                
                stm = m_oTcpClient.GetStream();
                stm.ReadTimeout = ReceiveTimeout;
                stm.Write(ba, 0, ba.Length);
                
                byte[] bResponse = new byte[m_oTcpClient.ReceiveBufferSize];

                try
                {
                    int iBytesRead = stm.Read(bResponse, 0, bResponse.Length);

                    long lStopwatch = stopwatch.ElapsedMilliseconds;
                    baRes = new byte[iBytesRead];
                    for (int i = 0; i < iBytesRead; i++)
                        baRes[i] = bResponse[i];

                }
                catch (IOException e2)
                {
                    log("read IOException:" + e2.Message);
                }
            }
            catch (Exception e2)
            {
                log("request Exception=" + e2.Message);
            }
            finally
            {
                m_oTcpClient.Close();
            }
            return baRes;
        }


        public static string ByteArrayToHex(byte[] ba)
        {
            return ByteArrayToHex(ba, true);
        }


        public static string ByteArrayToHex(byte[] ba, bool word)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                if (word)
                {
                    hex.Append("{");
                    hex.AppendFormat("{0:x2}", b);
                    hex.Append("}");
                }
                else
                {
                    hex.AppendFormat("{0:x2}", b);
                }
            }
            return hex.ToString();
        }


        public static string ByteArrayToString(byte[] ba)
        {
           return System.Text.Encoding.ASCII.GetString(ba);
        }


        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        static private void log(string m)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss - ") + m);
        }

        
    }
}
