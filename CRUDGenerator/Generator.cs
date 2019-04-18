using CRUDGenerator.Extensions;
using CRUDGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDGenerator
{
    public static partial class Generator
    {
        public static List<ResultPage> GetDefaultFiles(Type type)
        {
            var className = type.Name;
            var codeFile = GenerateDefaultPage(type, className);
            var markupFile = GenerateDefaultMarkup(type, className);

            return new List<ResultPage>
            {
                codeFile,
                markupFile
            };
        }

        public static List<ResultPage> GetCreateFiles(Type type)
        {
            var className = type.Name;
            var codeFile = GenerateCreatePage(type, className);
            var markupFile = GenerateCreateMarkup(type, className);

            return new List<ResultPage>
            {
                codeFile,
                markupFile
            };
        }

        public static List<ResultPage> GetEditFiles(Type type)
        {
            var className = type.Name;
            var codeFile = GenerateEditPage(type, className);
            var markupFile = GenerateEditMarkup(type, className);

            return new List<ResultPage>
            {
                codeFile,
                markupFile
            };
        }
    }
}
