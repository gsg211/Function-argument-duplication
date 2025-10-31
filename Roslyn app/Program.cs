// See https://aka.ms/new-console-template for more information

using System.Diagnostics.SymbolStore;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslyn_app
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var tree = CSharpSyntaxTree.ParseText(@"
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
            }");

            var syntaxRoot = tree.GetRoot();
            var methodList = syntaxRoot.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();

            //Console.WriteLine(MyClass.Identifier.ToString());
            //Console.WriteLine(MethodList.ToString());

            foreach (var method in methodList)
            {
                int count = method.ParameterList.Parameters.Count;
                if (count == 1)
                {
                    var parameter = method.ParameterList.Parameters.First();
                    var parameterType=parameter.Type;
                    var newParameterList = SyntaxFactory.ParameterList(
                        SyntaxFactory.SeparatedList<ParameterSyntax>( new []
                        {
                            parameter,
                            SyntaxFactory.Parameter(SyntaxFactory.Identifier("param2")).WithType(parameterType)
                        })
                    );
                    Console.WriteLine(parameterType);
                    var newMethod = method.WithParameterList(newParameterList);
                    syntaxRoot = syntaxRoot.ReplaceNode(method, newMethod);
                }
            }
            Console.WriteLine(syntaxRoot);
        }
    }
}
