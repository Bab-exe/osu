// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Objects.Drawables.Pieces;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Osu.Edit.Blueprints.Sliders.Components
{
    public class SliderBodyPiece : CompositeDrawable
    {
        private readonly Slider slider;
        private readonly ManualSliderBody body;

        public SliderBodyPiece(Slider slider)
        {
            this.slider = slider;

            InternalChild = body = new ManualSliderBody
            {
                AccentColour = Color4.Transparent,
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            body.BorderColour = colours.Yellow;
        }

        private void updatePosition() => Position = slider.StackedPosition;

        protected override void Update()
        {
            base.Update();

            Position = slider.StackedPosition;
            body.PathRadius = slider.Scale * OsuHitObject.OBJECT_RADIUS;

            var vertices = new List<Vector2>();
            slider.Path.GetPathToProgress(vertices, 0, 1);

            body.SetVertices(vertices);

            Size = body.Size;
            OriginPosition = body.PathOffset;
        }
    }
}
