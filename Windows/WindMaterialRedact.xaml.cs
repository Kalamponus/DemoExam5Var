using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для WindMaterialRedact.xaml
    /// </summary>
    public partial class WindMaterialRedact : Window
    {
        Material material;
        public WindMaterialRedact(int tag)
        {
            InitializeComponent();
            material = DBConnection.materialEntities.Material.Where(x => x.ID == tag).FirstOrDefault();
            List<MaterialType> materialTypes = DBConnection.materialEntities.MaterialType.ToList();

            List<string> typeTitle = new List<string>();
            foreach (var type in materialTypes)
            {
                typeTitle.Add(type.Title);
            }

            cbMaterialType.ItemsSource = typeTitle;
            cbMaterialType.SelectedItem = material.MaterialType.Title;

            tbTitle.Text = material.Title;
            tbCountInPack.Text = material.CountInPack.ToString();
            tbCountInStock.Text = material.CountInStock.ToString();
            tbUnit.Text = material.Unit.ToString();
            tbMinCount.Text = material.MinCount.ToString();
            tbDescription.Text = material.Description.ToString();
            tbCost.Text = material.Cost.ToString();
            tbImage.Text = material.Image.ToString();
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            
            material.Title = tbTitle.Text;
            material.MaterialTypeID = cbMaterialType.SelectedIndex + 1;
            material.CountInPack = Convert.ToInt32(tbCountInPack.Text);
            material.CountInStock = Convert.ToInt32(tbCountInStock.Text);
            material.MinCount = Convert.ToInt32(tbMinCount.Text);
            material.Description = tbDescription.Text;
            material.Cost = Convert.ToDecimal(tbCost.Text);
            material.Image = tbImage.Text;

            DBConnection.materialEntities.SaveChanges();
            
        }

        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char let = e.Text[0];
            if (!Char.IsDigit(let) && let != ',')
            {
                e.Handled = true;
            }
        }
    }
}
