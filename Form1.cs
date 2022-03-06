using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        List<string> imgList = new List<string>();
        int nImg = 0; // номер (в списке) отображаемой иллюстрации
        string aPath; // путь к файлам

        public Form1()
        {
            InitializeComponent();
            DirectoryInfo di; // каталог
                              // получить имя каталога "Мои рисунки"
            di = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.MyPictures));
            aPath = di.FullName;
            FillListBox(aPath);

        }
        private Boolean FillListBox(string aPath)
        {
            // информация о каталоге
            DirectoryInfo di = new DirectoryInfo(aPath);
            // информация о файлах
            FileInfo[] fi = di.GetFiles("*.jpg");
            // очистить список иллюстраций
            imgList.Clear();
            // добавляем в imgList имена jpg-файлов каталога aPath
            foreach (FileInfo fc in fi)
            {
                imgList.Add(fc.Name);
            }
            if (fi.Length == 0)
            {
                button2.Enabled = false;
                button3.Enabled = false;
                return false;
            }
            else
            {
                nImg = 0;
                ShowPicture(aPath + "\\" + imgList[nImg]);
                // сделать недоступной кнопку Назад
                button2.Enabled = false;
                // если в каталоге один jpg-файл, сделать недоступной кнопку Вперёд
                if (imgList.Count == 1)
                    button3.Enabled = false;
                else
                    button3.Enabled = true;
                return true;
            }

        }
        private void ShowPicture(string aPicture)
        {
            pictureBox1.Visible = false;
            // загружаем изображение в pictureBox1
            pictureBox1.Image = System.Drawing.Bitmap.FromFile(aPicture);
            // надо масштабировать?
            if ((pictureBox1.Image.Width > pictureBox1.Width) ||
            (pictureBox1.Image.Height > pictureBox1.Height))
            { // да
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else // нет, не надо масштабировать
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.Visible = true;
            this.Text = aPicture;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // если кнопка "Вперёд" недоступна, сделаем ее доступной
            if (!button3.Enabled)
                button3.Enabled = true;
            if (nImg > 0)
            {
                nImg--;
                ShowPicture(aPath + "\\" + imgList[nImg]);
                // отображается первая иллюстрация
                if (nImg == 0)
                { // теперь кнопка Назад недоступна
                    button2.Enabled = false;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!button2.Enabled)
                button2.Enabled = true;
            if (nImg < imgList.Count)
            {
                nImg++;
                ShowPicture(aPath + "\\" + imgList[nImg]);
                if (nImg == imgList.Count - 1)
                {
                    button3.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // FolderBrowserDialog - стандартное окно Обзор папок
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.Description = "Выберите папку в которой\nнаходятся иллюстрации";
            // кнопка Создать папку недоступна
            fb.ShowNewFolderButton = false;
            // "стартовая" папка
            fb.SelectedPath = aPath;
            // отображаем диалоговое окно
            if (fb.ShowDialog() == DialogResult.OK)
            { // пользователь выбрал каталог и щелкнул на кнопке ОК
                aPath = fb.SelectedPath;
                if (!FillListBox(fb.SelectedPath))
                    // в каталоге нет файлов иллюстраций
                    pictureBox1.Image = null;
            }

        }
    }
}
