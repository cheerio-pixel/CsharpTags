

using System.Numerics;
using System.Xml;

namespace CsharpTags.Core.Types
{
    public static partial class Prelude
    {
        extension<T>(T x) where T : INumber<T>
        {
            /// <summary>
            /// Represents a measurement in pixels (px).
            /// 1px = 1/96th of 1in. The base unit for screen displays.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Values/length#absolute_length_units
            /// </summary>
            public string Px => $"{x}px";

            /// <summary>
            /// Represents a measurement in points (pt).
            /// 1pt = 1/72nd of 1in. Commonly used in print.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Values/length#absolute_length_units
            /// </summary>
            public string Pt => $"{x}pt";

            /// <summary>
            /// Represents a measurement in millimeters (mm).
            /// 1mm = 1/10th of 1cm.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Values/length#absolute_length_units
            /// </summary>
            public string Mm => $"{x}mm";

            /// <summary>
            /// Represents a measurement in centimeters (cm).
            /// 1cm = 96px/2.54.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Values/length#absolute_length_units
            /// </summary>
            public string Cm => $"{x}cm";

            /// <summary>
            /// Represents a measurement in inches (in).
            /// 1in = 2.54cm = 96px.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Values/length#absolute_length_units
            /// </summary>
            public string In => $"{x}in";

            /// <summary>
            /// Represents a measurement in picas (pc).
            /// 1pc = 12pt = 1/6th of 1in.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Values/length#absolute_length_units
            /// </summary>
            public string Pc => $"{x}pc";

            /// <summary>
            /// Represents a measurement in em units.
            /// Relative to the current element's font-size.
            /// If used on font-size itself, represents the inherited font-size.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Values/length#relative_length_units
            /// </summary>
            public string Em => $"{x}em";

            /// <summary>
            /// Represents a measurement in ch units.
            /// Relative to the width of the "0" (zero) glyph in the element's font.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Values/length#relative_length_units
            /// </summary>
            public string Ch => $"{x}ch";

            /// <summary>
            /// Represents a measurement in ex units.
            /// Relative to the x-height of the element's font (height of lowercase letters).
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Values/length#relative_length_units
            /// </summary>
            public string Ex => $"{x}ex";

            /// <summary>
            /// Represents a measurement in rem units.
            /// Relative to the root element's font-size.
            /// Common browser default is 16px.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference/Values/length#relative_length_units
            /// </summary>
            public string Rem => $"{x}rem";

            /// <summary>
            /// Represents an angle in degrees (deg).
            /// 360deg represents a full circle.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/angle
            /// </summary>
            public string Deg => $"{x}deg";

            /// <summary>
            /// Represents an angle in gradians (grad).
            /// 400grad represents a full circle.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/angle
            /// </summary>
            public string Grad => $"{x}grad";

            /// <summary>
            /// Represents an angle in radians (rad).
            /// 2π rad represents a full circle.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/angle
            /// </summary>
            public string Rad => $"{x}rad";

            /// <summary>
            /// Represents an angle in turns (turn).
            /// 1turn represents a full circle (360deg).
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/angle
            /// </summary>
            public string Turn => $"{x}turn";

            /// <summary>
            /// Represents a percentage value (%).
            /// Relative to the parent element's corresponding dimension.
            /// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/percentage
            /// </summary>
            public string Pct => $"{x}%";
        }
    }
}

