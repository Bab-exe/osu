// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osuTK;
using osuTK.Input;

namespace osu.Game.Rulesets.Osu.Edit.Blueprints
{
    public partial class GridPlacementBlueprint : PlacementBlueprint
    {
        [Resolved]
        private HitObjectComposer? hitObjectComposer { get; set; }

        private OsuGridToolboxGroup gridToolboxGroup = null!;
        private Vector2 originalOrigin;
        private float originalSpacing;
        private float originalRotation;

        [BackgroundDependencyLoader]
        private void load(OsuGridToolboxGroup gridToolboxGroup)
        {
            this.gridToolboxGroup = gridToolboxGroup;
            originalOrigin = gridToolboxGroup.StartPosition.Value;
            originalSpacing = gridToolboxGroup.Spacing.Value;
            originalRotation = gridToolboxGroup.GridLinesRotation.Value;
        }

        public override void EndPlacement(bool commit)
        {
            if (!commit && PlacementActive != PlacementState.Finished)
            {
                gridToolboxGroup.StartPosition.Value = originalOrigin;
                gridToolboxGroup.Spacing.Value = originalSpacing;
                gridToolboxGroup.GridLinesRotation.Value = originalRotation;
            }

            base.EndPlacement(commit);

            // You typically only place the grid once, so we switch back to the select tool after placement.
            if (commit && hitObjectComposer is OsuHitObjectComposer osuHitObjectComposer)
                osuHitObjectComposer.SetSelectTool();
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                EndPlacement(true);
                return true;
            }

            return base.OnClick(e);
        }

        protected override bool OnDragStart(DragStartEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                BeginPlacement(true);
                return true;
            }

            return base.OnDragStart(e);
        }

        protected override void OnDragEnd(DragEndEvent e)
        {
            if (PlacementActive == PlacementState.Active)
                EndPlacement(true);

            base.OnDragEnd(e);
        }

        public override SnapType SnapType => ~SnapType.GlobalGrids;

        public override void UpdateTimeAndPosition(SnapResult result)
        {
            var pos = ToLocalSpace(result.ScreenSpacePosition);

            if (PlacementActive != PlacementState.Active)
                gridToolboxGroup.StartPosition.Value = pos;
            else
                gridToolboxGroup.SetGridFromPoints(gridToolboxGroup.StartPosition.Value, pos);
        }
    }
}
