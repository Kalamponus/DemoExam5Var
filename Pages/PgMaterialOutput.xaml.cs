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
        List<Material> materials = DBConnection.materialEntities.Material.ToList();
        public PgMaterialOutput()
        {
            InitializeComponent();                       
            lbMaterials.ItemsSource = materials;

            cbFilter.Items.Add("Все типы");
            foreach(var type in DBConnection.materialEntities.MaterialType)
            {
                cbFilter.Items.Add(type.Title);
            }
            cbFilter.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;
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
                materials = materials.Where(x => x.MaterialType.Title == cbFilter.SelectedItem.ToString()).ToList();
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
            lbMaterials.ItemsSource = materials;
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
            windMaterialAdd.Show();
            lbMaterials.Items.Refresh();
        }
    }
}
