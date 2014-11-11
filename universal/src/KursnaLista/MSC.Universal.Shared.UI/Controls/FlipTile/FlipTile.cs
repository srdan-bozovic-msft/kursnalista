// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MSC.Universal.Shared.UI.Controls
{
    /// <summary>
    /// Represents an animated tile that supports an image and a title.
    /// Furthermore, it can also be associated with a message or a notification.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [TemplateVisualState(Name = Expanded, GroupName = TileStates)]
    [TemplateVisualState(Name = Flipped, GroupName = TileStates)]
    public class FlipTile : Control
    {
        /// <summary>
        /// Common visual states.
        /// </summary>
        private const string TileStates = "TileStates";

        /// <summary>
        /// Expanded visual state.
        /// </summary>
        private const string Expanded = "Expanded";

        /// <summary>
        /// Flipped visual state.
        /// </summary>
        private const string Flipped = "Flipped";

        /// <summary>
        /// Represents the number of steps inside the pipeline of stalled images
        /// </summary>
        internal int _stallingCounter;

        /// <summary>
        /// Flag that determines if the flip tile has a secondary text string associated to it.
        /// If it does not, the flip tile will not flip.
        /// </summary>
        internal bool _canFlip = true;

        #region Front DependencyProperty

        /// <summary>
        /// Identifies the Front DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty FrontProperty =
            DependencyProperty.Register("Front", typeof(object), typeof(FlipTile), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the front.
        /// </summary>
        public object Front
        {
            get { return GetValue(FrontProperty); }
            set { SetValue(FrontProperty, value); }
        }

        #endregion

        #region FrontTemplate DependencyProperty

        /// <summary>
        /// Identifies the FrontTemplate DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty FrontTemplateProperty =
            DependencyProperty.Register("FrontTemplate", typeof(DataTemplate), typeof(FlipTile), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the template used to display the control's front side.
        /// </summary>
        public DataTemplate FrontTemplate
        {
            get { return (DataTemplate)GetValue(FrontTemplateProperty); }
            set { SetValue(FrontTemplateProperty, value); }
        }

        #endregion


        #region Back DependencyProperty

        /// <summary>
        /// Identifies the Back DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty BackProperty =
            DependencyProperty.Register("Back", typeof(object), typeof(FlipTile), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the back.
        /// </summary>
        public object Back
        {
            get { return GetValue(BackProperty); }
            set { SetValue(BackProperty, value); }
        }

        #endregion

        #region BackTemplate DependencyProperty

        /// <summary>
        /// Identifies the BackTemplate DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty BackTemplateProperty =
            DependencyProperty.Register("BackTemplate", typeof(DataTemplate), typeof(FlipTile), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the template used to display the control's back side.
        /// </summary>
        public DataTemplate BackTemplate
        {
            get { return (DataTemplate)GetValue(BackTemplateProperty); }
            set { SetValue(BackTemplateProperty, value); }
        }

        #endregion

        #region IsFrozen DependencyProperty

        /// <summary>
        /// Gets or sets the flag for images that do not animate.
        /// </summary>
        public bool IsFrozen
        {
            get { return (bool)GetValue(IsFrozenProperty); }
            set { SetValue(IsFrozenProperty, value); }
        }

        /// <summary>
        /// Identifies the IsFrozen dependency property.
        /// </summary>
        public static readonly DependencyProperty IsFrozenProperty =
            DependencyProperty.Register("IsFrozen", typeof(bool), typeof(FlipTile), new PropertyMetadata(false, new PropertyChangedCallback(OnIsFrozenChanged)));

        /// <summary>
        /// Removes the frozen image from the enabled image pool or the stalled image pipeline.
        /// Adds the non-frozen image to the enabled image pool.  
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnIsFrozenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            FlipTile tile = (FlipTile)obj;

            if ((bool)e.NewValue)
            {
                FlipTileService.FreezeFlipTile(tile);
            }
            else
            {
                FlipTileService.UnfreezeFlipTile(tile);
            }
        }

        #endregion

        #region GroupTag DependencyProperty

        /// <summary>
        /// Gets or sets the group tag.
        /// </summary>
        public string GroupTag
        {
            get { return (string)GetValue(GroupTagProperty); }
            set { SetValue(GroupTagProperty, value); }
        }

        /// <summary>
        /// Identifies the GroupTag dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupTagProperty =
            DependencyProperty.Register("GroupTag", typeof(string), typeof(FlipTile), new PropertyMetadata(string.Empty));

        #endregion

        #region State DependencyProperty

        /// <summary>
        /// Gets or sets the visual state.
        /// </summary>
        internal FlipState State
        {
            get { return (FlipState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        /// <summary>
        /// Identifies the State dependency property.
        /// </summary>
        private static readonly DependencyProperty StateProperty =
                DependencyProperty.Register("State", typeof(FlipState), typeof(FlipTile), new PropertyMetadata(FlipState.Expanded, OnTileStateChanged));

        /// <summary>
        /// Triggers the transition between visual states.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnTileStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((FlipTile)obj).UpdateVisualState();
        }

        #endregion

        #region Size DependencyProperty

        /// <summary>
        /// Gets or sets the visual state.
        /// </summary>
        public TileSize Size
        {
            get { return (TileSize)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        /// <summary>
        /// Identifies the State dependency property.
        /// </summary>
        public static readonly DependencyProperty SizeProperty =
                DependencyProperty.Register("Size", typeof(TileSize), typeof(FlipTile), new PropertyMetadata(TileSize.Default, OnSizeChanged));

        /// <summary>
        /// Triggers the transition between visual states.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnSizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            FlipTile flipTile = (FlipTile)obj;

            // And now we'll update the width and height to match the new size.
            switch (flipTile.Size)
            {
                case TileSize.Default:
                    flipTile.Width = 173;
                    flipTile.Height = 173;
                    break;

                case TileSize.Small:
                    flipTile.Width = 99;
                    flipTile.Height = 99;
                    break;

                case TileSize.Medium:
                    flipTile.Width = 210;
                    flipTile.Height = 210;
                    break;

                case TileSize.Large:
                    flipTile.Width = 432;
                    flipTile.Height = 210;
                    break;
            }

            flipTile.SizeChanged += OnFlipTileSizeChanged;
            FlipTileService.FinalizeReference(flipTile);
        }

        static void OnFlipTileSizeChanged(object sender, SizeChangedEventArgs e)
        {
            FlipTile flipTile = (FlipTile)sender;
            flipTile.SizeChanged -= OnFlipTileSizeChanged;

            // In order to avoid getting into a bad state, we'll shift the FlipTile
            // back to the Expanded state.  If we were already in the Expanded state,
            // then we'll manually shift the title panel to the right location,
            // since the visual state manager won't do it for us in that case.
            if (flipTile.State != FlipState.Expanded)
            {
                flipTile.State = FlipState.Expanded;
                VisualStateManager.GoToState(flipTile, Expanded, false);
            }

            FlipTileService.InitializeReference(flipTile);
        }

        #endregion

        /// <summary>
        /// Updates the visual state.
        /// </summary>
        private void UpdateVisualState()
        {
            string state;

            // If we're in the Small size, then we should just display the image
            // instead of having animations.
            if (Size != TileSize.Small)
            {
                switch (State)
                {
                    case FlipState.Expanded:
                        state = Expanded;
                        break;
                    case FlipState.Flipped:
                        state = Flipped;
                        break;
                    default:
                        state = Expanded;
                        break;
                }
            }
            else
            {
                state = Expanded;
            }
            
            VisualStateManager.GoToState(this, state, true);
        }

        /// <summary>
        /// Gets the template parts and sets binding.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateVisualState();            
        }

        /// <summary>
        /// Initializes a new instance of the FlipTile class.
        /// </summary>
        public FlipTile()
        {
            DefaultStyleKey = typeof(FlipTile);
            Loaded += FlipTile_Loaded;
            Unloaded += FlipTile_Unloaded;
        }

        /// <summary>
        /// This event handler gets called as soon as a flip tile is added to the visual tree.
        /// A reference of this flip tile is passed on to the service singleton.
        /// </summary>
        /// <param name="sender">The flip tile.</param>
        /// <param name="e">The event information.</param>
        void FlipTile_Loaded(object sender, RoutedEventArgs e)
        {
            FlipTileService.InitializeReference(this);
        }

        /// <summary>
        /// This event handler gets called as soon as a flip tile is removed from the visual tree.
        /// Any existing reference of this flip tile is eliminated from the service singleton.
        /// </summary>
        /// <param name="sender">The flip tile.</param>
        /// <param name="e">The event information.</param>
        void FlipTile_Unloaded(object sender, RoutedEventArgs e)
        {
            FlipTileService.FinalizeReference(this);
        }        
    }

    /// <summary>
    /// Represents the visual states of a Flip tile.
    /// </summary>
    internal enum FlipState
    {
        /// <summary>
        /// Expanded visual state value.
        /// </summary>
        Expanded = 0,

        /// <summary>
        /// Flipped visual state value.
        /// </summary>
        Flipped = 1,
    };

    /// <summary>
    /// Represents the size of a Flip tile.
    /// </summary>
    public enum TileSize
    {
        /// <summary>
        /// Default size (173 px x 173 px).
        /// </summary>
        Default,

        /// <summary>
        /// Small size (99 px x 99 px).
        /// </summary>
        Small,

        /// <summary>
        /// Medium size (210 px x 210 px).
        /// </summary>
        Medium,

        /// <summary>
        /// Large size (432 px x 210 px).
        /// </summary>
        Large,
    };
}