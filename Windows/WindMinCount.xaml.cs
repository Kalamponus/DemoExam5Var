using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DemoExam5Var.Classes;

namespace DemoExam5Var
{
    /// <summary>
    /// Логика взаимодействия для WindMinCount.xaml
    /// </summary>
    public partial class WindMinCount : Window
    {
        List<Material> materials = new List<Material>();
        public WindMinCount(List<int> items)
        {
            InitializeComponent();
            foreach (int id in items)
            {
                materials.Add(DBConnection.materialEntities.Material.Where(x => x.ID == id).FirstOrDefault());
            }
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(tbNewMinCount.Text.Length > 0)
            {
                foreach (Material material in materials)
                {
                    material.MinCount = Convert.ToDouble(tbNewMinCount.Text);
                }
                try
                {
                    DBConnection.materialEntities.SaveChanges();
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Не получилось изменить запись");
                }
            }
            else
            {
                MessageBox.Show("Введите значение");
            }            
        }
        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char let = e.Text[0];
            if (!Char.IsDigit(let) && let != ',')
            {
                e.Handled = true;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
