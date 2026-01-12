using MudBlazor;
using MudBlazor.Utilities;

namespace NetBlaze.Ui.Client.InternalHelperTypes.Constants
{
    public static class SharedTheme
    {
        static readonly MudColor NetBlazePrimary = new("#2ECCB0");
        static readonly MudColor NetBlazeSecondary = new("#6B9080");
        static readonly MudColor NetBlazeTertiary = new("#1ABC9C");

        static readonly MudColor NetBlazeWarning = new("#F4A261");
        static readonly MudColor NetBlazeError = new("#E76F51");
        static readonly MudColor NetBlazeSuccess = new("#2ECC71");

        static readonly MudColor NetBlazeDrawerBackground = new("#00796B");
        static readonly MudColor NetBlazeDrawerText = Colors.Shades.White;

        static readonly MudColor NetBlazeAppBarBackground = new("#E6F7F3");
        static readonly MudColor NetBlazeAppBarText = new("#004D40");

        static readonly MudColor NetBlazeTextPrimary = new("#1F2937");
        static readonly MudColor NetBlazeTextSecondary = new("#4B5563");

        public static readonly MudTheme NetBlazeTheme = new()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = NetBlazePrimary,
                Secondary = NetBlazeSecondary,
                Tertiary = NetBlazeTertiary,

                Success = NetBlazeSuccess,
                Warning = NetBlazeWarning,
                Error = NetBlazeError,

                AppbarBackground = NetBlazeAppBarBackground,
                AppbarText = NetBlazeAppBarText,

                DrawerBackground = NetBlazeDrawerBackground,
                DrawerText = NetBlazeDrawerText,

                TextPrimary = NetBlazeTextPrimary,
                TextSecondary = NetBlazeTextSecondary,

                HoverOpacity = 0.08
            },

            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "260px",
                DrawerWidthRight = "260px"
            }
        };
    }
}
