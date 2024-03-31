using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static MZZT.WinApi.PInvoke.Windef;
using static MZZT.WinApi.PInvoke.WinError;

namespace MZZT.WinApi.PInvoke {
	public static class User32 {
		public const int CHILDID_SELF = 0;

		public const int HWND_NOTOPMOST = -2;
		public const int HWND_TOPMOST = -1;
		public const int HWND_TOP = 0;
		public const int HWND_BOTTOM = 1;

		[Flags]
		public enum DESKTOP {
			READOBJECTS = 0x1,
			CREATEWINDOW = 0x2,
			CREATEMENU = 0x4,
			HOOKCONTROL = 0x8,
			JOURNALRECORD = 0x10,
			JOURNALPLAYBACK = 0x20,
			ENUMERATE = 0x40,
			WRITEOBJECTS = 0x80,
			SWITCHDESKTOP = 0x100,

			DELETE = 0x00010000,
			READ_CONTROL = 0x00020000,
			WRITE_DAC = 0x00040000,
			WRITE_OWNER = 0x00080000,

			STANDARD_RIGHTS_EXECUTE = 0x00020000,
			STANDARD_RIGHTS_READ = 0x00020000,
			STANDARD_RIGHTS_REQUIRED = 0x000F0000,
			STANDARD_RIGHTS_WRITE = 0x00020000,

			GENERIC_READ = 0x00020041,
			GENERIC_WRITE = 0x000200BE,
			GENERIC_EXECUTE = 0x00020100,
			GENERIC_ALL = 0x000F01FF
		}

		[Flags]
		public enum DF {
			ALLOWOTHERACCOUNTHOOK = 0x1
		}

		public enum EVENT : uint {
			SYSTEM_SOUND = 0x1,
			SYSTEM_ALERT = 0x2,
			SYSTEM_FOREGROUND = 0x3,
			SYSTEM_MENUSTART = 0x4,
			SYSTEM_MENUEND = 0x5,
			SYSTEM_MENUPOPUPSTART = 0x6,
			SYSTEM_MENUPOPUPEND = 0x7,
			SYSTEM_CAPTURESTART = 0x8,
			SYSTEM_CAPTUREEND = 0x9,
			SYSTEM_MOVESIZESTART = 0xA,
			SYSTEM_MOVESIZEEND = 0xB,
			SYSTEM_CONTEXTHELPSTART = 0xC,
			SYSTEM_CONTEXTHELPEND = 0xD,
			SYSTEM_DRAGDROPSTART = 0xE,
			SYSTEM_DRAGDROPEND = 0xF,
			SYSTEM_DIALOGSTART = 0x10,
			SYSTEM_DIALOGEND = 0x11,
			SYSTEM_SCROLLINGSTART = 0x12,
			SYSTEM_SCROLLINGEND = 0x13,
			SYSTEM_SWITCHSTART = 0x14,
			SYSTEM_SWITCHEND = 0x15,
			SYSTEM_MINIMIZESTART = 0x16,
			SYSTEM_MINIMIZEEND = 0x17,
			CONSOLE_CARET = 0x4001,
			CONSOLE_UPDATE_REGION = 0x4002,
			CONSOLE_UPDATE_SIMPLE = 0x4003,
			CONSOLE_UPDATE_SCROLL = 0x4004,
			CONSOLE_LAYOUT = 0x4005,
			CONSOLE_START_APPLICATION = 0x4006,
			OBJECT_CREATE = 0x8000,
			OBJECT_DESTROY = 0x8001,
			OBJECT_SHOW = 0x8002,
			OBJECT_HIDE = 0x8003,
			OBJECT_REORDER = 0x8004,
			OBJECT_FOCUS = 0x8005,
			OBJECT_SELECTION = 0x8006,
			OBJECT_SELECTIONADD = 0x8007,
			OBJECT_SELECTIONREMOVE = 0x8008,
			OBJECT_SELECTIONWITHIN = 0x8009,
			OBJECT_STATECHANGE = 0x800A,
			OBJECT_LOCATIONCHANGE = 0x800B,
			OBJECT_NAMECHANGE = 0x800C,
			OBJECT_DESCRIPTIONCHANGE = 0x800D,
			OBJECT_VALUECHANGE = 0x800E,
			OBJECT_PARENTCHANGE = 0x800F,
			OBJECT_HELPCHANGE = 0x8010,
			OBJECT_DEFACTIONCHANGE = 0x8011,
			OBJECT_ACCELERATORCHANGE = 0x8012,
			CONSOLE_END_APPLICATION = 0x4007,

			MIN = 0x1,
			MAX = 0x7FFFFFFF
		}

		public enum GA {
			PARENT = 0x1,
			ROOT = 0x2,
			ROOTOWNER = 0x3
		}

		public enum GCL {
			HICON = -14,
			HICONSM = -34
		}

		public enum GW : uint {
			OWNER = 0x4
		}

		public enum GWL {
			STYLE = -16,
			EXSTYLE = -20
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct HARDWAREINPUT {
			public int uMsg;
			public short wParamL;
			public short wParamH;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct KEYBDINPUT {
			public short wVk;
			public short wScan;
			public int dwFlags;
			public int time;
			public IntPtr dwExtraInfo;
		}

		public enum ICON {
			SMALL = 0,
			BIG = 1,
			SMALL2 = 2
		}

		public enum INPUTe {
			MOUSE = 0x0,
			KEYBOARD = 0x1,
			HARDWARE = 0x2
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct INPUTs {
			public INPUTe dwType;
			public INPUTi i;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct INPUTi {
			[FieldOffset(0)]
			public MOUSEINPUT mi;
			[FieldOffset(0)]
			public KEYBDINPUT ki;
			[FieldOffset(0)]
			public HARDWAREINPUT hi;
		}

		[Flags]
		public enum KEYSTATERET : short {
			TOGGLED = 1,
			PRESSED = -128
		}

		public enum MA {
			ACTIVATE = 0x1,
			ACTIVATEANDEAT = 0x2,
			NOACTIVATE = 0x3,
			NOACTIVATEANDEAT = 0x4
		}

		[Flags]
		public enum MOD : uint {
			NONE = 0x0,
			ALT = 0x1,
			CONTROL = 0x2,
			SHIFT = 0x4,
			WIN = 0x8,
			NOREPEAT = 0x4000
		}

		[Flags]
		public enum MOUSEEVENTF {
			MOVE = 0x1,
			LEFTDOWN = 0x2,
			LEFTUP = 0x4,
			RIGHTDOWN = 0x8,
			RIGHTUP = 0x10,
			MIDDLEDOWN = 0x20,
			MIDDLEUP = 0x40,
			XDOWN = 0x80,
			XUP = 0x100,
			WHEEL = 0x800,
			VIRTUALDESK = 0x4000,
			ABSOLUTE = 0x8000
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct MOUSEINPUT {
			public int dx;
			public int dy;
			public int mouseData;
			public MOUSEEVENTF dwFlags;
			public int time;
			public IntPtr dwExtraInfo;
		}
	
		public enum OBJID : uint {
			WINDOW = 0x0,
			SOUND = 0xFFFFFFF5,
			ALERT = 0xFFFFFFF6,
			CURSOR = 0xFFFFFFF7,
			CARET = 0xFFFFFFF8,
			SIZEGRIP = 0xFFFFFFF9,
			HSCROLL = 0xFFFFFFFA,
			VSCROLL = 0xFFFFFFFB,
			CLIENT = 0xFFFFFFFC,
			MENU = 0xFFFFFFFD,
			TITLEBAR = 0xFFFFFFFE,
			SYSMENU = 0xFFFFFFFF
		}

		[Flags]
		public enum SIF {
			RANGE = 0x1,
			PAGE = 0x2,
			POS = 0x4,
			DISABLENOSCROLL = 0x8,
			TRACKPOS = 0x10,
			ALL = 0x17
		}

		public enum SB {
			HORZ = 0x0,
			VERT = 0x1,
			CTL = 0x2,
			BOTH = 0x3,

			LINEUP = 0x0,
			LINELEFT = 0x0,
			LINEDOWN = 0x1,
			LINERIGHT = 0x1,
			PAGEUP = 0x2,
			PAGELEFT = 0x2,
			PAGEDOWN = 0x3,
			PAGERIGHT = 0x3,
			THUMBPOSITION = 0x4,
			THUMBTRACK = 0x5,
			TOP = 0x6,
			LEFT = 0x6,
			BOTTOM = 0x7,
			RIGHT = 0x7,
			ENDSCROLL = 0x8
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SCROLLINFO {
			public uint cbSize;
			public SIF fMask;
			public int nMin;
			public int nMax;
			public uint nPage;
			public int nPos;
			public int nTrackPos;
		}

		public enum SM {
			ARRANGE = 56,
			CLEANBOOT = 67,
			CMONITORS = 80,
			CMOUSEBUTTONS = 43,
			CONVERTIBLESLATEMODE = 0x2003,
			CXBORDER = 5,
			CXCURSOR = 13,
			CXDLGFRAME = 7,
			CXDOUBLECLK = 36,
			CXDRAG = 68,
			CXEDGE = 45,
			CXFIXEDFRAME = 7,
			CXFOCUSBORDER = 83,
			CXFRAME = 32,
			CXFULLSCREEN = 16,
			CXHSCROLL = 21,
			CXHTHUMB = 10,
			CXICON = 11,
			CXICONSPACING = 38,
			CXMAXIMIZED = 61,
			CXMAXTRACK = 59,
			CXMENUCHECK = 71,
			CXMENUSIZE = 54,
			CXMIN = 28,
			CXMINIMIZED = 57,
			CXMINSPACING = 47,
			CXMINTRACK = 34,
			CXPADDEDBORDER = 92,
			CXSCREEN = 0,
			CXSIZE = 30,
			CXSIZEFRAME = 32,
			CXSMICON = 49,
			CXSMSIZE = 52,
			CXVIRTUALSCREEN = 78,
			CXVSCROLL = 2,
			CYBORDER = 6,
			CYCAPTION = 4,
			CYCURSOR = 14,
			CYDLGFRAME = 8,
			CYDOUBLECLK = 37,
			CYDRAG = 69,
			CYEDGE = 46,
			CYFIXEDFRAME = 8,
			CYFOCUSBORDER = 84,
			CYFRAME = 33,
			CYFULLSCREEN = 17,
			CYHSCROLL = 3,
			CYICON = 12,
			CYICONSPACING = 39,
			CYKANJIWINDOW = 18,
			CYMAXIMIZED = 62,
			CYMAXTRACK = 60,
			CYMENU = 15,
			CYMENUCHECK = 72,
			CYMENUSIZE = 55,
			CYMIN = 29,
			CYMINIMIZED = 58,
			CYMINSPACING = 48,
			CYMINTRACK = 35,
			CYSCREEN = 1,
			CYSIZE = 31,
			CYSIZEFRAME = 33,
			CYSMCAPTION = 51,
			CYSMICON = 50,
			CYSMSIZE = 53,
			CYVIRTUALSCREEN = 79,
			CYVSCROLL = 20,
			CYVTHUMB = 9,
			DBCSENABLED = 42,
			DEBUG = 22,
			DIGITIZER = 94,
			IMMENABLED = 82,
			MAXIMUMTOUCHES = 95,
			MEDIACENTER = 87,
			MENUDROPALIGNMENT = 40,
			MIDEASTENABLED = 74,
			MOUSEPRESENT = 19,
			MOUSEHORIZONTALWHEELPRESENT = 91,
			MOUSEWHEELPRESENT = 75,
			NETWORK = 63,
			PENWINDOWS = 41,
			REMOTECONTROL = 0x2001,
			REMOTESESSION = 0x1000,
			SAMEDISPLAYFORMAT = 81,
			SECURE = 44,
			SERVERR2 = 89,
			SHOWSOUNDS = 70,
			SHUTTINGDOWN = 0x2000,
			SLOWMACHINE = 73,
			STARTER = 88,
			SWAPBUTTON = 23,
			SYSTEMDOCKED = 0x2004,
			TABLETPC = 86,
			XVIRTUALSCREEN = 76,
			YVIRTUALSCREEN = 77
		}

		[Flags]
		public enum SMTO : uint {
			NORMAL = 0x0,
			BLOCK = 0x1,
			ABORTIFHUNG = 0x2,
			NOTIMEOUTIFNOTHUNG = 0x8,
			ERRORONEXIT = 0x20
		}

		public enum SW : uint {
			HIDE = 0x0,
			SHOWNORMAL = 0x1,
			SHOWMINIMIZED = 0x2,
			MAXIMIZE = 0x3,
			SHOWMAXIMIZED = 0x3,
			SHOWNOACTIVATE = 0x4,
			SHOW = 0x5,
			MINIMIZE = 0x6,
			SHOWMINNOACTIVE = 0x7,
			SHOWNA = 0x8,
			RESTORE = 0x9,
			SHOWDEFAULT = 0xA,
			FORCEMINIMIZE = 0xB
		}

		[Flags]
		public enum SWP : uint {
			ASYNCWINDOWPOS = 0x4000,
			DEFERERASE = 0x2000,
			DRAWFRAME = 0x0020,
			FRAMECHANGED = 0x0020,
			HIDEWINDOW = 0x0080,
			NOACTIVATE = 0x0010,
			NOCOPYBITS = 0x0100,
			NOMOVE = 0x0002,
			NOOWNERZORDER = 0x0200,
			NOREDRAW = 0x0008,
			NOREPOSITION = 0x0200,
			NOSENDCHANGING = 0x0400,
			NOSIZE = 0x0001,
			NOZORDER = 0x0004,
			SHOWWINDOW = 0x0040
		}

		public enum UOI {
			FLAGS = 1,
			HEAPSIZE = 5,
			IO = 6,
			NAME = 2,
			TYPE = 3,
			USER_SID = 4
		}

		public enum VK {
			KEYCODE = 0xFFFF,
			MODIFIERS = -65536,
			NONE = 0x00,
			LBUTTON = 0x01,
			RBUTTON = 0x02,
			CANCEL = 0x03,
			MBUTTON = 0x04,
			XBUTTON1 = 0x05,
			XBUTTON2 = 0x06,
			BACK = 0x08,
			TAB = 0x09,
			LINEFEED = 0x0A,
			CLEAR = 0x0C,
			RETURN = 0x0D,
			SHIFT = 0x10,
			CONTROL = 0x11,
			MENU = 0x12,
			PAUSE = 0x13,
			CAPITAL = 0x14,
			KANA = 0x15,
			HANGUEL = 0x15,
			HANGUL = 0x15,
			IME_ON = 0x16,
			JUNJA = 0x17,
			FINAL = 0x18,
			HANJA = 0x19,
			KANJI = 0x19,
			IME_OFF = 0x1A,
			ESCAPE = 0x1B,
			CONVERT = 0x1C,
			NONCONVERT = 0x1D,
			ACCEPT = 0x1E,
			MODECHANGE = 0x1F,
			SPACE = 0x20,
			PRIOR = 0x21,
			NEXT = 0x22,
			END = 0x23,
			HOME = 0x24,
			LEFT = 0x25,
			UP = 0x26,
			RIGHT = 0x27,
			DOWN = 0x28,
			SELECT = 0x29,
			PRINT = 0x2A,
			EXECUTE = 0x2B,
			SNAPSHOT = 0x2C,
			INSERT = 0x2D,
			DELETE = 0x2E,
			HELP = 0x2F,
			D0 = 0x30,
			D1 = 0x31,
			D2 = 0x32,
			D3 = 0x33,
			D4 = 0x34,
			D5 = 0x35,
			D6 = 0x36,
			D7 = 0x37,
			D8 = 0x38,
			D9 = 0x39,
			A = 0x41,
			B = 0x42,
			C = 0x43,
			D = 0x44,
			E = 0x45,
			F = 0x46,
			G = 0x47,
			H = 0x48,
			I = 0x49,
			J = 0x4A,
			K = 0x4B,
			L = 0x4C,
			M = 0x4D,
			N = 0x4E,
			O = 0x4F,
			P = 0x50,
			Q = 0x51,
			R = 0x52,
			S = 0x53,
			T = 0x54,
			U = 0x55,
			V = 0x56,
			W = 0x57,
			X = 0x58,
			Y = 0x59,
			Z = 0x5A,
			LWIN = 0x5B,
			RWIN = 0x5C,
			APPS = 0x5D,
			SLEEP = 0x5F,
			NUMPAD0 = 0x60,
			NUMPAD1 = 0x61,
			NUMPAD2 = 0x62,
			NUMPAD3 = 0x63,
			NUMPAD4 = 0x64,
			NUMPAD5 = 0x65,
			NUMPAD6 = 0x66,
			NUMPAD7 = 0x67,
			NUMPAD8 = 0x68,
			NUMPAD9 = 0x69,
			MULTIPLY = 0x6A,
			ADD = 0x6B,
			SEPARATOR = 0x6C,
			SUBTRACT = 0x6D,
			DECIMAL = 0x6E,
			DIVIDE = 0x6F,
			F1 = 0x70,
			F2 = 0x71,
			F3 = 0x72,
			F4 = 0x73,
			F5 = 0x74,
			F6 = 0x75,
			F7 = 0x76,
			F8 = 0x77,
			F9 = 0x78,
			F10 = 0x79,
			F11 = 0x7A,
			F12 = 0x7B,
			F13 = 0x7C,
			F14 = 0x7D,
			F15 = 0x7E,
			F16 = 0x7F,
			F17 = 0x80,
			F18 = 0x81,
			F19 = 0x82,
			F20 = 0x83,
			F21 = 0x84,
			F22 = 0x85,
			F23 = 0x86,
			F24 = 0x87,
			NUMLOCK = 0x90,
			SCROLL = 0x91,
			LSHIFT = 0xA0,
			RSHIFT = 0xA1,
			LCONTROL = 0xA2,
			RCONTROL = 0xA3,
			LMENU = 0xA4,
			RMENU = 0xA5,
			BROWSER_BACK = 0xA6,
			BROWSER_FORWARD = 0xA7,
			BROWSER_REFRESH = 0xA8,
			BROWSER_STOP = 0xA9,
			BROWSER_SEARCH = 0xAA,
			BROWSER_FAVORITES = 0xAB,
			BROWSER_HOME = 0xAC,
			VOLUME_MUTE = 0xAD,
			VOLUME_DOWN = 0xAE,
			VOLUME_UP = 0xAF,
			MEDIA_NEXT_TRACK = 0xB0,
			MEDIA_PREV_TRACK = 0xB1,
			MEDIA_STOP = 0xB2,
			MEDIA_PLAY_PAUSE = 0xB3,
			LAUNCH_MAIL = 0xB4,
			LAUNCH_MEDIA_SELECT = 0xB5,
			LAUNCH_APP1 = 0xB6,
			LAUNCH_APP2 = 0xB7,
			SEMICOLON = 0xBA,
			OEM_1 = 0xBA,
			OEM_PLUS = 0xBB,
			OEM_COMMA = 0xBC,
			OEM_MINUS = 0xBD,
			OEM_PERIOD = 0xBE,
			QUESTION = 0xBF,
			OEM_2 = 0xBF,
			TILDE = 0xC0,
			OEM_3 = 0xC0,
			OPENBRACKETS = 0xDB,
			OEM_4 = 0xDB,
			PIPE = 0xDC,
			OEM_5 = 0xDC,
			CLOSEBRACKETS = 0xDD,
			OEM_6 = 0xDD,
			QUOTES = 0xDE,
			OEM_7 = 0xDE,
			OEM_8 = 0xDF,
			OEM_102 = 0xE2,
			PROCESSKEY = 0xE5,
			PACKET = 0xE7,
			ATTN = 0xF6,
			CRSEL = 0xF7,
			EXSEL = 0xF8,
			EREOF = 0xF9,
			PLAY = 0xFA,
			ZOOM = 0xFB,
			NONAME = 0xFC,
			PA1 = 0xFD,
			OEM_CLEAR = 0xFE,
			MOD_SHIFT = 0x10000,
			MOD_CONTROL = 0x20000,
			MOD_ALT = 0x40000
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WINDOWPLACEMENT {
			public void InitLength() {
				this.length = (uint)Marshal.SizeOf<WINDOWPLACEMENT>();
			}

			public uint length;
			public WPF flags;
			public SW showCmd;
			public POINT ptMinPosition;
			public POINT ptMaxPosition;
			public RECT rcNormalPosition;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WINDOWPOS {
			public IntPtr hwnd;
			public IntPtr hwndInsertAfter;
			public int x;
			public int y;
			public int cx;
			public int cy;
			public SWP flags;
		}

		[Flags]
		public enum WINEVENT : uint {
			OUTOFCONTEXT = 0x0,
			SKIPOWNTHREAD = 0x1,
			SKIPOWNPROCESS = 0x2,
			INCONTEXT = 0x4
		}

		public enum WM : uint {
			NULL = 0x00,
			CREATE = 0x01,
			DESTROY = 0x02,
			MOVE = 0x03,
			SIZE = 0x05,
			ACTIVATE = 0x06,
			SETFOCUS = 0x07,
			KILLFOCUS = 0x08,
			ENABLE = 0x0A,
			SETREDRAW = 0x0B,
			SETTEXT = 0x0C,
			GETTEXT = 0x0D,
			GETTEXTLENGTH = 0x0E,
			PAINT = 0x0F,
			CLOSE = 0x10,
			QUERYENDSESSION = 0x11,
			QUIT = 0x12,
			QUERYOPEN = 0x13,
			ERASEBKGND = 0x14,
			SYSCOLORCHANGE = 0x15,
			ENDSESSION = 0x16,
			SYSTEMERROR = 0x17,
			SHOWWINDOW = 0x18,
			CTLCOLOR = 0x19,
			WININICHANGE = 0x1A,
			SETTINGCHANGE = 0x1A,
			DEVMODECHANGE = 0x1B,
			ACTIVATEAPP = 0x1C,
			FONTCHANGE = 0x1D,
			TIMECHANGE = 0x1E,
			CANCELMODE = 0x1F,
			SETCURSOR = 0x20,
			MOUSEACTIVATE = 0x21,
			CHILDACTIVATE = 0x22,
			QUEUESYNC = 0x23,
			GETMINMAXINFO = 0x24,
			PAINTICON = 0x26,
			ICONERASEBKGND = 0x27,
			NEXTDLGCTL = 0x28,
			SPOOLERSTATUS = 0x2A,
			DRAWITEM = 0x2B,
			MEASUREITEM = 0x2C,
			DELETEITEM = 0x2D,
			VKEYTOITEM = 0x2E,
			CHARTOITEM = 0x2F,
			SETFONT = 0x30,
			GETFONT = 0x31,
			SETHOTKEY = 0x32,
			GETHOTKEY = 0x33,
			QUERYDRAGICON = 0x37,
			COMPAREITEM = 0x39,
			GETOBJECT = 0x3D,
			COMPACTING = 0x41,
			COMMNOTIFY = 0x44,
			WINDOWPOSCHANGING = 0x46,
			WINDOWPOSCHANGED = 0x47,
			POWER = 0x48,
			COPYDATA = 0x4A,
			CANCELJOURNAL = 0x4B,
			NOTIFY = 0x4E,
			INPUTLANGCHANGEREQUEST = 0x50,
			INPUTLANGCHANGE = 0x51,
			TCARD = 0x52,
			HELP = 0x53,
			USERCHANGED = 0x54,
			NOTIFYFORMAT = 0x55,
			CONTEXTMENU = 0x7B,
			STYLECHANGING = 0x7C,
			STYLECHANGED = 0x7D,
			DISPLAYCHANGE = 0x7E,
			GETICON = 0x7F,
			SETICON = 0x80,
			NCCREATE = 0x81,
			NCDESTROY = 0x82,
			NCCALCSIZE = 0x83,
			NCHITTEST = 0x84,
			NCPAINT = 0x85,
			NCACTIVATE = 0x86,
			GETDLGCODE = 0x87,
			SYNCPAINT = 0x88,
			NCMOUSEMOVE = 0xA0,
			NCLBUTTONDOWN = 0xA1,
			NCLBUTTONUP = 0xA2,
			NCLBUTTONDBLCLK = 0xA3,
			NCRBUTTONDOWN = 0xA4,
			NCRBUTTONUP = 0xA5,
			NCRBUTTONDBLCLK = 0xA6,
			NCMBUTTONDOWN = 0xA7,
			NCMBUTTONUP = 0xA8,
			NCMBUTTONDBLCLK = 0xA9,
			NCXBUTTONDOWN = 0xAB,
			NCXBUTTONUP = 0xAC,
			NCXBUTTONDBLCLK = 0xAD,
			INPUT_DEVICE_CHANGE = 0xFE,
			INPUT = 0xFF,
			KEYFIRST = 0x100,
			KEYDOWN = 0x100,
			KEYUP = 0x101,
			CHAR = 0x102,
			DEADCHAR = 0x103,
			SYSKEYDOWN = 0x104,
			SYSKEYUP = 0x105,
			SYSCHAR = 0x106,
			SYSDEADCHAR = 0x107,
			UNICHAR = 0x109,
			KEYLAST = 0x109,
			IME_STARTCOMPOSITION = 0x10D,
			IME_ENDCOMPOSITION = 0x10E,
			IME_COMPOSITION = 0x10F,
			IME_KEYLAST = 0x10F,
			INITDIALOG = 0x110,
			COMMAND = 0x111,
			SYSCOMMAND = 0x112,
			TIMER = 0x113,
			HSCROLL = 0x114,
			VSCROLL = 0x115,
			INITMENU = 0x116,
			INITMENUPOPUP = 0x117,
			GESTURE = 0x119,
			GESTURENOTIFY = 0x11A,
			MENUSELECT = 0x11F,
			MENUCHAR = 0x120,
			ENTERIDLE = 0x121,
			MENURBUTTONUP = 0x122,
			MENUDRAG = 0x123,
			MENUGETOBJECT = 0x124,
			UNINITMENUPOPUP = 0x125,
			MENUCOMMAND = 0x126,
			CHANGEUISTATE = 0x127,
			UPDATEUISTATE = 0x128,
			QUERYUISTATE = 0x129,
			CTLCOLORMSGBOX = 0x132,
			CTLCOLOREDIT = 0x133,
			CTLCOLORLISTBOX = 0x134,
			CTLCOLORBTN = 0x135,
			CTLCOLORDLG = 0x136,
			CTLCOLORSCROLLBAR = 0x137,
			CTLCOLORSTATIC = 0x138,
			MOUSEFIRST = 0x200,
			MOUSEMOVE = 0x200,
			LBUTTONDOWN = 0x201,
			LBUTTONUP = 0x202,
			LBUTTONDBLCLK = 0x203,
			RBUTTONDOWN = 0x204,
			RBUTTONUP = 0x205,
			RBUTTONDBLCLK = 0x206,
			MBUTTONDOWN = 0x207,
			MBUTTONUP = 0x208,
			MBUTTONDBLCLK = 0x209,
			MOUSEWHEEL = 0x20A,
			XBUTTONDOWN = 0x20B,
			XBUTTONUP = 0x20C,
			XBUTTONDBLCLK = 0x20D,
			MOUSEHWHEEL = 0x20E,
			MOUSELAST = 0x20E,
			PARENTNOTIFY = 0x210,
			ENTERMENULOOP = 0x211,
			EXITMENULOOP = 0x212,
			NEXTMENU = 0x213,
			SIZING = 0x214,
			CAPTURECHANGED = 0x215,
			MOVING = 0x216,
			POWERBROADCAST = 0x218,
			DEVICECHANGE = 0x219,
			MDICREATE = 0x220,
			MDIDESTROY = 0x221,
			MDIACTIVATE = 0x222,
			MDIRESTORE = 0x223,
			MDINEXT = 0x224,
			MDIMAXIMIZE = 0x225,
			MDITILE = 0x226,
			MDICASCADE = 0x227,
			MDIICONARRANGE = 0x228,
			MDIGETACTIVE = 0x229,
			MDISETMENU = 0x230,
			ENTERSIZEMOVE = 0x231,
			EXITSIZEMOVE = 0x232,
			DROPFILES = 0x233,
			MDIREFRESHMENU = 0x234,
			POINTERDEVICECHANGE = 0x238,
			POINTERDEVICEINRANGE = 0x239,
			POINTERDEVICEOUTOFRANGE = 0x23A,
			TOUCH = 0x240,
			NCPOINTERUPDATE = 0x241,
			NCPOINTERDOWN = 0x242,
			NCPOINTERUP = 0x243,
			POINTERUPDATE = 0x245,
			POINTERDOWN = 0x246,
			POINTERUP = 0x247,
			POINTERENTER = 0x249,
			POINTERLEAVE = 0x24A,
			POINTERACTIVATE = 0x24B,
			POINTERCAPTURECHANGED = 0x24C,
			TOUCHHITTESTING = 0x24D,
			POINTERWHEEL = 0x24E,
			POINTERHWHEEL = 0x24F,
			POINTERHITTEST = 0x250,
			POINTERROUTEDTO = 0x251,
			POINTERROUTEDAWAY = 0x252,
			POINTERROUTEDRELEASED = 0x253,
			IME_SETCONTEXT = 0x281,
			IME_NOTIFY = 0x282,
			IME_CONTROL = 0x283,
			IME_COMPOSITIONFULL = 0x284,
			IME_SELECT = 0x285,
			IME_CHAR = 0x286,
			IME_REQUEST = 0x288,
			IME_KEYDOWN = 0x290,
			IME_KEYUP = 0x291,
			NCMOUSEHOVER = 0x2A0,
			MOUSEHOVER = 0x2A1,
			NCMOUSELEAVE = 0x2A2,
			MOUSELEAVE = 0x2A3,
			WTSSESSION_CHANGE = 0x2B1,
			TABLET_FIRST = 0x2C0,
			TABLET_LAST = 0x2DF,
			DPICHANGED = 0x2E0,
			DPICHANGED_BEFOREPARENT = 0x2E2,
			DPICHANGED_AFTERPARENT = 0x2E3,
			GETDPISCALEDSIZE = 0x2E4,
			CUT = 0x300,
			COPY = 0x301,
			PASTE = 0x302,
			CLEAR = 0x303,
			UNDO = 0x304,
			RENDERFORMAT = 0x305,
			RENDERALLFORMATS = 0x306,
			DESTROYCLIPBOARD = 0x307,
			DRAWCLIPBOARD = 0x308,
			PAINTCLIPBOARD = 0x309,
			VSCROLLCLIPBOARD = 0x30A,
			SIZECLIPBOARD = 0x30B,
			ASKCBFORMATNAME = 0x30C,
			CHANGECBCHAIN = 0x30D,
			HSCROLLCLIPBOARD = 0x30E,
			QUERYNEWPALETTE = 0x30F,
			PALETTEISCHANGING = 0x310,
			PALETTECHANGED = 0x311,
			HOTKEY = 0x312,
			GETSYSMENU = 0x313,
			PRINT = 0x317,
			PRINTCLIENT = 0x318,
			APPCOMMAND = 0x319,
			THEMECHANGED = 0x31A,
			CLIPBOARDUPDATE = 0x31D,
			DWMCOMPOSITIONCHANGED = 0x31E,
			DWMNCRENDERINGCHANGED = 0x31F,
			DWMCOLORIZATIONCOLORCHANGED = 0x320,
			DWMWINDOWMAXIMIZEDCHANGE = 0x321,
			DWMSENDICONICTHUMBNAIL = 0x323,
			DWMSENDICONICLIVEPREVIEWBITMAP = 0x326,
			GETTITLEBARINFOEX = 0x33F,
			HANDHELDFIRST = 0x358,
			HANDHELDLAST = 0x35F,
			AFXFIRST = 0x360,
			AFXLAST = 0x37F,
			PENWINFIRST = 0x380,
			PENWINLAST = 0x38F,
			COALESCE_FIRST = 0x390,
			COALESCE_LAST = 0x39F,
			DDE_FIRST = 0x3E0,
			DDE_INITIATE = 0x3E0,
			DDE_TERMINATE = 0x3E1,
			DDE_ADVISE = 0x3E2,
			DDE_UNADVISE = 0x3E3,
			DDE_ACK = 0x3E4,
			DDE_DATA = 0x3E5,
			DDE_REQUEST = 0x3E6,
			DDE_POKE = 0x3E7,
			DDE_EXECUTE = 0x3E8,
			DDE_LAST = 0x3E8,
			USER = 0x400,

			LVM_FIRST = 0x1000,
			LVM_GETBKCOLOR = 0x1000,
			LVM_SETBKCOLOR = 0x1001,
			LVM_GETIMAGELIST = 0x1002,
			LVM_SETIMAGELIST = 0x1003,
			LVM_GETITEMCOUNT = 0x1004,
			LVM_GETITEMA = 0x1005,
			LVM_SETITEMA = 0x1006,
			LVM_INSERTITEMA = 0x1007,
			LVM_DELETEITEM = 0x1008,
			LVM_DELETEALLITEMS = 0x1009,
			LVM_GETCALLBACKMASK = 0x100A,
			LVM_SETCALLBACKMASK = 0x100B,
			LVM_GETNEXTITEM = 0x100C,
			LVM_FINDITEMA = 0x100D,
			LVM_GETITEMRECT = 0x100E,
			LVM_SETITEMPOSITION = 0x100F,
			LVM_GETITEMPOSITION = 0x1010,
			LVM_GETSTRINGWIDTHA = 0x1011,
			LVM_HITTEST = 0x1012,
			LVM_ENSUREVISIBLE = 0x1013,
			LVM_SCROLL = 0x1014,
			LVM_REDRAWITEMS = 0x1015,
			LVM_ARRANGE = 0x1016,
			LVM_EDITLABELA = 0x1017,
			LVM_GETEDITCONTROL = 0x1018,
			LVM_GETCOLUMNA = 0x1019,
			LVM_SETCOLUMNA = 0x101A,
			LVM_INSERTCOLUMNA = 0x101B,
			LVM_DELETECOLUMN = 0x101C,
			LVM_GETCOLUMNWIDTH = 0x101D,
			LVM_SETCOLUMNWIDTH = 0x101E,
			LVM_GETHEADER = 0x101F,
			LVM_CREATEDRAGIMAGE = 0x1021,
			LVM_GETVIEWRECT = 0x1022,
			LVM_GETTEXTCOLOR = 0x1023,
			LVM_SETTEXTCOLOR = 0x1024,
			LVM_GETTEXTBKCOLOR = 0x1025,
			LVM_SETTEXTBKCOLOR = 0x1026,
			LVM_GETTOPINDEX = 0x1027,
			LVM_GETCOUNTPERPAGE = 0x1028,
			LVM_GETORIGIN = 0x1029,
			LVM_UPDATE = 0x102A,
			LVM_SETITEMSTATE = 0x102B,
			LVM_GETITEMSTATE = 0x102C,
			LVM_GETITEMTEXTA = 0x102D,
			LVM_SETITEMTEXTA = 0x102E,
			LVM_SETITEMCOUNT = 0x102F,
			LVM_SORTITEMS = 0x1030,
			LVM_SETITEMPOSITION32 = 0x1031,
			LVM_GETSELECTEDCOUNT = 0x1032,
			LVM_GETITEMSPACING = 0x1033,
			LVM_GETISEARCHSTRINGA = 0x1034,
			LVM_SETICONSPACING = 0x1035,
			LVM_SETEXTENDEDLISTVIEWSTYLE = 0x1036,
			LVM_GETEXTENDEDLISTVIEWSTYLE = 0x1037,
			LVM_GETSUBITEMRECT = 0x1038,
			LVM_SUBITEMHITTEST = 0x1039,
			LVM_SETCOLUMNORDERARRAY = 0x103A,
			LVM_GETCOLUMNORDERARRAY = 0x103B,
			LVM_SETHOTITEM = 0x103C,
			LVM_GETHOTITEM = 0x103D,
			LVM_SETHOTCURSOR = 0x103E,
			LVM_GETHOTCURSOR = 0x103F,
			LVM_APPROXIMATEVIEWRECT = 0x1040,
			LVM_SETWORKAREAS = 0x1041,
			LVM_GETSELECTIONMARK = 0x1042,
			LVM_SETSELECTIONMARK = 0x1043,
			LVM_SETBKIMAGEA = 0x1044,
			LVM_GETBKIMAGEA = 0x1045,
			LVM_GETWORKAREAS = 0x1046,
			LVM_SETHOVERTIME = 0x1047,
			LVM_GETHOVERTIME = 0x1048,
			LVM_GETNUMBEROFWORKAREAS = 0x1049,
			LVM_SETTOOLTIPS = 0x104A,
			LVM_GETITEMW = 0x104B,
			LVM_SETITEMW = 0x104C,
			LVM_INSERTITEMW = 0x104D,
			LVM_GETTOOLTIPS = 0x104E,
			LVM_SORTITEMSEX = 0x1051,
			LVM_FINDITEMW = 0x1053,
			LVM_GETSTRINGWIDTHW = 0x1057,
			LVM_GETCOLUMNW = 0x105F,
			LVM_SETCOLUMNW = 0x1060,
			LVM_INSERTCOLUMNW = 0x1061,
			LVM_GETITEMTEXTW = 0x1073,
			LVM_SETITEMTEXTW = 0x1074,
			LVM_GETISEARCHSTRINGW = 0x1075,
			LVM_EDITLABELW = 0x1076,
			LVM_SETBKIMAGEW = 0x108A,
			LVM_GETBKIMAGEW = 0x108B,
			LVM_SETSELECTEDCOLUMN = 0x108C,
			LVM_SETTILEWIDTH = 0x108D,
			LVM_SETVIEW = 0x108E,
			LVM_GETVIEW = 0x108F,
			LVM_INSERTGROUP = 0x1091,
			LVM_SETGROUPINFO = 0x1093,
			LVM_GETGROUPINFO = 0x1095,
			LVM_REMOVEGROUP = 0x1096,
			LVM_MOVEGROUP = 0x1097,
			LVM_MOVEITEMTOGROUP = 0x109A,
			LVM_SETGROUPMETRICS = 0x109B,
			LVM_GETGROUPMETRICS = 0x109C,
			LVM_ENABLEGROUPVIEW = 0x109D,
			LVM_SORTGROUPS = 0x109E,
			LVM_INSERTGROUPSORTED = 0x109F,
			LVM_REMOVEALLGROUPS = 0x10A0,
			LVM_HASGROUP = 0x10A1,
			LVM_SETTILEVIEWINFO = 0x10A2,
			LVM_GETTILEVIEWINFO = 0x10A3,
			LVM_SETTILEINFO = 0x10A4,
			LVM_GETTILEINFO = 0x10A5,
			LVM_SETINSERTMARK = 0x10A6,
			LVM_GETINSERTMARK = 0x10A7,
			LVM_INSERTMARKHITTEST = 0x10A8,
			LVM_GETINSERTMARKRECT = 0x10A9,
			LVM_SETINSERTMARKCOLOR = 0x10AA,
			LVM_GETINSERTMARKCOLOR = 0x10AB,
			LVM_SETINFOTIP = 0x10AD,
			LVM_GETSELECTEDCOLUMN = 0x10AE,
			LVM_ISGROUPVIEWENABLED = 0x10AF,
			LVM_GETOUTLINECOLOR = 0x10B0,
			LVM_SETOUTLINECOLOR = 0x10B1,
			LVM_CANCELEDITLABEL = 0x10B3,
			LVM_MAPINDEXTOID = 0x10B4,
			LVM_MAPIDTOINDEX = 0x10B5,

			CCM_FIRST = 0x2000,
			CCM_SETBKCOLOR = 0x2001,
			CCM_SETCOLORSCHEME = 0x2002,
			CCM_GETCOLORSCHEME = 0x2003,
			CCM_GETDROPTARGET = 0x2004,
			CCM_SETUNICODEFORMAT = 0x2005,
			LVM_SETUNICODEFORMAT = 0x2005,
			CCM_GETUNICODEFORMAT = 0x2006,
			LVM_GETUNICODEFORMAT = 0x2006,
			CCM_SETVERSION = 0x2007,
			CCM_GETVERSION = 0x2008,
			CCM_SETNOTIFYWINDOW = 0x2009,
			CCM_SETWINDOWTHEME = 0x200B,
			CCM_DPISCALE = 0x200C,

			APP = 0x8000
		}

		[Flags]
		public enum WPF : uint {
			SETMINPOSITION = 0x1,
			RESTORETOMAXIMIZED = 0x2,
			ASYNCWINDOWPLACEMENT = 0x4
		}

		[Flags]
		public enum WS : uint {
			BORDER = 0x00800000,
			CAPTION = 0x00C00000,
			CHILD = 0x40000000,
			CHILDWINDOW = 0x40000000,
			CLIPCHILDREN = 0x02000000,
			CLIPSIBLINGS = 0x04000000,
			DISABLED = 0x08000000,
			DLGFRAME = 0x00400000,
			GROUP = 0x00020000,
			HSCROLL = 0x00100000,
			ICONIC = 0x20000000,
			MAXIMIZE = 0x01000000,
			MAXIMIZEBOX = 0x00010000,
			MINIMIZE = 0x20000000,
			MINIMIZEBOX = 0x00020000,
			OVERLAPPED = 0x00000000,
			OVERLAPPEDWINDOW = 0x00CF0000,
			POPUP = 0x80000000,
			POPUPWINDOW = 0x80880000,
			SIZEBOX = 0x00040000,
			SYSMENU = 0x00080000,
			TABSTOP = 0x00010000,
			THICKFRAME = 0x00040000,
			TILED = 0x00000000,
			TILEDWINDOW = 0x00CF0000,
			VISIBLE = 0x10000000,
			VSCROLL = 0x00200000
		}

		[Flags]
		public enum WS_EX : uint {
			ACCEPTFILES = 0x00000010,
			APPWINDOW = 0x00040000,
			CLIENTEDGE = 0x00000200,
			COMPOSITED = 0x02000000,
			CONTEXTHELP = 0x00000400,
			CONTROLPARENT = 0x00010000,
			DLGMODALFRAME = 0x00000001,
			LAYERED = 0x00080000,
			LAYOUTRTL = 0x00400000,
			LEFT = 0x00000000,
			LEFTSCROLLBAR = 0x00004000,
			LTRREADING = 0x00000000,
			MDICHILD = 0x00000040,
			NOACTIVATE = 0x08000000,
			NOINHERITLAYOUT = 0x00100000,
			NOPARENTNOTIFY = 0x00000004,
			NOREDIRECTIONBITMAP = 0x00200000,
			OVERLAPPEDWINDOW = 0x00000300,
			PALETTEWINDOW = 0x00000188,
			RIGHT = 0x00001000,
			RIGHTSCROLLBAR = 0x00000000,
			RTLREADING = 0x00002000,
			STATICEDGE = 0x00020000,
			TOOLWINDOW = 0x00000080,
			TOPMOST = 0x00000008,
			TRANSPARENT = 0x00000020,
			WINDOWEDGE = 0x00000100
		}

		[DllImport("user32.dll", EntryPoint = "AdjustWindowRectEx",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool AdjustWindowRectEx(ref RECT lpRect, WS dwStyle, bool bMenu, WS_EX dwExStyle);

		[DllImport("user32.dll", EntryPoint = "ChangeClipboardChain",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = false)]
		public extern static bool ChangeClipboardChain(IntPtr hWnd, IntPtr hWndNext);

		[DllImport("user32.dll", EntryPoint = "ClientToScreen",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

		[DllImport("user32.dll", EntryPoint = "CloseDesktop",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool CloseDesktop(IntPtr hDesktop);

		[DllImport("user32.dll", EntryPoint = "CreateDesktop",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static IntPtr CreateDesktop(string lpszDesktop, IntPtr lpszDevice,
			IntPtr pDevmode, DF dwFlags, DESKTOP dwDesiredAccess, [Optional] IntPtr lpsa);

		[DllImport("user32.dll", EntryPoint = "DestroyIcon",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool DestroyIcon(IntPtr hIcon);

		[DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumWindowsProc lpfn,
			IntPtr lParam);
		public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "EnumThreadWindows",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool EnumThreadWindows(int dwThreadId, EnumWindowsProc lpfn,
			IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "FindWindow",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static IntPtr FindWindow([Optional] string lpClassName,
			[Optional] string lpWindowName);

		[DllImport("user32.dll", EntryPoint = "GetAncestor",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetAncestor(IntPtr hwnd, GA gAFlags);

		[DllImport("user32.dll", EntryPoint = "GetClassLong",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetClassLong(IntPtr hWnd, GCL nIndex);

		[DllImport("user32.dll", EntryPoint = "GetForegroundWindow",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", EntryPoint = "GetKeyState",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern KEYSTATERET GetKeyState(VK nVirtKey);

		[DllImport("user32.dll", EntryPoint = "GetMenu",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static IntPtr GetMenu(IntPtr hWnd);

		[DllImport("user32.dll", EntryPoint = "GetScrollInfo",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool GetScrollInfo(IntPtr hWnd, SB nBar, ref SCROLLINFO lpsi);

		[DllImport("user32.dll", EntryPoint = "GetSystemMetrics",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static int GetSystemMetrics(SM nIndex);

		[DllImport("user32.dll", EntryPoint = "GetThreadDesktop",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static IntPtr GetThreadDesktop(int dwThreadId);

		[DllImport("user32.dll", EntryPoint = "GetUserObjectInformation",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool GetUserObjectInformation(IntPtr hObj, UOI nIndex,
			[Optional] IntPtr pvInfo, int nLength, [Optional] out int lnpLengthNeeded);

		[DllImport("user32.dll", EntryPoint = "GetWindow",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetWindow(IntPtr hWnd, GW uCmd);

		[DllImport("user32.dll", EntryPoint = "GetWindowLong",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

		[DllImport("user32.dll", EntryPoint = "GetWindowPlacement",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll", EntryPoint = "GetWindowRect",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowThreadProcessId(IntPtr hWnd,
			[Optional] out int lpwdProcessId);

		[DllImport("user32.dll", EntryPoint = "IsWindowVisible",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("user32.dll", EntryPoint = "OpenDesktop",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static IntPtr OpenDesktop(string lpszDesktop, DF dwFlags, bool fInherit,
			DESKTOP dwDesiredAccess);

		[DllImport("user32.dll", EntryPoint = "OpenInputDesktop",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static IntPtr OpenInputDesktop(DF dwFlags, bool fInherit,
			DESKTOP dwDesiredAccess);

		[DllImport("user32.dll", EntryPoint = "PostMessage",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "RegisterHotKey",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool RegisterHotKey([Optional] IntPtr hWnd, int id, MOD fsModifier,
			VK vk);

		[DllImport("user32.dll", EntryPoint = "UnregisterHotKey",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool UnregisterHotKey([Optional] IntPtr hWnd, int id);

		[DllImport("user32.dll", EntryPoint = "SendInput",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint SendInput(uint nInputs, INPUTs[] pInputs, int cbSize);

		[DllImport("user32.dll", EntryPoint = "SendMessage",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessageCallback",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SendMessageCallback(IntPtr hWnd, WM Msg, IntPtr wParam,
			IntPtr lParam, SendAsyncProc lpCallback, IntPtr dwData);
		public delegate void SendAsyncProc(IntPtr hWnd, WM Msg, IntPtr dwData, IntPtr lResult);

		[DllImport("user32.dll", EntryPoint = "SendMessageTimeout",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessageTimeout(IntPtr hWnd, WM Msg, IntPtr wParam,
			IntPtr lParam, SMTO fwFlags, uint uTimeout, [Optional] out IntPtr lpdwResult);

		[DllImport("user32.dll", EntryPoint = "SetClipboardViewer",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static IntPtr SetClipboardViewer(IntPtr hWnd);

		[DllImport("user32.dll", EntryPoint = "SetForegroundWindow",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll", EntryPoint = "SetThreadDesktop",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool SetThreadDesktop(IntPtr hDesktop);

		[DllImport("user32.dll", EntryPoint = "SetWindowLong",
		ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowPos",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool SetWindowPos(IntPtr hWnd, [Optional] IntPtr hWndInsertAfter,
			int X, int Y, int cx, int cy, SWP uFlags);

		[DllImport("user32.dll", EntryPoint = "SetWinEventHook",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetWinEventHook(EVENT eventMin, EVENT eventMax,
			IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc, int idProcess, int idThread,
			WINEVENT dwFlags); 
		public delegate void WinEventProc(IntPtr hWinEventHook, EVENT @event, IntPtr hWnd,
			OBJID idObject, int idChild, int dwEventThread, int dwmsEventTime);

		[DllImport("user32.dll", EntryPoint = "ShowWindow",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool ShowWindow(IntPtr hWnd, SW nCmdShow);

		[DllImport("user32.dll", EntryPoint = "SwitchDesktop",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool SwitchDesktop(IntPtr hDesktop);

		[DllImport("user32.dll", EntryPoint = "UnhookWinEvent",
		ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr UnhookWinEvent(IntPtr hWinEventHook);

		public static class Helpers {
			public static uint AutoTimeout = 250;

			public static IntPtr SendMessageAutoTimeout(IntPtr hWnd, WM Msg, IntPtr wParam,
				IntPtr lParam) {

				if (SendMessageTimeout(hWnd, Msg, wParam, lParam, SMTO.ABORTIFHUNG, AutoTimeout,
					out IntPtr ret) == IntPtr.Zero) {

					if ((ERROR)Marshal.GetLastWin32Error() == ERROR.TIMEOUT) {
						throw new TimeoutException();
					}
					throw new Win32Exception();
				}
				return ret;
			}

			public async static Task<IntPtr> SendMessageAsync(IntPtr hWnd, WM Msg, IntPtr wParam,
				IntPtr lParam) {

				IntPtr result = IntPtr.Zero;
				bool complete = false;
				object lockObj = new object();
				if (!SendMessageCallback(hWnd, Msg, wParam, lParam, (hWnd2, Msg2, dwData, lResult) => {
					lock (lockObj) {
						result = lResult;
						complete = true;
					}
				}, IntPtr.Zero)) {
					throw new Win32Exception();
				}

				while (true) {
					lock (lockObj) {
						if (complete) {
							return result;
						}
					}
					await Task.Delay(25);
				}
			}
		}
	}
}
