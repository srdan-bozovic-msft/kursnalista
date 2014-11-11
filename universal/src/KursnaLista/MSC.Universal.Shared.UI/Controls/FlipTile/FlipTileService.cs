// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Windows.UI.Xaml;

namespace MSC.Universal.Shared.UI.Controls
{
    /// <summary>
    /// Provides organized animations for the flip tiles.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public static class FlipTileService
    {
        /// <summary>
        /// Number of steps in the pipeline
        /// </summary>
        private const int WaitingPipelineSteps = 3;

        /// <summary>
        /// Number of flip tile that can be animated at exactly the same time.
        /// </summary>
        private const int NumberOfSimultaneousAnimations = 1;

        /// <summary>
        /// Track resurrection for weak references.
        /// </summary>
        private const bool TrackResurrection = false;

        /// <summary>
        /// Timer to trigger animations in timely.
        /// </summary>        
        private static DispatcherTimer Timer = new DispatcherTimer();

        /// <summary>
        /// Random number generator to take certain random decisions.
        /// e.g. which flip tile is to be animated next.
        /// </summary>
        private static Random ProbabilisticBehaviorSelector = new Random();

        /// <summary>
        /// Pool that contains references to the flip tiles that are not frozen.
        /// i.e. flip tiles that can be animated at the moment.
        /// </summary>
        private static List<WeakReference> EnabledImagesPool = new List<WeakReference>();

        /// <summary>
        /// Pool that contains references to the flip tiles which are frozen.
        /// i.e. flip tiles that cannot be animated at the moment.
        /// </summary>
        private static List<WeakReference> FrozenImagesPool = new List<WeakReference>();

        /// <summary>
        /// Pipeline that contains references to the flip tiles that where animated previously.
        /// These are stalled briefly before they can be animated again.
        /// </summary>
        private static List<WeakReference> StalledImagesPipeline = new List<WeakReference>();

        /// <summary>
        /// Static constructor to add the tick event handler.
        /// </summary>        
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Attaching event handlers cannot be done inline.")]
        static FlipTileService()
        {
            Timer.Tick += OnTimerTick;
        }

        /// <summary>
        /// Restart the timer to trigger animations.
        /// </summary>
        private static void RestartTimer()
        {
            if (!Timer.IsEnabled)
            {        
                Timer.Interval = TimeSpan.FromMilliseconds(2500);
                Timer.Start();
            }
        }

        /// <summary>
        /// Add a reference to a newly instantiated flip tile.
        /// </summary>
        /// <param name="tile">The newly instantiated flip tile.</param>
        internal static void InitializeReference(FlipTile tile)
        {
            WeakReference wref = new WeakReference(tile, TrackResurrection);
            if (tile.IsFrozen)
            {
                AddReferenceToFrozenPool(wref);  
            }
            else
            {
                AddReferenceToEnabledPool(wref);  
            }

            RestartTimer();
        }

        /// <summary>
        /// Remove all references of a flip tile before finalizing it.
        /// </summary>
        /// <param name="tile">The flip tile that is to be finalized.</param>
        internal static void FinalizeReference(FlipTile tile)
        {
            WeakReference wref = new WeakReference(tile, TrackResurrection);
            FlipTileService.RemoveReferenceFromEnabledPool(wref);
            FlipTileService.RemoveReferenceFromFrozenPool(wref);
            FlipTileService.RemoveReferenceFromStalledPipeline(wref);
        }

        /// <summary>
        /// Add a reference of a flip tile to the enabled images pool.
        /// </summary>
        /// <param name="tile">The flip tile to be added.</param>
        private static void AddReferenceToEnabledPool(WeakReference tile)
        {
            if (!ContainsTarget(EnabledImagesPool, tile.Target))
            {
                EnabledImagesPool.Add(tile);
            }
        }

        /// <summary>
        /// Add a reference of a flip tile to the frozen images pool.
        /// </summary>
        /// <param name="tile">The flip tile to be added.</param>
        private static void AddReferenceToFrozenPool(WeakReference tile)
        {
            if (!ContainsTarget(FrozenImagesPool, tile.Target))
            {
                FrozenImagesPool.Add(tile);
            }
        }

        /// <summary>
        /// Add a reference of a flip tile to the stalled images pipeline.
        /// </summary>
        /// <param name="tile">The flip tile to be added.</param>
        private static void AddReferenceToStalledPipeline(WeakReference tile)
        {
            if (!ContainsTarget(StalledImagesPipeline, tile.Target))
            {
                StalledImagesPipeline.Add(tile);
            }
        }

        /// <summary>
        /// Remove the reference of a flip tile from the enabled images pool.
        /// </summary>
        /// <param name="tile">The flip tile to be removed.</param>
        private static void RemoveReferenceFromEnabledPool(WeakReference tile)
        {
            RemoveTarget(EnabledImagesPool, tile.Target);
        }

        /// <summary>
        /// Remove the reference of a flip tile from the frozen images pool.
        /// </summary>
        /// <param name="tile">The flip tile to be removed.</param>
        private static void RemoveReferenceFromFrozenPool(WeakReference tile)
        {
            RemoveTarget(FrozenImagesPool, tile.Target);
        }

        /// <summary>
        /// Remove the reference of a flip tile from the stalled images pipeline.
        /// </summary>
        /// <param name="tile">The flip tile to be removed.</param>
        private static void RemoveReferenceFromStalledPipeline(WeakReference tile)
        {
            RemoveTarget(StalledImagesPipeline, tile.Target);
        }

        /// <summary>
        /// Determine if there is a reference to a known target in a list.
        /// </summary>
        /// <param name="list">The list to be examined.</param>
        /// <param name="target">The known target.</param>
        /// <returns>True if a reference to the known target exists in the list. False otherwise.</returns>
        private static bool ContainsTarget(List<WeakReference> list, Object target)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Target == target)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove a reference to a known target in a list.
        /// </summary>
        /// <param name="list">The list to be examined.</param>
        /// <param name="target">The known target.</param>
        private static void RemoveTarget(List<WeakReference> list, Object target)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Target == target)
                {
                    list.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Executes the code to process a visual transition:
        /// 1. Stop the timer.
        /// 2. Advances the stalled tiles to the next step in the pipeline.
        /// If there is at least one tile that can be currently animated ...
        /// 3. Animate as many tiles as indicated.
        /// 4. Select a tile andomly from the pool of enabled tiles.
        /// 5. Based on this tile's current visual state, move it onto 
        /// the next one.
        /// 6. Set the stalling counter for the recently animated image.
        /// 7. Take it out from the pool and into the pipeline to prevent it 
        /// from being animated continuously.
        /// 8. Restart the timer with a randomly generated time interval
        /// between 100 and 3000 ms.
        /// Notice that if there are no flip tiles that can be animated, 
        /// the timer is not restarted.
        /// </summary>
        /// <param name="sender">The static timer.</param>
        /// <param name="e">The event information.</param>
        private static void OnTimerTick(object sender, object e)
        {
            Timer.Stop();

            for (int i = 0; i < StalledImagesPipeline.Count; i++)
            {
                if ((StalledImagesPipeline[i].Target as FlipTile)._stallingCounter-- == 0)
                {
                    AddReferenceToEnabledPool(StalledImagesPipeline[i]);
                    RemoveReferenceFromStalledPipeline(StalledImagesPipeline[i]);
                    i--;
                }
            }

            if (EnabledImagesPool.Count > 0) 
            {
                for (int j = 0; j < NumberOfSimultaneousAnimations; j++)
                {
                    int index = ProbabilisticBehaviorSelector.Next(EnabledImagesPool.Count);

                    switch ((EnabledImagesPool[index].Target as FlipTile).State)
                    {
                        case FlipState.Expanded:
                            //If the tile can neither drop nor flip, or if its size is Small, do not change state.
                            if ((!(EnabledImagesPool[index].Target as FlipTile)._canFlip) || (EnabledImagesPool[index].Target as FlipTile).Size == TileSize.Small)
                            {
                                break;
                            }

                            //If the tile can only flip, change to the Flipped state.
                            if ((EnabledImagesPool[index].Target as FlipTile)._canFlip)
                            {
                                (EnabledImagesPool[index].Target as FlipTile).State = FlipState.Flipped;
                                break;
                            }
                            break;
                        case FlipState.Flipped:
                            (EnabledImagesPool[index].Target as FlipTile).State = FlipState.Expanded;
                            break;
                    }
                    (EnabledImagesPool[index].Target as FlipTile)._stallingCounter = WaitingPipelineSteps;
                    AddReferenceToStalledPipeline(EnabledImagesPool[index]);
                    RemoveReferenceFromEnabledPool(EnabledImagesPool[index]);
                }
            }
            else if (StalledImagesPipeline.Count == 0)
            {
                return;
            }

            Timer.Interval = TimeSpan.FromMilliseconds(ProbabilisticBehaviorSelector.Next(1, 31) * 100);
            Timer.Start();
        }

        /// <summary>
        /// Freeze a flip tile.
        /// </summary>
        /// <param name="tile">The flip tile to be frozen.</param>
        public static void FreezeFlipTile(FlipTile tile)
        {
            WeakReference wref = new WeakReference(tile, TrackResurrection);
            AddReferenceToFrozenPool(wref);
            RemoveReferenceFromEnabledPool(wref);
            RemoveReferenceFromStalledPipeline(wref);
        }

        /// <summary>
        /// Unfreezes a flip tile and restarts the timer if needed.
        /// </summary>
        /// <param name="tile">The flip tile to be unfrozen.</param>
        public static void UnfreezeFlipTile(FlipTile tile)
        {
            WeakReference wref = new WeakReference(tile, TrackResurrection);
            AddReferenceToEnabledPool(wref);
            RemoveReferenceFromFrozenPool(wref);
            RemoveReferenceFromStalledPipeline(wref);

            RestartTimer();
        }

        /// <summary>
        /// Freezes all the flip tiles with the specified group tag that are not already frozen.
        /// </summary>
        /// <param name="group">The group tag representing the flip tiles that should be frozen.</param>
        public static void FreezeGroup(string group)
        {
            for (int i = 0; i < EnabledImagesPool.Count; i++)
            {
                if ((EnabledImagesPool[i].Target as FlipTile).GroupTag == group)
                {
                    (EnabledImagesPool[i].Target as FlipTile).IsFrozen = true;
                    i--;
                }
            }

            for (int j = 0; j < StalledImagesPipeline.Count; j++)
            {
                if ((StalledImagesPipeline[j].Target as FlipTile).GroupTag == group)
                {
                    (StalledImagesPipeline[j].Target as FlipTile).IsFrozen = true;
                    j--;
                }
            }
        }

        /// <summary>
        /// Unfreezes all the flip tiles with the specified group tag 
        /// that are currently frozen and restarts the timer if needed.
        /// </summary>
        /// <param name="group">The group tag representing the flip tiles that should be unfrozen.</param>
        public static void UnfreezeGroup(string group)
        {
            for (int i = 0; i < FrozenImagesPool.Count; i++)
            {
                if ((FrozenImagesPool[i].Target as FlipTile).GroupTag == group)
                {
                    (FrozenImagesPool[i].Target as FlipTile).IsFrozen = false;
                    i--;
                }
            }

            RestartTimer();
        }
    }
}