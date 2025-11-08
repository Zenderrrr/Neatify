using System.Security.Cryptography;
using System.Text;

class Neatify
{
    private static readonly Dictionary<string, string> FileTypes = new()
{
    // Music / Audio
    { "mp3", "Music" }, { "wav", "Music" }, { "flac", "Music" }, { "aac", "Music" },
    { "ogg", "Music" }, { "m4a", "Music" }, { "wma", "Music" },

    // Videos
    { "mp4", "Videos" }, { "mkv", "Videos" }, { "avi", "Videos" }, { "mov", "Videos" },
    { "wmv", "Videos" }, { "flv", "Videos" }, { "webm", "Videos" }, { "m4v", "Videos" },

    // Images
    { "jpg", "Images" }, { "jpeg", "Images" }, { "png", "Images" }, { "gif", "Images" },
    { "bmp", "Images" }, { "tiff", "Images" }, { "svg", "Images" }, { "webp", "Images" },

    // Documents
    { "pdf", "Documents" }, { "doc", "Documents" }, { "docx", "Documents" }, { "xls", "Documents" },
    { "xlsx", "Documents" }, { "ppt", "Documents" }, { "pptx", "Documents" }, { "txt", "Documents" },
    { "rtf", "Documents" }, { "odt", "Documents" },

    // Compressed / Archives
    { "zip", "Archives" }, { "rar", "Archives" }, { "7z", "Archives" }, { "tar", "Archives" },
    { "gz", "Archives" }, { "bz2", "Archives" },

    // Subtitles
    { "srt", "Subtitles" }, { "sub", "Subtitles" }, { "ass", "Subtitles" },

    // Scripts / Code
    { "js", "Code" }, { "html", "Code" }, { "css", "Code" }, { "py", "Code" },
    { "cs", "Code" }, { "cpp", "Code" }, { "java", "Code" }, { "json", "Code" },
    { "xml", "Code" }, { "bat", "Scripts" }, { "sh", "Scripts" },

    // Logs
    { "log", "Logs" }, { "err", "Logs" },

    // Others
    { "iso", "DiskImages" }, { "exe", "Programs" }, { "dll", "Programs" },
    { "apk", "Programs" }
};


    private static string SourcePath = "";
    private static string TargetPath = "";
    private static string LogFile = "";

    private static readonly Dictionary<string, int> Summary = new()
    {
        { "Moved", 0 },
        { "Duplicates", 0 },
        { "Errors", 0 },
        { "Skipped", 0 }
    };

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        do
        {
            Console.Write("Please enter a valid source folder for your media: ");
            SourcePath = Console.ReadLine()?.Trim() ?? "";
            if (!Directory.Exists(SourcePath))
            {
                Console.WriteLine("Invalid path. Please try again...");
            }
        } while (!Directory.Exists(SourcePath));

        TargetPath = Path.Combine(SourcePath, "Media-Organized");
        LogFile = Path.Combine(SourcePath, "MediaOrganizer.log");

        if (!Directory.Exists(TargetPath))
        {
            Directory.CreateDirectory(TargetPath);
            WriteLog($"Created target folder: {TargetPath}");
        }

        WriteLog("\n=== Sorting started ===");
        OrganizeMedia();
        WriteLog("=== Sorting completed ===\n");

        Console.WriteLine("\nStatistics:");
        foreach (var entry in Summary)
            Console.WriteLine($"{entry.Key,-15}: {entry.Value}");
    }

    private static void WriteLog(string message)
    {
        string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        Console.WriteLine(entry);
        File.AppendAllText(LogFile, entry + Environment.NewLine, Encoding.UTF8);
    }

    private static string? GetFileHashSHA256(string filePath)
    {
        try
        {
            using var sha256 = SHA256.Create();
            using var stream = File.OpenRead(filePath);
            var hashBytes = sha256.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
        catch
        {
            WriteLog($"Error calculating hash: {filePath}");
            Summary["Errors"]++;
            return null;
        }
    }

    private static void OrganizeMedia()
    {
        var hashTable = new Dictionary<string, string>();
        var files = Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            try
            {
                string ext = Path.GetExtension(file).TrimStart('.').ToLower();
                string category = FileTypes.ContainsKey(ext) ? FileTypes[ext] : "Other";
                int year = File.GetCreationTime(file).Year;
                string destFolder = Path.Combine(TargetPath, category, year.ToString());

                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                    WriteLog($"Created folder: {destFolder}");
                }

                string? fileHash = GetFileHashSHA256(file);
                if (fileHash == null) continue;

                if (hashTable.ContainsKey(fileHash))
                {
                    WriteLog($"Duplicate removed: {file} (duplicate of {hashTable[fileHash]})");
                    File.Delete(file);
                    Summary["Duplicates"]++;
                }
                else
                {
                    hashTable[fileHash] = file;
                    string destination = Path.Combine(destFolder, Path.GetFileName(file));

                    if (File.Exists(destination))
                    {
                        string baseName = Path.GetFileNameWithoutExtension(file);
                        string extName = Path.GetExtension(file);
                        int counter = 1;
                        string newName;

                        do
                        {
                            newName = $"{baseName}_v{counter}{extName}";
                            destination = Path.Combine(destFolder, newName);
                            counter++;
                        } while (File.Exists(destination));

                        WriteLog($"Renamed due to conflict: {Path.GetFileName(file)} -> {Path.GetFileName(destination)}");
                    }

                    File.Move(file, destination);
                    WriteLog($"Moved: {file} -> {destination}");
                    Summary["Moved"]++;
                }
            }
            catch (Exception ex)
            {
                WriteLog($"Error processing file {file}: {ex.Message}");
                Summary["Errors"]++;
            }
        }
    }
}
