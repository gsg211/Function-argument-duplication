// See https://aka.ms/new-console-template for more information

using System.Diagnostics.SymbolStore;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslyn_app
{
    class Program
    {
        static public String DuplicateParameters(String code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var syntaxRoot = tree.GetRoot();
            var newSyntaxRoot = syntaxRoot;
            var methodList = syntaxRoot.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();

            var newMethodList =new List<MethodDeclarationSyntax>();


            foreach (var method in methodList)
            {
                if (method.ParameterList.Parameters.Count == 1)
                {
                    var parameter = method.ParameterList.Parameters.First();
                    var parameterType = parameter.Type;
                    var newParameterList = SyntaxFactory.ParameterList(
                        SyntaxFactory.SeparatedList<ParameterSyntax>(new[]
                        {
                            parameter,
                            SyntaxFactory.Parameter(SyntaxFactory.Identifier("param2")).WithType(parameterType)
                        })
                    );
                    Console.WriteLine(parameterType);
                    var newMethod = method.WithParameterList(newParameterList);
                    newMethodList.Add(newMethod);
                }
                else
                {
                    newMethodList.Add(method);
                }
            }

            newSyntaxRoot = newSyntaxRoot.ReplaceNodes(
                methodList, 
                (original, rewritten) => newMethodList[methodList.IndexOf(original)]
            );
            return newSyntaxRoot.ToString();
        }
        
        static void Main(string[] args)
        {
            var code = @"
            public class MyClass
            {
                public bool MyMethod(int x, int y)
                {
                    return true;
                }
                public int MyMethod2(int x)
                {
                    return 23;
                }
                public void MyMethod3(bool isSorted)
                {
                    return 23;
                }
            }";

            
            
            Console.WriteLine(DuplicateParameters(code));
        }
    }
}
