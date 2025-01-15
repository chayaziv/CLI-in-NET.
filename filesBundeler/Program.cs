

using System.CommandLine;
using fib;

var rootCommand = new RootCommand();

var languageOption = new Option<string[]>(
 "--lang",
 "Programming languages to include in the bundle. Use 'all' to include all code files.")
{
    IsRequired = true,
    AllowMultipleArgumentsPerToken = true
};
languageOption.AddAlias("-l");
// Validation: Ensure specified languages are valid or 'all'
languageOption.AddValidator(result =>
{
    var values = result.GetValueOrDefault<string[]>();
    if (values == null || values.Length == 0)
    {
        result.ErrorMessage = "You must specify at least one language or use 'all'.";
        return;
    }

    // Validate languages against the allowed list
    var validLanguages = ServiceLanguage.languagesWithExtensions.Select(l => l.language).ToList();
    var invalidLanguages = values
        .Where(lang => !lang.Equals("all", StringComparison.OrdinalIgnoreCase) &&
                       !validLanguages.Contains(lang.ToLower()))
        .ToList();

    if (invalidLanguages.Any())
    {
        result.ErrorMessage = $"Invalid languages specified: {string.Join(", ", invalidLanguages)}. Allowed languages are: {string.Join(", ", validLanguages)} or 'all'.";
    }
});



var outputOption = new Option<string>(
    "--output",
    () => "bundle.txt",
    "Output file name or full path for the bundled file.");
outputOption.AddAlias("-o");

// Validation: Ensure the output path is valid and writable
outputOption.AddValidator(result =>
{
    var outputPath = result.GetValueOrDefault<string>();
    if (string.IsNullOrWhiteSpace(outputPath))
    {
        result.ErrorMessage = "The output path cannot be empty.";
        return;
    }

    try
    {
        var fullPath = Path.IsPathRooted(outputPath)
            ? outputPath
            : Path.Combine(Directory.GetCurrentDirectory(), outputPath);

        var directory = Path.GetDirectoryName(fullPath);

        // Check if directory exists
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            result.ErrorMessage = $"The directory '{directory}' does not exist.";
            return;
        }

        // Check if file is writable
        using (var stream = File.Create(fullPath, 1, FileOptions.DeleteOnClose))
        {
            // File can be created and written to
        }
    }
    catch (Exception ex)
    {
        result.ErrorMessage = $"Invalid output path: {ex.Message}";
    }
});

var noteOption = new Option<bool>(
    "--note",
    "Include the source file name and path as a comment in the bundled file.");
noteOption.AddAlias("-n");

var sortOption = new Option<string>(
    "--sort",
    () => "name",
    "Sort order for files: 'name' for alphabetical by file name, 'type' for by file extension.");
sortOption.AddAlias("-s");
sortOption.AddValidator(result =>
{
    var value = result.GetValueOrDefault<string>();
    if (value != "name" && value != "type")
    {
        result.ErrorMessage = "Invalid value for --sort. Allowed values are 'name' or 'type'.";
    }
});

var removeEmptyLinesOption = new Option<bool>(
    "--remove-empty-lines",
    "Remove empty lines from the source files.");
removeEmptyLinesOption.AddAlias("-r");

var authorOption = new Option<string>(
    "--author",
    "Include the author's name as a comment at the top of the bundled file.");
authorOption.AddAlias("-a");
authorOption.AddValidator(result =>
{
    var value = result.GetValueOrDefault<string>();
    if (!string.IsNullOrWhiteSpace(value) && value.Length > 100)
    {
        result.ErrorMessage = "Author name cannot exceed 100 characters.";
    }
});

var bundleCommand = new Command("bundle", "Bundles files into a single output file.")
{
    languageOption,
    outputOption,
    noteOption,
    sortOption,
    removeEmptyLinesOption,
    authorOption
};

bundleCommand.SetHandler((languages, output,note ,sort , removeEmptyLines , author) =>
{
    var currentDir = Directory.GetCurrentDirectory();
    var files = Directory.GetFiles(currentDir, "*", SearchOption.AllDirectories);

    Console.WriteLine("All files:");
    foreach (var file in files)
    {
        Console.WriteLine(file);
    }

    var languagesWithExtensions = ServiceLanguage.languagesWithExtensions;

    var selectedExtensions = languages.Contains("all", StringComparer.OrdinalIgnoreCase)
        ? languagesWithExtensions.SelectMany(l => l.extensions).ToArray()  // Include all file extensions
        : languagesWithExtensions
            .Where(l => languages.Contains(l.language, StringComparer.OrdinalIgnoreCase))
            .SelectMany(l => l.extensions)
            .ToArray();
    Console.WriteLine(  "exes");
    foreach (var ex in selectedExtensions)
    {
        Console.Write(ex+" ");
    }
    Console.WriteLine(  );
    // Filter files by selected extensions
    var filteredFiles = files.Where(f =>
    {
        var excludedDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "bin", "debug", "release", "obj", ".git", ".svn", ".vs"
    };
        var relativePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), f);
        var directoryParts = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        // Check if any part of the path matches an excluded directory
        if (directoryParts.Any(part => excludedDirectories.Contains(part)))
            return false;

        // Check if the file has an allowed extension
        return selectedExtensions.Any(ext => f.EndsWith(ext, StringComparison.OrdinalIgnoreCase)); 
    
    });
    
    Console.WriteLine("All files:");
    foreach (var file in filteredFiles)
    {
        Console.WriteLine(file);
    }
    // sort files
    filteredFiles = sort == "type"
        ? filteredFiles.OrderBy(f => Path.GetExtension(f)).ThenBy(f => Path.GetFileName(f)).ToList() // מיון לפי סוג ואז לפי שם
        : filteredFiles.OrderBy(f => Path.GetFileName(f)).ToList(); // מיון לפי שם בלבד

    // Combine the content of selected files
    var combinedContent = string.Empty;
    if (!string.IsNullOrEmpty(author))
    {
        combinedContent += $"// Author: {author}\n";
    }

    foreach (var file in filteredFiles)
    {
        var lines = File.ReadAllLines(file);

        if (note)
        {
            combinedContent += $"// Source: {file}\n";
        }

        foreach (var line in lines)
        {
            if (removeEmptyLines && string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            combinedContent += line + "\n";
        }

        combinedContent += "\n"; // Add space between files
    }

    // Write the combined content to the output file
    var outputPath = Path.IsPathRooted(output) ? output : Path.Combine(currentDir, output);
    try
    {
        File.WriteAllText(outputPath, combinedContent);
        Console.WriteLine($"Bundle created successfully: {outputPath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error writing to file: {ex.Message}");
    }
},

languageOption,
outputOption,
noteOption,
sortOption,
removeEmptyLinesOption,
authorOption);



rootCommand.Add(bundleCommand);




var createRspCommand = new Command("create-rsp", "Create a response file for the bundle command");

createRspCommand.SetHandler(() =>
{
    // inputs from user
    Console.WriteLine("Enter the programming languages (separated by spaces):");
    var languagesInput = Console.ReadLine();
    var languages = languagesInput?.Split(' ') ?? Array.Empty<string>();

    Console.WriteLine("Enter the output file path:");
    var output = Console.ReadLine();

    Console.WriteLine("Include source file path as comment? (yes/no):");
    var noteInput = Console.ReadLine();
    var note = noteInput?.ToLower() == "yes";

    Console.WriteLine("Enter the sort order ('name' or 'type'):");
    var sort = Console.ReadLine();

    Console.WriteLine("Remove empty lines? (yes/no):");
    var removeEmptyLinesInput = Console.ReadLine();
    var removeEmptyLines = removeEmptyLinesInput?.ToLower() == "yes";

    Console.WriteLine("Enter the author name:");
    var author = Console.ReadLine();

    // generate command line
    var commandText = $" -l {string.Join(" ", languages)} "+
                      $" {(!string.IsNullOrEmpty(output) ? $"-o {output} " : "")}" +
                      $"{(note ? "-n " : "")}" +
                      $"{(!string.IsNullOrEmpty(sort) ? $"-s {sort} " : "" )}" +
                      $"{(removeEmptyLines ? "-r " : "")}" +
                      $"{(!string.IsNullOrEmpty(author) ? $"-a \"{author}\" " : "")}";

   
    Console.WriteLine("Enter the response file name (only file name, no path):");
    var rspFileName = Console.ReadLine();
    string rspFilePath = Path.Combine(Directory.GetCurrentDirectory(), rspFileName);
    try
    {
        File.WriteAllText(rspFilePath, commandText);
        Console.WriteLine($"Response file saved to {rspFilePath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to save the response file: {ex.Message}");
    }
});

rootCommand.Add(createRspCommand);

rootCommand.InvokeAsync(args);