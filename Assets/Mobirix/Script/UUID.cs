using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mobirix
{
    public class UUID : MonoBehaviour
    {

        struct mobi_uuid_t
        {
            public uint time_low;
            public ushort time_mid;
            public ushort time_hi_and_version;
            public byte clock_seq_hi_and_reserved;
            public byte clock_seq_low;
            public byte[] node;        // size = 6
        }

        static private ulong time()
        {
            System.DateTime centuryBegin = new System.DateTime(1970, 1, 1);
            System.DateTime currentDate = System.DateTime.Now;


            long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;
            System.TimeSpan elapsedSpan = new System.TimeSpan(elapsedTicks);
            return (ulong)elapsedSpan.TotalSeconds;

        }

        static ulong g_nSeed = 0;
        static ushort _rand()
        {
            return (ushort)(((g_nSeed = g_nSeed * 214013 + 2531011) >> 16) & 0x7fff);
        }

        static ulong get_system_time()
        {
            System.DateTime centuryBegin = new System.DateTime(1970, 1, 1);
            System.DateTime currentDate = System.DateTime.Now;


            long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;


            ulong p_uuid_time = ((ulong)elapsedTicks * 10) + 0x01B21DD213814000;

            return p_uuid_time;
        }

        static void get_random(out byte[] p_seed)
        {
            p_seed = new byte[16];

            int i = 0;
            ushort myRand;
            do
            {
                myRand = (ushort)Random.Range((int)ushort.MinValue, (int)ushort.MaxValue);
                p_seed[i++] = (byte)(myRand & 0xFF);
                p_seed[i++] = (byte)(myRand >> 8);

            } while (i < 14);

        }

        static public string M_Create_UUID()
        {
            string p_OutUUID = "";

            ulong timestamp;
            ushort clockseq;
            mobi_uuid_t uuid;
            byte[] seed;

            timestamp = get_system_time();
            get_random(out seed);

            seed[0] |= (byte)0x01;

            g_nSeed = time();
            clockseq = _rand();

            uuid.time_low = (uint)(timestamp & 0xFFFFFFFF);
            uuid.time_mid = (ushort)((timestamp >> 32) & 0xFFFF);
            uuid.time_hi_and_version = (ushort)((timestamp >> 48) & 0xFFFF);
            uuid.time_hi_and_version |= (1 << 12);
            uuid.clock_seq_low = (byte)(clockseq & 0xFF);
            uuid.clock_seq_hi_and_reserved = (byte)((clockseq & 0x3F00) >> 8);
            uuid.clock_seq_hi_and_reserved |= 0x80;
            uuid.node = seed;

            //p_OutUUID = string.Format("%8.8X-%4.4X-%4.4X-%2.2X-%2.2X-", uuid.time_low, uuid.time_mid, uuid.time_hi_and_version, uuid.clock_seq_hi_and_reserved, uuid.clock_seq_low);
            p_OutUUID = string.Format("{0,8:X8}-{1,4:X4}-{2,4:X4}-{3,2:X2}{4,2:X2}-", uuid.time_low, uuid.time_mid, uuid.time_hi_and_version, uuid.clock_seq_hi_and_reserved, uuid.clock_seq_low);

            for (int i = 0; i < 6; i++)
            {
                p_OutUUID = string.Format("{0}{1,2:X2}", p_OutUUID, uuid.node[i]);
            }

            return p_OutUUID;
        }

    }
}