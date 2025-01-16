using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fib
{
    public class ServiceLanguage
    {


        public static Dictionary<string, (string[] fileExtensions, string[] ignoreFolders)> LanguagesConfig = new Dictionary<string, (string[], string[])>
        {
            { "csharp", (new string[] { ".cs" }, new string[] { "bin", "debug" }) },
            { "python", (new string[] { ".py", ".ipynb" }, new string[] { "__pycache__", ".pytest_cache", ".vscode", "env", "venv", "*.egg-info" }) },
            { "javascript", (new string[] { ".js", ".jsx" }, new string[] { "node_modules", "dist", ".vscode" }) },
            { "java", (new string[] { ".java", ".class" }, new string[] { "target", ".vscode", ".idea", "*.iml" }) },
            { "html", (new string[] { ".html", ".htm" }, new string[] { ".vscode" }) },
            { "css", (new string[] { ".css", ".scss", ".sass" }, new string[] { ".vscode" }) },
            { "cpp", (new string[] { ".cpp", ".h", ".hpp", ".c" }, new string[] { "build", ".vscode" }) },
            { "php", (new string[] { ".php" }, new string[] { "vendor", ".vscode" }) },
            { "ruby", (new string[] { ".rb" }, new string[] { ".vscode", ".bundle" }) },
            { "go", (new string[] { ".go" }, new string[] { "bin", "pkg", ".vscode" }) },
            { "typescript", (new string[] { ".ts", ".tsx" }, new string[] { "node_modules", "dist", ".vscode" }) },
            { "shell", (new string[] { ".sh" }, new string[] { ".vscode" }) },
            { "kotlin", (new string[] { ".kt", ".kts" }, new string[] { "build", ".idea", "*.iml" }) },
            { "swift", (new string[] { ".swift" }, new string[] { ".vscode", "build" }) },
            { "rust", (new string[] { ".rs" }, new string[] { "target", ".vscode" }) }

        };
      

    }
}
