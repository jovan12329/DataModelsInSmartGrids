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
        private ModelResourcesDesc descM = new ModelResourcesDesc();
        private IList<string> Gids { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            Gids = client.ServerGids();
            gidBox.ItemsSource = Gids;
            relatedGidBox.ItemsSource = Gids;
            modelBox.ItemsSource = client.GetConcreteModelCodes();
            DataContext = this;
        }

        private void gidBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var num = (string)gidBox.SelectedItem;


            num = num.Remove(0, 2);

            var gid = Int64.Parse(num, System.Globalization.NumberStyles.HexNumber);

            modelCodeBox.ItemsSource = client.GetModelCodesForSelectedGid(gid);




        }

        private void modelBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var num = (ModelCode)modelBox.SelectedItem;

            modelCodePropBox.ItemsSource = client.GetConcreteMCProperties(num);



        }

        private void relatedGidBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var num = (string)relatedGidBox.SelectedItem;

            num = num.Remove(0, 2);

            var gid = Int64.Parse(num, System.Globalization.NumberStyles.HexNumber);


            List<ModelCode> mdcs = client.GetRelatedDMSForRelatedQuery(gid).ToList();


            comboRelated.ItemsSource= mdcs;
            comboRelated.SelectedIndex = -1;
            propIdsRelated.ItemsSource = null;
                
            


        }

        private void comboRelated_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var modelReference = comboRelated.SelectedItem;

            if (modelReference == null) 
            {

                return;
            
            }

            var modelReference1 = (ModelCode)modelReference;

            switch (modelReference1) 
            {

                case ModelCode.REGULATINGCONDEQ_REGCONTROL:

                    propIdsRelated.ItemsSource=descM.GetAllPropertyIds(ModelCode.REGULATINGCONTROL);

                    break;

                case ModelCode.REGULATINGCONTROL_REGCONDEQS:

                    propIdsRelated.ItemsSource = descM.GetAllPropertyIds(ModelCode.REGULATINGCONDEQ);

                    break;
                case ModelCode.REGULATINGCONTROL_TERMINAL:

                    propIdsRelated.ItemsSource = descM.GetAllPropertyIds(ModelCode.TERMINAL);

                    break;
                case ModelCode.REGULATINGCONTROL_REGSCHEDULERS:

                    propIdsRelated.ItemsSource = descM.GetAllPropertyIds(ModelCode.REGULATIONSCHEDULE);

                    break;
                case ModelCode.TERMINAL_REGCONTROLS:

                    propIdsRelated.ItemsSource = descM.GetAllPropertyIds(ModelCode.REGULATINGCONTROL);

                    break;
                case ModelCode.DAYTYPE_SEASONDAYSCHEDS:

                    propIdsRelated.ItemsSource = descM.GetAllPropertyIds(ModelCode.SEASONDAYTYPESCHEDULE);

                    break;

                case ModelCode.SEASON_SEASONDAYSCHEDS:

                    propIdsRelated.ItemsSource = descM.GetAllPropertyIds(ModelCode.SEASONDAYTYPESCHEDULE);

                    break;
                case ModelCode.SEASONDAYTYPESCHEDULE_SEASON:

                    propIdsRelated.ItemsSource = descM.GetAllPropertyIds(ModelCode.SEASON);

                    break;

                case ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE:

                    propIdsRelated.ItemsSource = descM.GetAllPropertyIds(ModelCode.DAYTYPE);

                    break;
                case ModelCode.REGULATIONSCHEDULE_REGCONTROL:

                    propIdsRelated.ItemsSource = descM.GetAllPropertyIds(ModelCode.REGULATINGCONTROL);

                    break;

                default:

                    MessageBox.Show("Noting to show!");
                    break;

            }





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
                MessageBox.Show("Please choose Model Code !");
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

        private void GetRelatedValues_Click(object sender, RoutedEventArgs e)
        {

            var selectedGid = relatedGidBox.SelectedItem;
            var selectedRef = comboRelated.SelectedItem;
            var selectedProps = propIdsRelated.SelectedItems;


            if (selectedGid == null)
            {
                MessageBox.Show("Please choose GID !");
                return;
            }

            if (selectedRef == null) 
            {

                MessageBox.Show("Please choose reference!");
                return;
            
            }

            if (selectedProps == null || selectedProps.Count == 0)
            {
                MessageBox.Show("Please choose properties !");
                return;
            }

            string gid = (string)selectedGid;
            ModelCode selectedref1 = (ModelCode)selectedRef;
            var myGid= gid.Remove(0, 2);


            var gid1 = Int64.Parse(myGid, System.Globalization.NumberStyles.HexNumber);

            


            IList<ModelCode> props = CastModelCode(selectedProps);

            List<ResourceDescription> chosenRsource = null;
            Association a = new Association();
            a.Type = 0;
            a.PropertyId = selectedref1;

            

            client.MyGetRelatedValues(gid1,props.ToList(), a, out chosenRsource);

            if (chosenRsource.Count == 0)
            {

                displayRelated.Text = "No resources found!";
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var d in chosenRsource)
            {

                sb.AppendLine(DisplayResource(d)).AppendLine();

            }

            displayRelated.Text = sb.ToString();





        }


        #region Utilities


       
        private string DisplayResource(ResourceDescription rd) {

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





        private IList<ModelCode> CastModelCode(object toCast) {

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
