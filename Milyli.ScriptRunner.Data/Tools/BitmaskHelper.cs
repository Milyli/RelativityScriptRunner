namespace Milyli.ScriptRunner.Data.Tools
{
    public static class BitmaskHelper
    {
        public static int MakeMask(int count, int offset = 0)
        {
            int result = 0;
            for (int i = 0; i < count; i++)
            {
                result += 1 << (i + offset);
            }

            return result;
        }

        public static int RotateRight(int source, int count, int len)
        {
            if (count > len)
            {
                return RotateRight(source, count % len, len);
            }
            else
            {
                var rotateMask = MakeMask(count);
                var rotateVal = (source & rotateMask) << (len - count);
                return rotateVal + (source >> count);
            }
        }

        public static int RotateLeft(int source, int count, int len)
        {
            if (count > len)
            {
                return RotateLeft(source, count % len, len);
            }
            else
            {
                var rotateMask = MakeMask(count, len - count);
                var totalMask = ~MakeMask(32 - len, len);

                var rotateVal = (source & rotateMask) >> (len - count);
                return rotateVal + ((source << count) & totalMask);
            }
        }
    }
}
