﻿using Strokes.BasicAchievements.Achievements;

namespace Strokes.BasicAchievements.Test.TestCases.PrimitiveTypeUsage
{
    [ExpectUnlock(typeof(AssignInt))]
    [ExpectUnlock(typeof(AssignDouble))]
    [ExpectUnlock(typeof(AssignFloat))]
    [ExpectUnlock(typeof(AssignChar))]
    [ExpectUnlock(typeof(DeclareInt))]
    [ExpectUnlock(typeof(DeclareDouble))]
    [ExpectUnlock(typeof(DeclareFloat))]
    [ExpectUnlock(typeof(DeclareChar))]
    [ExpectUnlock(typeof(InstantiateObjectAchievement))]
    [ExpectUnlock(typeof(InlineCommentAchievement))]
    public class AssignValueToPrimitiveType
    {
        int intVar;
        float floatVar;
        double doubleVar;
        char charVar;

        public void Main()
        {
            var bla = new object();
            bla.Equals(""); //To try and provoke a response for AbstractMethodCall

            intVar = 5;
            floatVar = 5f;
            doubleVar = 4d;
            charVar = 'c';
        }
    }
}