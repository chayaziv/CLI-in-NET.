using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fib
{
    public class ServiceLanguage
    {
        public static (string language, string[] extensions)[] languagesWithExtensions =
            new (string language, string[] extensions)[]
    {
        ("py", new string[] { ".py" }),            // Python
        ("js", new string[] { ".js" }),            // JavaScript
        ("java", new string[] { ".java" }),        // Java
        ("tsx",new string []{".tsx"}),
        ("cs", new string[] { ".cs" }),            // C#
        ("cpp", new string[] { ".cpp", ".h" }),    // C++
        ("rb", new string[] { ".rb" }),            // Ruby
        ("php", new string[] { ".php" }),          // PHP
        ("go", new string[] { ".go" }),            // Go
        ("swift", new string[] { ".swift" }),      // Swift
        ("kt", new string[] { ".kt" }),            // Kotlin
        ("rs", new string[] { ".rs" }),            // Rust
        ("ts", new string[] { ".ts" }),            // TypeScript
        ("html", new string[] { ".html" }),        // HTML
        ("css", new string[] { ".css" }),          // CSS
        ("sql", new string[] { ".sql" }),          // SQL
        ("sh", new string[] { ".sh" }),            // Shell Script
        ("r", new string[] { ".r" }),              // R
        ("m", new string[] { ".m" }),              // MATLAB
        ("pl", new string[] { ".pl" }),            // Perl
        ("lua", new string[] { ".lua" }),          // Lua
        ("objc", new string[] { ".m" }),           // Objective-C
        ("hs", new string[] { ".hs" }),            // Haskell
        ("fs", new string[] { ".fs" })             // F#
    };

    }
}
