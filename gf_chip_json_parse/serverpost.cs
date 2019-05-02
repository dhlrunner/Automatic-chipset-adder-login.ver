using System;
using System.Net;
using System.Text;
using AC;

class server
{
    
    public static DateTime Delay(int MS)
    {
        DateTime ThisMoment = DateTime.Now;
        TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
        DateTime AfterWards = ThisMoment.Add(duration);

        while (AfterWards >= ThisMoment)
        {
            System.Windows.Forms.Application.DoEvents();
            ThisMoment = DateTime.Now;
        }

        return DateTime.Now;
    }


    public static string login(string id)
    {
        WebClient wc = new WebClient();
        string data = "id=&appid=158714&ver=8&time=1556708527&email=" +id+
            "&fuid=android_kr_snqx&device_id=731478aa6cade063&binding=1&imei=&mac=&" +
            "l=kr&isguest=google&device_token=&fbl=1600_900&os=5.1.1%28REL%29&dev=Gentle" +
            "&cpu=x86&men=0&appver=2.0311_233&buildnumber=233&" +
            "sys=android&adid=84d651e3-2a4b-4a25-a909-a6a98bf559aa&platform=txwy&sig=VMFu9nIVDLz397Qe8EONt7LXhP4%3D";
        wc.Headers.Add("User-Agent", "Apache-HttpClient/UNAVAILABLE (java 1.4)");
        //wc.Headers.Add("Content-Length", data.Length.ToString());
        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
        //wc.Headers.Add("Connection", "Keep-Alive");
        wc.Headers.Add("Host", "p.txwy.tw");
        wc.Encoding = Encoding.UTF8;
        byte[] req = Encoding.ASCII.GetBytes(data);
        byte[] result = wc.UploadData(ReqURLs.login, "POST", req);
        //string a = System.IO.File.ReadAllText("C:\\Users\\dhl\\Desktop\\a.txt");
        return Encoding.UTF8.GetString(result);
    }
    public static string GetEncrypedSigntoken(string sid,string openid, long time) 
    {
        
        Random randomdelay = new Random();
        Delay(randomdelay.Next(2000, 4000));
        WebClient wc = new WebClient();
        string data = string.Format("openid={0}&sid={1}&req_id={2}{3}",openid,sid,time.ToString(),"00002");
        wc.Headers.Add("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 5.1.1; Gentle Build/LMY49I)");
        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        wc.Headers.Add("Host", "gf-game.girlfrontline.co.kr");
        wc.Headers.Add("Accept-Encoding", "none");
        wc.Headers.Add("X-Unity-Version", "2017.4.21f1");
        wc.Encoding = Encoding.UTF8;
        byte[] req = Encoding.ASCII.GetBytes(data);
        byte[] result = wc.UploadData(ReqURLs.getsigntoken_kr, "POST", req);      
        string result_str= Encoding.UTF8.GetString(result);
        if (result_str.Contains("error"))
        {
            Delay(randomdelay.Next(3000,5000));
            data = string.Format("openid={0}&sid={1}&req_id={2}{3}", openid, sid, time.ToString(), "00003");           
            wc.Headers.Add("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 5.1.1; Gentle Build/LMY49I)");
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.Headers.Add("Host", "gf-game.girlfrontline.co.kr");
            wc.Headers.Add("Accept-Encoding", "none");
            wc.Headers.Add("X-Unity-Version", "2017.4.21f1");
            wc.Encoding = Encoding.UTF8;
            req = Encoding.ASCII.GetBytes(data);
            result = wc.UploadData(ReqURLs.getsigntoken_kr, "POST", req);
            result_str = Encoding.UTF8.GetString(result);
            return result_str;
        }
        else
        {
            return result_str;
        }
       
    }
    public static string GetUserInfotoJson(string uid,string signtoken,long time)
    {
        Random randomdelay = new Random();
        Delay(randomdelay.Next(3000, 6000));
        WebClient wc = new WebClient();
        string willencdata = "{\"time\":" + packetdecode.GetCurrentTimeStamp().ToString() + "}";
        string data = string.Format("uid={0}&outdatacode={1}&req_id={2}{3}",uid,packetdecode.Encode(willencdata,signtoken),time.ToString(),"00004");
        wc.Headers.Add("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 5.1.1; Gentle Build/LMY49I)");
        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        wc.Headers.Add("Host", "gf-game.girlfrontline.co.kr");
        wc.Headers.Add("Accept-Encoding", "none");
        wc.Headers.Add("X-Unity-Version", "2017.4.21f1");
        wc.Encoding = Encoding.UTF8;
        byte[] req = Encoding.ASCII.GetBytes(data);
        byte[] result = wc.UploadData(ReqURLs.getuserinfo_kr, "POST", req);
        return Encoding.UTF8.GetString(result);
    }
    
}