// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Objects.Drawables.Pieces;
using osuTK;

namespace osu.Game.Rulesets.Osu.Edit.Blueprints.HitCircles.Components
{
    public class HitCirclePiece : CompositeDrawable
    {
        private readonly HitCircle hitCircle;

        public HitCirclePiece(HitCircle hitCircle)
        {
            this.hitCircle = hitCircle;
            Origin = Anchor.Centre;

            Size = new Vector2(OsuHitObject.OBJECT_RADIUS * 2);
            Scale = new Vector2(hitCircle.Scale);
            CornerRadius = Size.X / 2;

            InternalChild = new RingPiece();
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.Yellow;
        }

        protected override void Update()
        {
            base.Update();

            UpdatePosition();
            Scale = new Vector2(hitCircle.Scale);
        }

        protected virtual void UpdatePosition() => Position = hitCircle.StackedPosition;
    }
}
