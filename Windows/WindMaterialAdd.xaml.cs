using Microsoft.Win32;
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
    /// Логика взаимодействия для MaterialAdd.xaml
    /// </summary>
    public partial class WindMaterialAdd : Window
    {

        public WindMaterialAdd()
        {
            InitializeComponent();          
            cbSuppliers.ItemsSource = DBConnection.materialEntities.Supplier.ToList();
            cbSuppliers.DisplayMemberPath = "Title";
            cbMaterialType.ItemsSource = DBConnection.materialEntities.MaterialType.ToList();
            cbMaterialType.DisplayMemberPath = "Title";
        }
        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char let = e.Text[0];
            if (!Char.IsDigit(let) && let != ',')
            {
                e.Handled = true;
            }
        }
        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Material material = new Material();
            try
            {               
                material.Title = tbTitle.Text;
                material.MaterialTypeID = cbMaterialType.SelectedIndex + 1;
                material.CountInPack = Convert.ToInt32(tbCountInPack.Text);
                material.CountInStock = Convert.ToDouble(tbCountInStock.Text);
                material.MinCount = Convert.ToDouble(tbMinCount.Text);
                material.Description = tbDescription.Text;
                material.Cost = Convert.ToDecimal(tbCost.Text);
                material.Image = tbImg.Text;
                material.Unit = tbUnit.Text;
            }
            catch
            {
                MessageBox.Show("Поля некорректно заполнены");                
            }
            
            foreach (Supplier supplier in lbSuppliers.Items)
            {
                material.Supplier.Add((Supplier)supplier);
            }

            try
            {
                DBConnection.materialEntities.Material.Add(material);
                DBConnection.materialEntities.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Не получилось добавить запись");
            }
            
            
        }

        private void btnAddImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg";
            openFileDialog.Filter = "Изображения |*.jpg;*.png";
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                tbImg.Text = openFileDialog.FileName;
            }
        }

        private void btnSupAdd_Click(object sender, RoutedEventArgs e)
        {
            if(cbSuppliers.SelectedIndex != -1)
            {
                lbSuppliers.Items.Add(cbSuppliers.SelectedItem as Supplier);
                cbSuppliers.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Выберите поставщика");
            }
        }

        private void ButtonX_Click(object sender, RoutedEventArgs e)
        {
            Button BTN = sender as Button;
            int id = Convert.ToInt32(BTN.Tag);
            Supplier supplier = DBConnection.materialEntities.Supplier.Where(x => x.ID == id).FirstOrDefault();
            lbSuppliers.Items.Remove(supplier);
            lbSuppliers.Items.Refresh();
        }

        private void tbImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            imgMater.Source = new BitmapImage(new Uri(tbImg.Text, UriKind.Relative));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
