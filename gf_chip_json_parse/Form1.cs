using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Json;

namespace gf_chip_json_parse
{
    public partial class Form1 : Form
    {
        string[] grid_id = {"1","2" ,"3I","3L","4I","4O","4Lm","4L","4Zm","4Z","4T",
            "5Pm","5P","5I","5C","5Z","5Zm","5V","5L","5Lm","5W","5Nm","5N","5Ym","5Y",
            "5X","5T","5F","5Fm","6O","6A","6D","6Z","6Zm","6Y","6T","6I","6C","6R"};
        int[] rotation_value = { 0, 0, 0, 0, 1, 0, 9, 0, 0, 0, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 2, 3, 1, 0, 3, 3, 0, 0, 2, 1, 1, 3, 3 };
        string[] squad_name = {"BGM-71","AGS-30","2B14","M2","AT4" };
        
        string jsonfilepath = "";
        string username= "";
        string gem = "";
        string uid = "";
        string exp = "";
        string level = "";
        int star_5 = 0;
        int star_4 = 0;
        int star_3 = 0;
        int star_2 = 0;
        int chipcount = 0;


        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "gfci file(*.gfci)|*.gfci|user extension|*.*";
            saveFileDialog1.Title = "파일 저장";
            saveFileDialog1.FileName = username + "_" + uid + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            saveFileDialog1.ShowDialog();                                 
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            load_json(openFileDialog1.FileName);

        }
        private void load_json(string filepath)
        {
            if (openFileDialog1.FileName != "")
            {
                jsonfilepath = filepath;
                textBox1.Text = jsonfilepath;
                string rawjson = System.IO.File.ReadAllText(@jsonfilepath);
                JObject o = JObject.Parse(rawjson);
                chipcount = o["chip_with_user_info"].Count();
                username = o["user_info"]["name"].ToString();
                gem = o["user_info"]["gem"].ToString();
                uid = o["user_info"]["user_id"].ToString();
                exp = o["user_info"]["experience"].ToString();
                level = o["user_info"]["lv"].ToString();
                label1.Text = "지휘관 이름: " + username + "\n지휘관 레벨: " + level + "\nUID: " + uid + "\n총 칩셋 개수: " + chipcount.ToString("D4");
                button1.Enabled = true;
            }
        }
        private void fileload_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog1.ShowDialog();
            
        }
        
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string savefilepath = "";
            if (saveFileDialog1.FileName != "")
            {
                savefilepath = saveFileDialog1.FileName;
                string[] squad_id = new string[squad_name.Length];
                string[] squad_id_temp = new string[squad_name.Length];
                string rawjson = System.IO.File.ReadAllText(@jsonfilepath);
                JObject o = JObject.Parse(rawjson);
                int chipcount = o["chip_with_user_info"].Count();
                string[] chip_id = new string[chipcount];
                int[] chip_id_int = new int[chipcount];
                int i = 0;
                int a = 0;
                foreach (JToken token in o["chip_with_user_info"])
                {
                    chip_id[i] = ((JProperty)token).Name.ToString();
                    i++;
                }
                foreach (JToken token in o["squad_with_user_info"])
                {
                    squad_id_temp[a] = ((JProperty)token).Name.ToString();
                    a++;
                }
                for(int p=0; p< o["squad_with_user_info"].Count(); p++ )
                {
                    string temp_id = o["squad_with_user_info"][squad_id_temp[p]]["squad_id"].ToString();
                    if ( temp_id == "1")
                    {
                        squad_id[0] = squad_id_temp[p];
                    }
                    else if (temp_id == "2")
                    {
                        squad_id[1] = squad_id_temp[p];
                    }
                    else if (temp_id == "3")
                    {
                        squad_id[2] = squad_id_temp[p];
                    }
                    else if (temp_id == "4")
                    {
                        squad_id[3] = squad_id_temp[p];
                    }
                    else if (temp_id == "5")
                    {
                        squad_id[4] = squad_id_temp[p];
                    }

                }
                if (checkBox1.Checked)
                {
                    for (int k = 0; k < chipcount; k++)
                    {
                        chip_id_int[k] = int.Parse(chip_id[k]);
                    }
                    Array.Sort(chip_id_int);
                    Array.Reverse(chip_id_int);
                    for (int k = 0; k < chipcount; k++)
                    {
                        chip_id[k] = chip_id_int[k].ToString();
                    }
                }
               
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@savefilepath))
                {
                    file.WriteLine("4.0.0");
                    for (int p = 0; p < chipcount; p++)
                    {
                        int id = int.Parse(o["chip_with_user_info"][chip_id[p]]["id"].ToString());
                        string grid = grid_id[int.Parse(o["chip_with_user_info"][chip_id[p]]["grid_id"].ToString()) - 1];
                        string damage = o["chip_with_user_info"][chip_id[p]]["assist_damage"].ToString();
                        string def_break = o["chip_with_user_info"][chip_id[p]]["assist_def_break"].ToString();
                        string hit = o["chip_with_user_info"][chip_id[p]]["assist_hit"].ToString();
                        string reload = o["chip_with_user_info"][chip_id[p]]["assist_reload"].ToString();
                        string[] ingame_rotate_info = Convert.ToString(o["chip_with_user_info"][chip_id[p]]["shape_info"]).Split(',');
                        int sim_rotate = int.Parse(ingame_rotate_info[0]) + rotation_value[int.Parse(o["chip_with_user_info"][chip_id[p]]["grid_id"].ToString()) - 1];
                        if(sim_rotate == 4) { 
                                sim_rotate = 0;
                        }
                        string tag = "0";
                        string rotation = sim_rotate.ToString();
                        string marking = "0";
                        string star = o["chip_with_user_info"][chip_id[p]]["chip_id"].ToString().Substring(0, 1);
                        string level = o["chip_with_user_info"][chip_id[p]]["chip_level"].ToString();
                        string color = Convert.ToString(int.Parse(o["chip_with_user_info"][chip_id[p]]["color_id"].ToString()) - 1);
                        if (checkBox2.Checked)
                        {
                            tag = o["chip_with_user_info"][chip_id[p]]["squad_with_user_id"].ToString();
                            //for(int w=0; w < squad_id.Length; w++)
                            {
                                if(tag == squad_id[0])
                                {
                                    tag = squad_name[0];                                   
                                }
                                else if (tag == squad_id[1])
                                {
                                    tag = squad_name[1];
                                }
                                else if (tag == squad_id[2])
                                {
                                    tag = squad_name[2];
                                }
                                else if (tag == squad_id[3])
                                {
                                    tag = squad_name[3];
                                }
                                else if (tag == squad_id[4])
                                {
                                    tag = squad_name[4];
                                }                              
                                else
                                {
                                    tag = "0";                           
                                }
                            }                                                      
                        }
                                                
                        if (tag != "0")
                        {
                            marking = "1";
                            file.WriteLine("00000000-0000-0000-0000-" + id.ToString("D12") + ";" + grid + ";" + damage + ";" + def_break + ";" + hit
                                + ";" + reload + ";" + rotation + ";" + marking + ";" + star + ";" + level + ";" + color + ";" + "000000"+tag);
                        }
                        else
                        {
                            file.WriteLine("00000000-0000-0000-0000-" + id.ToString("D12") + ";" + grid + ";" + damage + ";" + def_break + ";" + hit
                                + ";" + reload + ";" + rotation + ";" + marking + ";" + star + ";" + level + ";" + color + ";");
                        }
                    }
                    MessageBox.Show("파일명 " + System.IO.Path.GetFileName(@savefilepath) + " (으)로 저장했습니다.", "파일 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = checkBox1.Checked; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("소녀전선 서버에 직접 로그인하여 유저 정보를 불러옵니다. 이 기능을 사용함에 따라 발생하는 모든 사적/법적 불이익에 대한 책임은 모두 사용자 본인에게 있습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form2 frm = new Form2();
            frm.form2SendEvent += new FormSendDataHandler(receiveFormEvent);
            frm.Show();
        }
        public void receiveFormEvent(Object objc)
        {
            load_json(objc.ToString());
        }
    }
}
