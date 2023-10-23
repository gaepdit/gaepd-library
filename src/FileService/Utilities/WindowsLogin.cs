using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace GaEpd.FileService.Utilities;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public sealed class WindowsLogin : IDisposable
{
    // ReSharper disable InconsistentNaming
    private const int LOGON32_PROVIDER_DEFAULT = 0;
    private const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
    // ReSharper restore InconsistentNaming

#pragma warning disable S125
    // private const int LOGON32_LOGON_INTERACTIVE = 2;
    // private const int LOGON32_LOGON_NETWORK = 3;
#pragma warning restore S125

    private IntPtr _mAccessToken;
    private WindowsIdentity? _identity;
    public SafeAccessTokenHandle? AccessToken => _identity?.AccessToken;

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType,
        int dwLogonProvider, ref IntPtr phToken);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern bool CloseHandle(IntPtr handle);

    public WindowsLogin(string username, string domain, string password)
    {
        if (username == string.Empty || password == string.Empty)
            _identity = WindowsIdentity.GetCurrent();
        else
            Login(username, domain, password);
    }

    private void Login(string username, string domain, string password)
    {
        if (_identity != null)
        {
            _identity.Dispose();
            _identity = null;
        }

        _mAccessToken = new IntPtr(0);
        Logout();

        _mAccessToken = IntPtr.Zero;
        bool logonSuccessful = LogonUser(username, domain, password,
            LOGON32_LOGON_NETWORK_CLEARTEXT, LOGON32_PROVIDER_DEFAULT, ref _mAccessToken);

        if (!logonSuccessful)
        {
            int error = Marshal.GetLastWin32Error();
            throw new Win32Exception(error);
        }

        _identity = new WindowsIdentity(_mAccessToken);
    }

    private void Logout()
    {
        if (_mAccessToken != IntPtr.Zero) CloseHandle(_mAccessToken);
        _mAccessToken = IntPtr.Zero;
        if (_identity == null) return;

        _identity.Dispose();
        _identity = null;
    }

    void IDisposable.Dispose()
    {
        Logout();
    }
}
