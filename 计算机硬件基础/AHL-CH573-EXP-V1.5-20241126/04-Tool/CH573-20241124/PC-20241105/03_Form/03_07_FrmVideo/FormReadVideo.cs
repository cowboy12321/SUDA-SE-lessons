using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Light;
using System.IO;

namespace AHL_GEC
{
    public partial class FormReadVideo : Form
    {
        public FormReadVideo()
        {
            InitializeComponent();
        }

        private void btnReadArm_Click(object sender, EventArgs e)
        {
            DataProcess dataProcess = new DataProcess();
            int softAdr;

            OpenFileDialog dialog = new OpenFileDialog();
            //FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Title = "请选择文件路径";
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string armFilePath = dialog.FileName.Trim();



            Console.WriteLine(armFilePath);
            if (armFilePath.Substring(armFilePath.Length - 4) == ".arm")
            {
                //如果文件正确，创建目录，并给用户提示

                //获取工程的文件夹目录
                DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);
                string currentDirPath = dir.Parent.Parent.FullName;
                currentDirPath += "\\LightData";
                //Console.WriteLine(currentDirPath);

                //在此目录下创建存放灯数据的文件夹
                string fileName = Path.GetFileNameWithoutExtension(armFilePath);//文件名

                this.textBoxArmName.Text = fileName + ".arm";
                this.textBoxCurrentState.Text = "读取文件成功";
                //string lightDir = "C:\\Users\\xishengxin\\Desktop\\论文\\led_light_ch573\\02-Software\\AHL-GEC-EXTERN-AUTO(V1.0)-20210413\\AHL-GEC-EXTERN-AUTO(V1.0)-20210413\\LightData" + "\\" + fileName;
                string lightDir = "E:\\01-Reserch\\视频文件" + "\\" + fileName;
                
                //创建文件夹，给某一个arm文件用，此文件夹下有128个txt文件，对应128个灯
                if (!Directory.Exists(lightDir))
                {

                    Directory.CreateDirectory(lightDir);
                }
                else
                {
                    this.textBoxCurrentState.Text = "文件夹已经存在，arm文件已生成对应数据";
                }

                //读arm文件，把所有灯的数据写入同一个文件a
                //读文件a，把各个灯的数据单独提取出来写入128个txt 

                //所有灯的数据都写入outputAllPath，然后从outputAllPath里面提取单独灯的数据
                string allDataPath = lightDir + "\\All.txt";
                dataProcess.readAll(armFilePath, allDataPath);

                for (softAdr = 1; softAdr <= 128; softAdr++)
                {
                    string outputLightPath = lightDir + "\\light" + softAdr.ToString() + ".txt";
                    dataProcess.extractData(softAdr, allDataPath, outputLightPath);
                }
                this.textBoxGenerateDataPath.Text = lightDir;
                this.textBoxCurrentState.Text += ",生成文件成功";

            }
            else
            {
                this.textBoxArmName.Text = "文件错误";
                Console.WriteLine("路径错误");
            }
        }
    }
}
