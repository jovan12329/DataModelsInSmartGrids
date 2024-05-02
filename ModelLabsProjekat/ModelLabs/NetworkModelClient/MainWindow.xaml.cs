using FTN.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Xml;

namespace NetworkModelClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientGda client = new ClientGda();
        private EnumDescs enumDescs = new EnumDescs();

        public MainWindow()
        {
            InitializeComponent();
            gidBox.ItemsSource = client.ServerGids();
            modelBox.ItemsSource = client.GetConcreteModelCodes();
            DataContext = this;
        }

        private void gidBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            
            var num=(string)gidBox.SelectedItem;


            num = num.Remove(0, 2);

            var gid=Int64.Parse(num, System.Globalization.NumberStyles.HexNumber);
            
            modelCodeBox.ItemsSource = client.GetModelCodesForSelectedGid(gid);

            


        }

        private void modelBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var num = (ModelCode)modelBox.SelectedItem;

            modelCodePropBox.ItemsSource = client.GetConcreteMCProperties(num);



        }



        private void GetValues_Click(object sender, RoutedEventArgs e)
        {

            

            var selectedGid = (string)gidBox.SelectedItem;


            var selectedProperties = modelCodeBox.SelectedItems;


            if (selectedGid == null)
            {
                MessageBox.Show("Please choose GID !");
                return;
            }

            if (selectedProperties == null || selectedProperties.Count==0) 
            {
                MessageBox.Show("Please choose properties !");
                return;
            }

            selectedGid = selectedGid.Remove(0, 2);

            var gid = Int64.Parse(selectedGid, System.Globalization.NumberStyles.HexNumber);

            IList<ModelCode> props = CastModelCode(selectedProperties);

            ResourceDescription rd=client.GetSelectedPropertiesForGid(gid,props);

            displayProps.Text = DisplayResource(rd);


        }


        private void GetExtentValues_Click(object sender, RoutedEventArgs e)
        {

            var selectedModelCode = modelBox.SelectedItem;
            var selectedProperties = modelCodePropBox.SelectedItems;


            if (selectedModelCode == null)
            {
                MessageBox.Show("Please choose GID !");
                return;
            }

            if (selectedProperties == null || selectedProperties.Count == 0)
            {
                MessageBox.Show("Please choose properties !");
                return;
            }


            ModelCode selectedModelCode1 = (ModelCode)modelBox.SelectedItem;


            IList<ModelCode> props = CastModelCode(selectedProperties);

            List<ResourceDescription> chosenRsource = null;

            client.MyGetExtentValues(selectedModelCode1,props.ToList(),out chosenRsource);

            if (chosenRsource.Count == 0) {

                displayExtent.Text = "No resources found!";
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var d in chosenRsource) { 
            
                sb.AppendLine(DisplayResource(d)).AppendLine();
            
            }

            displayExtent.Text=sb.ToString();
            






        }




        #region Utilities
        public string DisplayResource(ResourceDescription rd) {

            StringBuilder sb = new StringBuilder();

            string type = ((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)).ToString();
            string gid = "0x"+rd.Id.ToString("x16");
            sb.AppendLine($"{type} \t gid={gid}:\n\n\tProperties:\n");

            foreach (var p in rd.Properties) 
            {

                

                switch (p.Type) {

                    case PropertyType.Bool:
                        sb.AppendLine($"\t\t{p.Id.ToString()}:\n \t\t\t\t *{p.AsBool()}\n");
                        
                        break;
                    case PropertyType.DateTime:
                        sb.AppendLine($"\t\t{p.Id.ToString()}:\n \t\t\t\t *{p.AsDateTime()}\n");
                        
                        break;
                    case PropertyType.Enum:
                        sb.AppendLine($"\t\t{p.Id.ToString()}:\n \t\t\t\t *{enumDescs.GetStringFromEnum(p.Id,p.AsEnum())}\n");
                        
                        break;
                    case PropertyType.Float:
                        sb.AppendLine($"\t\t{p.Id.ToString()}:\n \t\t\t\t *{p.AsFloat()}\n");
                        
                        break;
                    case PropertyType.Int64:
                        sb.AppendLine($"\t\t{p.Id.ToString()}:\n \t\t\t\t *0x{p.AsLong().ToString("x16")}\n");
                        
                        break;
                    case PropertyType.Reference:
                        sb.AppendLine($"\t\t{p.Id.ToString()}:\n \t\t\t\t +0x{p.AsReference().ToString("x16")}\n");
                        
                        break;
                    case PropertyType.ReferenceVector:

                        if (p.AsLongs().Count > 0)
                        {
                            sb.Append($"\t\t{p.Id.ToString()}:\n");
                            var list = p.AsLongs();

                            foreach (var t in list)
                            {
                                sb.AppendLine($" \t\t\t\t +0x{t.ToString("x16")}");
                            }
                            sb.AppendLine();
                            
                        }
                        else
                        {
                            sb.AppendLine($"\t\t{p.Id.ToString()}:\n \t\t\t\t +empty long/reference vector\n");
                            
                        }
                        break;

                    default:
                        MessageBox.Show("Nothing to show !");
                        break;


                }
            
            
            }

            sb.AppendLine();


            return sb.ToString();



        
        }





        public IList<ModelCode> CastModelCode(object toCast) {

            List<ModelCode> mds = new List<ModelCode>();

            System.Collections.IList list = (System.Collections.IList)toCast;

            foreach (var ob in list) {

                mds.Add((ModelCode)ob);
            
            }

            return mds;
        
        
        
        }

        #endregion

    }
}
