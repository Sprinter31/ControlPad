using System.Windows;

namespace ControlPad.Windows
{
    /// <summary>
    /// Interaction logic for SelectCategoryPopup.xaml
    /// </summary>
    public partial class SelectCategoryPopup : Window
    {
        public SelectCategoryPopup()
        {
            InitializeComponent();

            cb_Categories.ItemsSource = DataHandler.Categories;
        }
        public int SliderNr { get; set; }

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Categories.SelectedItem is Category selectedCategory)
            {
                var existing = DataHandler.SliderAssignments.FirstOrDefault(a => a.SliderNr == this.SliderNr);

                if (existing != null)
                {
                    existing.CategoryId = selectedCategory.Id;
                }
                else
                {
                    var assignment = new SliderAssignments
                    {
                        SliderNr = this.SliderNr,
                        CategoryId = selectedCategory.Id
                    };

                    DataHandler.SliderAssignments.Add(assignment);
                }
                DataHandler.SaveDataToFile(DataHandler.SliderAssignmentPath, DataHandler.SliderAssignments);

                this.Close();
            }
        }


        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 
    }
}
