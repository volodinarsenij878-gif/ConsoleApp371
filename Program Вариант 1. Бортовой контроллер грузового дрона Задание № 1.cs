using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program1
{
    internal class Program
    {
        private static void Main()
        { }
        public static class DroneTelemetry
        {
            // Битовые маски и сдвиги
            private const int BatteryShift = 0;      // 7 бит: 0–100%
            private const int HeightShift = 7;      // 10 бит: 0–1000 м
            private const int SpeedShift = 17;      // 7 бит: 0–120 км/ч
            private const int AccidentShift = 24;   // 1 бит: флаг аварии

            public static uint PackTelemetry(byte batteryPercent, ushort heightM, byte speedKmh, bool isAccident)
            {
                // Проверка диапазонов (можно вынести в отдельные проверки при необходимости)
                if (batteryPercent > 100 || heightM > 1000 || speedKmh > 120)
                    throw new ArgumentOutOfRangeException();

                uint packed = 0;
                packed |= ((uint)batteryPercent & 0x7F) << BatteryShift;
                packed |= ((uint)heightM & 0x3FF) << HeightShift;
                packed |= ((uint)speedKmh & 0x7F) << SpeedShift;
                packed |= (isAccident ? 1u : 0u) << AccidentShift;

                return packed;
            }

            public static void UnpackTelemetry(uint packed, out byte batteryPercent, out ushort heightM, out byte speedKmh, out bool isAccident)
            {
                batteryPercent = (byte)((packed >> BatteryShift) & 0x7F);
                heightM = (ushort)((packed >> HeightShift) & 0x3FF);
                speedKmh = (byte)((packed >> SpeedShift) & 0x7F);
                isAccident = ((packed >> AccidentShift) & 1u) == 1;
            }

            // Вычисление бита чётности только через &, ^, >>
            public static byte ComputeParityBit(uint value)
            {
                value ^= value >> 16;
                value ^= value >> 8;
                value ^= value >> 4;
                value ^= value >> 2;
                value ^= value >> 1;
                return (byte)(value & 1);
            }
        }
    }
}