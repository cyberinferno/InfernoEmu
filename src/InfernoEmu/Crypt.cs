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

        public static byte[] Decrypt(byte[] packet)
        {
            var length = packet.Length;
            IntPtr unmanagedPointer = Marshal.AllocHGlobal(length);
            Marshal.Copy(packet, 0, unmanagedPointer, length);
            Crypt.decrypt_acl(unmanagedPointer, length, 0);
            Marshal.Copy(unmanagedPointer, packet, 0, length);
            Marshal.FreeHGlobal(unmanagedPointer);
            return packet;
        }

        public static byte[] Encrypt(byte[] packet)
        {
            var length = packet.Length;
            IntPtr unmanagedPointer = Marshal.AllocHGlobal(length);
            Marshal.Copy(packet, 0, unmanagedPointer, length);
            Crypt.encrypt_acl(unmanagedPointer, length, 0);
            Marshal.Copy(unmanagedPointer, packet, 0, length);
            Marshal.FreeHGlobal(unmanagedPointer);
            return packet;
        }
    }
}