﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory;

namespace Strokes.BasicAchievements.NRefactory
{
    public class NRefactorySession : IDisposable
    {
        private readonly IDictionary<string, IParser> _parsers = new Dictionary<string, IParser>();

        public IParser GetParser(string filename)
        {
            IParser parser;
            if(!_parsers.TryGetValue(filename, out parser))
            {
                parser = ParserFactory.CreateParser(filename);
            }

            return parser;
        }


        public void Dispose()
        {
            //Dispose all parsers
            foreach(var parser in _parsers.Values)
            {
                parser.Dispose();
            }
        }
    }
}
