using AC;
using ICSharpCode.SharpZipLib.GZip;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using LitJson;

class packetdecode
{
    public static int realtimeSinceLogin = 0;
    public static int loginTime = 0;
    public static string init()
    {
       // loginTime = time;
        AuthCode.Init(new AuthCode.IntDelegate(GetCurrentTimeStamp));
        return GetCurrentTimeStamp().ToString();
    }
    public static string Getsigns(string result)
    {
        JsonData jsonData2 = null;
        // GameData.realtimeSinceLogin = Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);



        //GameData.loginTime = ConvertDateTime_China_Int(DateTime.Now);
        try
        {
            using (MemoryStream stream = new MemoryStream(AuthCode.DecodeWithGzip(result.Substring(1), "yundoudou")))
            {
                using (Stream stream2 = new GZipInputStream(stream))
                {
                    using (StreamReader streamReader = new StreamReader(stream2, Encoding.Default))
                    {
                        jsonData2 = JsonMapper.ToObject(streamReader);
                    }
                }
            }
        }
        catch //(Exception e)
        {
            //Console.WriteLine(e.ToString());
            //throw;
            return string.Empty;
        }
        //Console.WriteLine(jsonData2["uid"].ToString());
        // Console.WriteLine(jsonData2["sign"].ToString());
        return jsonData2["sign"].ToString()+";"+ jsonData2["uid"].ToString();
        //return ProgrameData.sign;

    }



    public static int GetCurrentTimeStamp()
    {
        return Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - realtimeSinceLogin + loginTime);
    }

    public static string Decode(string m, string s)
    {
        try
        {
            if (m.StartsWith("#"))
            {
                using (MemoryStream stream = new MemoryStream(AuthCode.DecodeWithGzip(m.Substring(1), s)))
                {
                    using (Stream stream2 = new GZipInputStream(stream))
                    {
                        using (StreamReader reader = new StreamReader(stream2, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            return AuthCode.Decode(m, s);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString(), "Decode Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return e.ToString();
        }
    }

    public static string Encode(string data, string signcode)
    {
        return AuthCode.Encode(data, signcode);
    }
    private static byte[] GetKey(byte[] pass, int kLen)
    {
        byte[] buffer = new byte[kLen];
        for (long i = 0L; i < kLen; i += 1L)
        {
            buffer[(int)((IntPtr)i)] = (byte)i;
        }
        long num = 0L;
        for (long j = 0L; j < kLen; j += 1L)
        {
            num = ((num + buffer[(int)((IntPtr)j)]) + pass[(int)((IntPtr)(j % ((long)pass.Length)))]) % ((long)kLen);
            byte num4 = buffer[(int)((IntPtr)j)];
            buffer[(int)((IntPtr)j)] = buffer[(int)((IntPtr)num)];
            buffer[(int)((IntPtr)num)] = num4;
        }
        return buffer;
    }
  
}