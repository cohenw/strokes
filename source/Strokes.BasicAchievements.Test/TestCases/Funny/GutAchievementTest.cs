﻿using System;
using Strokes.BasicAchievements.Achievements;

namespace Strokes.BasicAchievements.Test.TestCases.Funny
{
    [ExpectUnlock(typeof(GUTAchievement))]
    [ExpectUnlock(typeof(InstantiateObjectAchievement))]
    [ExpectUnlock(typeof(CreateMethodMultipleParametersAchievement))]
    [ExpectUnlock(typeof(ThrownNotImplementedAchievement))]
    [ExpectUnlock(typeof(CreateMethodReturnDoubleAchievement))]
    [ExpectUnlock(typeof(CreateMethodReturnIntAchievement))]
    [ExpectUnlock(typeof(CreateMethodReturnBoolAchievement))]
    [ExpectUnlock(typeof(CreateMethodReturnFloatAchievement))]
    [ExpectUnlock(typeof(CreateMethodReturnStringAchievement))]
    [ExpectUnlock(typeof(CreateMethodReturnCharAchievement))]
    [ExpectUnlock(typeof(CreateMethodOneParameterAchievement))]
    [ExpectUnlock(typeof(IComparableAchievement))]
    [ExpectUnlock(typeof(ThrownExceptionAchievement))]
    public class GutAchievementTest : IDisposable, IComparable, IFormattable, IConvertible, IComparable<GutAchievementTest>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(GutAchievementTest other)
        {
            throw new NotImplementedException();
        }
    }
}