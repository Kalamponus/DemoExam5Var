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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DemoExam5Var.Classes;

namespace DemoExam5Var
{
    /// <summary>
    /// Логика взаимодействия для ProductOutput.xaml
    /// </summary>
    public partial class PgMaterialOutput : Page
    {
        Pagination pagination = new Pagination();
        List<Material> materials;
        
        public PgMaterialOutput()
        {
            InitializeComponent();         
            cbFilter.Items.Add("Все типы");
            foreach(var type in DBConnection.materialEntities.MaterialType)
            {
                cbFilter.Items.Add(type.Title);
            }
            cbFilter.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;
            DataContext = pagination;
            DisplayOnPage();
        }

        public void cbChanged(object sender, SelectionChangedEventArgs e)
        {
            Filters();
            lbMaterials.Items.Refresh();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filters();
            lbMaterials.Items.Refresh();
        }
        public void Filters()
        {
            
            if (tbSearch.Text.Length > 0)
            {
                materials = DBConnection.materialEntities.Material.Where(x => x.Title.Contains(tbSearch.Text)).ToList();
            }

            if (cbFilter.SelectedIndex != 0)
            {
                materials = DBConnection.materialEntities.Material.Where(x => x.MaterialType.Title == cbFilter.SelectedItem.ToString()).ToList();
            }
            else
            {
                materials = DBConnection.materialEntities.Material.Where(x => x.Title.Contains(tbSearch.Text)).ToList();
            }
            
            switch (cbSort.SelectedIndex)
            {
                case 1:
                    materials = materials.OrderBy(x => x.Title).ToList();
                    break;
                case 2:
                    materials = materials.OrderByDescending(x => x.Title).ToList();
                    break;
                case 3:
                    materials = materials.OrderBy(x => x.CountInStock).ToList();
                    break;
                case 4:
                    materials = materials.OrderByDescending(x => x.CountInStock).ToList();
                    break;
                case 5:
                    materials = materials.OrderBy(x => x.Cost).ToList();
                    break;
                case 6:
                    materials = materials.OrderByDescending(x => x.Cost).ToList();
                    break;
                default:
                    break;
            }
            DisplayOnPage();            
        }

        private void lbMaterials_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(lbMaterials.SelectedIndex != -1)
            {
                Material item = lbMaterials.SelectedItem as Material;
                
                WindMaterialRedact redact = new WindMaterialRedact(item.ID);
                redact.ShowDialog();
                lbMaterials.SelectedIndex = -1;
                lbMaterials.Items.Refresh();
            }
            
        }

        private void btnAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            WindMaterialAdd windMaterialAdd = new WindMaterialAdd();
            windMaterialAdd.ShowDialog();
            lbMaterials.Items.Refresh();
        }

        private void btnMinCount_Click(object sender, RoutedEventArgs e)
        {

            List<int> itemsID = new List<int>();
            foreach (Material item in lbMaterials.SelectedItems)
            {
                itemsID.Add(item.ID);
            }

            WindMinCount windMinCount = new WindMinCount(itemsID);
            windMinCount.ShowDialog();
            lbMaterials.SelectedIndex = -1;
            lbMaterials.Items.Refresh();
        }

        private void lbMaterials_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbMaterials.SelectedItems.Count >= 2)
            {
                btnMinCount.Visibility = Visibility.Visible;
            }
            else
            {
                btnMinCount.Visibility = Visibility.Hidden;
            }
        }
        private void DisplayOnPage()
        {
            pagination.CountPage = 15;
            pagination.Countlist = materials.Count;
            lbMaterials.ItemsSource = materials.Skip(pagination.CurrentPage * pagination.CountPage - pagination.CountPage).Take(pagination.CountPage).ToList();
            tblOnPage.Text = "Выведено: " + lbMaterials.Items.Count + " из " + materials.Count();
        }
        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            switch (tb.Uid)
            {
                case "prev":                    
                    pagination.CurrentPage--;
                    break;
                case "next":
                    pagination.CurrentPage++;                    
                    break;
                default:
                    pagination.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            Filters();
            DisplayOnPage();
        }


    }
}
