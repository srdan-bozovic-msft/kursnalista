using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using MSC.Universal.Shared.UI.Contracts.ViewModels;
using MSC.Universal.Shared.UI.Converters;

namespace MSC.Universal.Shared.UI.Controls
{
    public class PowerHub : Hub
    {
        public PowerHub()
        {

        }

        #region ItemTemplate Dependency Property

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(PowerHub), new PropertyMetadata(null, ItemTemplateChanged));

        private static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PowerHub hub = d as PowerHub;
            if (hub != null)
            {
                DataTemplate template = e.NewValue as DataTemplate;
                if (template != null)
                {
                    // Apply template
                    foreach (var section in hub.Sections)
                    {
                        section.ContentTemplate = template;
                    }
                }
            }
        }

        #endregion

        #region ItemsSource Dependency Property

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList), typeof(PowerHub), new PropertyMetadata(null, ItemsSourceChanged));

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PowerHub hub = d as PowerHub;
            if (hub != null)
            {
                IList items = e.NewValue as IList;
                if (items != null)
                {
                    hub.Sections.Clear();
                    for (int index = 0; index < items.Count; index++)
                    {
                        var item = items[index];
                        HubSection section = new HubSection();
                        if (index == 0)
                        {
                            // N1 only HACK !!! Very Lame!
                            //section.Margin = new Thickness(95, section.Margin.Top, section.Margin.Right,
                            //    section.Margin.Bottom);
                            section.Padding = new Thickness(115, 20, 20, 20);
                        }
                        section.DataContext = item;
                        section.Header = item;
                        DataTemplate template = hub.ItemTemplate;
                        section.ContentTemplate = template;
                        hub.Sections.Add(section);

                        IViewItem viewItem = item as IViewItem;
                        if (viewItem != null)
                        {
                            section.IsHeaderInteractive = viewItem.IsNavigable;
                            section.SetBinding(HubSection.VisibilityProperty, new Binding()
                            {
                                Converter = new BooleanToVisibilityConverter(),
                                Path = new PropertyPath("IsVisible"),
                                Mode = BindingMode.OneWay
                            });
                        }
                        
                    }
                }
            }
        }

        #endregion
    }
}
