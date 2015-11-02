using System;
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
                    case "AdViewModel":
                        contentTypeName = "AdViewModelTemplate";
                        break;
                    case "SubCategoryListViewModel":
                        contentTypeName = "SubCategoryListViewModelTemplate";
                        break;
                }

                if (Resources.ContainsKey(contentTypeName))
                {
                    return Resources[contentTypeName] as DataTemplate;
                }
                
                return Application.Current.Resources[contentTypeName] as DataTemplate;
            }

            return null;
        }
    }

    
}