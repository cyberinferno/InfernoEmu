using System;
using System.Runtime.InteropServices;

namespace InfernoEmu
{
    public static class Crypt
    {
        [DllImportAttribute("asdecr.dll", EntryPoint = "decrypt_acl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int decrypt_acl(IntPtr acldata, int size, int header);

        [DllImportAttribute("asdecr.dll", EntryPoint = "encrypt_acl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int encrypt_acl(IntPtr acldata, int size, int header);
    }
}