using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace gf_chip_json_parse
{
    public delegate void FormSendDataHandler(Object obj);
    public partial class Form2 : Form
    {
        public event FormSendDataHandler form2SendEvent;
        public static string sign = string.Empty;
        public static string openid = string.Empty;
        public static long sevtime = 0;
        public static string[] signtoken = new string[2];
        public static string userjson;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            if (textBox1.Text != string.Empty)
            {
                if (textBox1.Text.Contains("@"))
                {
                    string encsigntoken = string.Empty;
                    richTextBox1.Clear();
                    richTextBox1.Text += "Decode module initialized to " + packetdecode.init() + "\n";
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    richTextBox1.Text += "\""+ ReqURLs.login+"\"에 접속 중...\n";
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    richTextBox1.Text += textBox1.Text + " 으로 로그인 시도 중...\n";
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                    string result = server.login(textBox1.Text);
                    try
                    {
                        JObject json = JObject.Parse(result);                       
                        //richTextBox1.Text += json.ToString();
                        try
                        {                            
                            openid = json["uid"].ToString();
                            sign = json["sid"].ToString();
                            sevtime = long.Parse(json["time"].ToString());
                            richTextBox1.Text += "서버 메세지: " + System.Text.RegularExpressions.Regex.Unescape(@json["msg"].ToString()) + "\n";
                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                            richTextBox1.ScrollToCaret();
                            richTextBox1.Text += "서버 시간(unix): " +sevtime.ToString() + "\n";
                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                            richTextBox1.ScrollToCaret();
                            richTextBox1.Text += "유저명: " + json["username"].ToString()+"\n";
                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                            richTextBox1.ScrollToCaret();
                            richTextBox1.Text += "sid: " +sign+ "\n";
                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                            richTextBox1.ScrollToCaret();
                            richTextBox1.Text += "openid: " + openid+ "\n";
                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                            richTextBox1.ScrollToCaret();
                            try
                            {
                                richTextBox1.Text += "\"" + ReqURLs.getsigntoken_kr + "\"에 접속 중...\n";
                                richTextBox1.Text += "토큰 정보 취득 중...\n";
                                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                richTextBox1.ScrollToCaret();
                                encsigntoken = server.GetEncrypedSigntoken(sign, openid, sevtime);
                                signtoken = packetdecode.Getsigns(encsigntoken).Split(';');
                                if (signtoken[0] == string.Empty)
                                    throw new Exception("빈 문자열이 반환되었습니다.");
                                richTextBox1.Text += "토큰 취득 성공: " + signtoken[0] + "\nUID: "+signtoken[1]+"\n";
                                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                richTextBox1.ScrollToCaret();
                                try
                                {
                                    
                                    richTextBox1.Text += "\"" + ReqURLs.getuserinfo_kr + "\"에 접속 중...\n";
                                    richTextBox1.Text += "유저 정보 취득 중...\n";
                                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                    richTextBox1.ScrollToCaret();
                                    userjson = server.GetUserInfotoJson(signtoken[1], signtoken[0], sevtime);
                                    //richTextBox1.Text += userjson;
                                    if(userjson.Length < 10)
                                    {
                                        throw new Exception("올바르지 않은 json 파일입니다.");
                                    }
                                    string filepath = Application.StartupPath + "\\" + signtoken[1] + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath))
                                    {
                                        file.Write(userjson);
                                        file.Close();
                                    }
                                    richTextBox1.Text += "유저 정보 취득 성공. \n파일명 "+filepath+" 으로 저장했습니다.\n";
                                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                    richTextBox1.ScrollToCaret();
                                    richTextBox1.Text += "창이 3초 뒤에 자동으로 닫힙니다.\n";
                                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                    richTextBox1.ScrollToCaret();                                    
                                    server.Delay(3000);
                                    form2SendEvent(filepath);
                                    this.Close();
                                }
                                catch (Exception ex)
                                {
                                    richTextBox1.Text += "토큰 정보 취득에 실패했습니다.\n에러 내용: " + ex.ToString() + "\n";
                                    richTextBox1.Text += "서버 응답: " + userjson + "\n";
                                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                    richTextBox1.ScrollToCaret();
                                }
                            }
                            catch (Exception ex)
                            {
                                richTextBox1.Text += "토큰 정보 취득에 실패했습니다.\n에러 내용: "+ex.ToString()+"\n";
                                richTextBox1.Text += "서버 응답: " + encsigntoken + "\n";
                               richTextBox1.SelectionStart = richTextBox1.Text.Length;
                                richTextBox1.ScrollToCaret();
                            }
                        }
                        catch(Exception ex)
                        {
                            richTextBox1.Text += "sid 정보 취득 실패\n";
                            richTextBox1.Text += "서버 메세지: " + System.Text.RegularExpressions.Regex.Unescape(@json["msg"].ToString()) + "\n";
                            richTextBox1.Text += "오류 내용: " + ex.ToString() + "\n";
                            richTextBox1.SelectionStart = richTextBox1.Text.Length;
                            richTextBox1.ScrollToCaret();
                        }
                        

                    } 
                    catch (Exception ex)
                    {
                        richTextBox1.Text += "로그인 시도 중 오류가 발생했습니다.\n";                       
                        richTextBox1.Text += "서버 응답: "+result+"\n";
                        richTextBox1.Text += "오류 내용: " +ex.ToString()+"\n";
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.ScrollToCaret();                        
                    }
                    
                    
                }
                else
                {
                    MessageBox.Show("이메일 주소가 올바르지 않습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }           
           
            else
            {
                MessageBox.Show("이메일 주소를 입력하세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
