﻿using System.Collections.Generic;
using System.Linq;
using Strokes.BasicAchievements.NRefactory;
using Strokes.Core;
using ICSharpCode.NRefactory.CSharp;

namespace Strokes.BasicAchievements.Achievements
{
    [AchievementDescriptor("{E61CFC56-7F74-48B9-A19A-FB414D35CA6B}", "@WhileLoopAchievementName",
        AchievementDescription = "@WhileLoopAchievementDescription",
        AchievementCategory = "@ProgramFlow")]
    public class WhileLoopAchievement : NRefactoryAchievement
    {
        protected override AbstractAchievementVisitor CreateVisitor(DetectionSession detectionSession)
        {
            return new Visitor();
        }

        private class Visitor : AbstractAchievementVisitor
        {
            public override object VisitDoWhileStatement(DoWhileStatement doWhileStatement, object data)
            {
                UnlockWith(doWhileStatement);

                return base.VisitDoWhileStatement(doWhileStatement, data);
            }

            public override object VisitWhileStatement(WhileStatement whileStatement, object data)
            {
                UnlockWith(whileStatement);

                return base.VisitWhileStatement(whileStatement, data);
            }
        }
    }
}