namespace SeedApp.Common.Enums
{
    public struct DeviceSize
    {
        public static readonly int XSmall = 0;
        public static readonly int Small = 1;
        public static readonly int Medium = 2;
        public static readonly int Large = 3;
        public static readonly int XLarge = 4;
        public static readonly int XXLarge = 5;

        public int InternalValue { get; private set; }

        public static bool operator <=(DeviceSize left, int right)
        {
            return left.InternalValue <= ((DeviceSize)right).InternalValue;
        }

        public static bool operator >=(DeviceSize left, int right)
        {
            return left.InternalValue >= ((DeviceSize)right).InternalValue;
        }

        public static bool operator >(DeviceSize left, DeviceSize right)
        {
            return left.InternalValue > right.InternalValue;
        }

        public static bool operator <(DeviceSize left, DeviceSize right)
        {
            return left.InternalValue < right.InternalValue;
        }

        public static implicit operator DeviceSize(int otherType)
        {
            return new DeviceSize
            {
                InternalValue = otherType
            };
        }

        public override bool Equals(object obj)
        {
            DeviceSize otherObj = (DeviceSize)obj;
            return otherObj.InternalValue.Equals(this.InternalValue);
        }

        public override int GetHashCode()
        {
            return InternalValue.GetHashCode();
        }
    }
}