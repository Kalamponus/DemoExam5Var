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

        public void Filters(string materialType)
        {
            materials = DBConnection.materialEntities.Material.Where(x => x.MaterialType.Title == materialType).ToList(); 
            switch (cbSort.SelectedIndex)
            {
                case 0:
                    materials.OrderBy(x => x.)
            }
            
            materials = 
            lbMaterials.Items.Refresh();
        }

    }
}
