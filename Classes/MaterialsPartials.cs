using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoExam5Var.Classes;
namespace DemoExam5Var
{
    partial class Material
    {
        public string Suppliers
        {
            get
            {
                String materialSuppliers = "";
                
                List<Supplier> suppliers = DBConnection.materialEntities.Supplier.ToList();
                foreach(Supplier sup in suppliers)
                {
                    if (sup.Material.Contains(this))
                    {
                        materialSuppliers += sup.Title;
                        materialSuppliers += ", ";
                    }
                }
                if(materialSuppliers == "")
                {
                    materialSuppliers = "Нет";
                }
                return materialSuppliers;
            }
        }
        public string Img
        {
            get => Image == "" ? @"\materials\picture.png" : Image;
        }
    }
}
