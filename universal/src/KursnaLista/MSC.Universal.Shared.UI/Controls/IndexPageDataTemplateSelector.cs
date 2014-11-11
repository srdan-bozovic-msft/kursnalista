using Windows.UI.Xaml;

namespace MSC.Universal.Shared.UI.Controls
{
    public class IndexPageDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object newContent, DependencyObject container)
        {
            if (newContent != null)
            {
                string contentTypeName = string.Empty;
                switch (newContent.GetType().Name)
                {
                    case "IndexSekcijeViewModel":
                        contentTypeName = "IndexSectionMenuTemplate";
                        break;
                    case "VestiViewModel":
                        contentTypeName = "IndexPanoramaItemTemplate";
                        break;
                }

                var local = Resources[contentTypeName] as DataTemplate;
                return local??Application.Current.Resources[contentTypeName] as DataTemplate;
            }

            return null;
        }
    }
}