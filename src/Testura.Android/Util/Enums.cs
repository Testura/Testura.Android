#pragma warning disable 1591
namespace Testura.Android.Util
{
    /// <summary>
    /// Defines all ways to handle dependencies when creating a new device object.
    /// </summary>
    public enum DependencyHandling
    {
        /// <summary>
        /// Only install dependencies if missing.
        /// </summary>
        InstallIfMissing,

        /// <summary>
        /// Always install all dependencies.
        /// </summary>
        AlwaysInstall,

        /// <summary>
        /// Never install dependencies.
        /// </summary>
        NeverInstall
    }

    /// <summary>
    /// Defines all attribute tags that we can use to find a node.
    /// </summary>
    public enum AttributeTag
    {
        /// <summary>
        /// Node that contains text.
        /// </summary>
        TextContains,

        /// <summary>
        /// Node that have exact matching text.
        /// </summary>
        Text,

        /// <summary>
        /// Node that have resource id.
        /// </summary>
        ResourceId,

        /// <summary>
        /// Node that have content desc.
        /// </summary>
        ContentDesc,

        /// <summary>
        /// Node that have class.
        /// </summary>
        Class,

        /// <summary>
        /// Node that have package.
        /// </summary>
        Package,

        /// <summary>
        /// Node that have index.
        /// </summary>
        Index
    }

    /// <summary>
    /// Defines all swipe direction
    /// </summary>
    public enum SwipeDirection
    {
        /// <summary>
        /// Swipe left.
        /// </summary>
        Left,

        /// <summary>
        /// Swipe up.
        /// </summary>
        Up,

        /// <summary>
        /// Swipe right.
        /// </summary>
        Right,

        /// <summary>
        /// Swipe down.
        /// </summary>
        Down
    }

    /// <summary>
    /// Defines wanted state of a setting in the devices.
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Enable the setting.
        /// </summary>
        Enable,

        /// <summary>
        /// Disable the setting.
        /// </summary>
        Disable
    }

    /// <summary>
    /// Defines all different key events that can be sent to a device.
    /// </summary>
    public enum KeyEvent
    {
        Unknown,
        Menu,
        SoftRight,
        Home,
        Back,
        Call,
        Endcall,
        Number0,
        Number1,
        Number2,
        Number3,
        Number4,
        Number5,
        Number6,
        Number7,
        Number8,
        Number9,
        Star,
        Pound,
        DpadUp,
        DpadDown,
        DpadLeft,
        DpadRight,
        DpadCenter,
        VolumeUp,
        VolumeDown,
        Power,
        Camera,
        Clear,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        Comma,
        Period,
        AltLeft,
        AltRight,
        ShiftLeft,
        ShiftRight,
        Tab,
        Space,
        Sym,
        Explorer,
        Envelope,
        Enter,
        Del,
        Grave,
        Minus,
        Equals,
        LeftBracket,
        RightBracket,
        Backslash,
        Semicolon,
        Apostrophe,
        Slash,
        At,
        Num,
        Headsethook,
        Focus,
        Plus,
        Menu2,
        Notification,
        Search,
        TagLastKeycode,
        Sleep = 223,
        WakeUp = 224,
        SoftSleep = 276
    }

    /// <summary>
    /// Defines all possible walker inputs.
    /// </summary>
    public enum WalkerInputs
    {
        /// <summary>
        /// Tap on the screen.
        /// </summary>
        Tap,

        /// <summary>
        /// Swipe on the screen.
        /// </summary>
        Swipe
    }
}