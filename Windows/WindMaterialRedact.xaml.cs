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
using Microsoft.Win32;

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
            cbMaterialType.ItemsSource = DBConnection.materialEntities.MaterialType.ToList();
            cbMaterialType.DisplayMemberPath = "Title";
            cbMaterialType.SelectedItem = material.MaterialType;
            cbSuppliers.ItemsSource = DBConnection.materialEntities.Supplier.ToList();
            cbSuppliers.DisplayMemberPath = "Title";

            lbSuppliers.ItemsSource = material.Supplier.ToList();

            tbTitle.Text = material.Title;
            tbCountInPack.Text = material.CountInPack.ToString();
            tbCountInStock.Text = material.CountInStock.ToString();
            tbUnit.Text = material.Unit.ToString();
            tbMinCount.Text = material.MinCount.ToString();
            tbDescription.Text = material.Description.ToString();
            tbCost.Text = material.Cost.ToString();
            tbImage.Text = material.Image.ToString();
            imgMaterial.Source = new BitmapImage(new Uri(tbImage.Text, UriKind.Relative));
        }
        private bool tbValidation()
        {
            return tbCost.Text.Length > 0 && tbCountInPack.Text.Length > 0 && tbCountInStock.Text.Length > 0 && tbDescription.Text.Length > 0 && tbMinCount.Text.Length > 0 && tbTitle.Text.Length > 0 && tbUnit.Text.Length > 0;
        }
        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (tbValidation())
            {
                try
                {
                    material.Title = tbTitle.Text;
                    material.MaterialTypeID = cbMaterialType.SelectedIndex + 1;
                    material.CountInPack = Convert.ToInt32(tbCountInPack.Text);
                    material.CountInStock = Convert.ToDouble(tbCountInStock.Text);
                    material.MinCount = Convert.ToDouble(tbMinCount.Text);
                    material.Description = tbDescription.Text;
                    material.Cost = Convert.ToDecimal(tbCost.Text);
                    material.Image = tbImage.Text;
                }
                catch
                {
                    MessageBox.Show("Поля некорректно заполнены");
                }
                if (lbSuppliers.Items.Count > 0)
                {
                    foreach (Supplier supplier in lbSuppliers.Items)
                    {
                        if (!material.Supplier.Contains(supplier))
                        {
                            material.Supplier.Add((Supplier)supplier);
                        }
                    }
                }
                try
                {
                    DBConnection.materialEntities.SaveChanges();
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Не получилось добавить запись");
                }
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
        private void btnAddImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg";
            openFileDialog.Filter = "Изображения |*.jpg;*.png";
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                tbImage.Text = openFileDialog.FileName;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rsltMessageBox = MessageBox.Show("Вы уверены, что хотите удалить данный материал?", "Удаление материала", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (rsltMessageBox == MessageBoxResult.Yes)
                try
                {                   
                    DBConnection.materialEntities.Material.Remove(material);                    
                    DBConnection.materialEntities.SaveChanges();
                    MessageBox.Show("Материал был успешно удален!");
                    Close();
                }
                catch
                {
                    MessageBox.Show("Не удалось удалить материал!");
                }
        }

        private void btnSupAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbSuppliers.SelectedIndex != -1 && !material.Supplier.Contains(cbSuppliers.SelectedItem as Supplier))
            {
                material.Supplier.Add(cbSuppliers.SelectedItem as Supplier);
                lbSuppliers.ItemsSource = material.Supplier.ToList();
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
            material.Supplier.Remove(supplier);
            lbSuppliers.ItemsSource = material.Supplier.ToList();
            
        }

        private void tbImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            imgMaterial.Source = new BitmapImage(new Uri(tbImage.Text, UriKind.Relative));
        }
    }
}
