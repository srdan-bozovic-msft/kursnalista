using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MSC.Universal.Shared.UI.Controls
{
    public abstract class DataTemplateSelector : ContentControl
    {
        public virtual DataTemplate SelectTemplate(object newContent, DependencyObject container)
        {
            return null;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }
}