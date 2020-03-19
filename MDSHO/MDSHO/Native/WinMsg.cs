using System;
using System.Collections.Generic;
using System.Text;

namespace MDSHO.Native
{
    /// <summary>
    /// Window Notifications. <br />
    /// https://docs.microsoft.com/en-us/windows/win32/winmsg/window-notifications
    /// </summary>
    public static class WinMsg
    {
        /// <summary>
        /// Sent to a window that the user is resizing. <br />
        /// By processing this message, an application can monitor the size and position of the drag rectangle and, if needed, change its size or position. <br />
        /// https://docs.microsoft.com/en-us/windows/desktop/winmsg/wm-sizing
        /// </summary>
        public const int WM_SIZING = 0x214;


        /// <summary>
        /// Sent to a window that the user is moving. <br />
        /// By processing this message, an application can monitor the position of the drag rectangle and, if needed, change its position. <br />
        /// https://docs.microsoft.com/en-us/windows/desktop/winmsg/wm-moving
        /// </summary>
        public const int WM_MOVING = 0x0216;


        /// <summary>
        /// Sent one time to a window, after it has exited the moving or sizing modal loop. <br />
        /// The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. <br />
        /// The operation is complete when DefWindowProc returns. <br />
        /// https://docs.microsoft.com/en-us/windows/desktop/winmsg/wm-exitsizemove
        /// </summary>
        public const int WM_EXITSIZEMOVE = 0x232;



    }
}
