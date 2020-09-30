// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays.Settings;

namespace osu.Game.Screens.Edit.Timing
{
    internal class SliderWithTextBoxInput<T> : CompositeDrawable, IHasCurrentValue<T>
        where T : struct, IEquatable<T>, IComparable<T>, IConvertible
    {
        private readonly SettingsSlider<T> slider;

        public SliderWithTextBoxInput(string labelText)
        {
            LabelledTextBox textbox;

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            InternalChildren = new Drawable[]
            {
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        textbox = new LabelledTextBox
                        {
                            Label = labelText,
                        },
                        slider = new SettingsSlider<T>
                        {
                            RelativeSizeAxes = Axes.X,
                        }
                    }
                },
            };

            textbox.OnCommit += (t, isNew) =>
            {
                if (!isNew) return;

                try
                {
                    slider.Bindable.Parse(t.Text);
                }
                catch
                {
                    // will restore the previous text value on failure.
                    Current.TriggerChange();
                }
            };

            Current.BindValueChanged(val =>
            {
                textbox.Text = val.NewValue.ToString();
            }, true);
        }

        public Bindable<T> Current
        {
            get => slider.Bindable;
            set => slider.Bindable = value;
        }
    }
}
