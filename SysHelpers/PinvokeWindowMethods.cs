namespace SysHelpers
{
	using System;
	using System.Runtime.InteropServices;

	public class PinvokeWindowMethods
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AllocConsole();

		[DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern uint GetDpiForWindow(IntPtr hwnd);
		[DllImport("user32")]
		public static extern int RegisterWindowMessage(string message);

		[DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
		public static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
		public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetFocus(IntPtr hWnd);

		[DllImport("User32.dll")]
		public static extern int SetForegroundWindow(IntPtr point);

	}
}