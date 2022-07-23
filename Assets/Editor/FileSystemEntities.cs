using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSystemEntities
{
    public abstract class FileSystemEntity
    {
        public string Path { private set; get; }
        public string Name 
        { 
            get
            {
                var path = Path;
                if (path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                {
                    path = path.Remove(path.Length - 1);
                }

                var separatorIndex = path.LastIndexOf(System.IO.Path.DirectorySeparatorChar);

                if(separatorIndex < 0)
                {
                    return string.Empty;
                }

                return path.Substring(separatorIndex + 1);

            } 
        }

        public int IndentLevel { get; } = 0;

        protected FileSystemEntity(string path, int indentLevel = 0)
        {
            Path = path;
            IndentLevel = indentLevel;
        }

    }

    public class Folder : FileSystemEntity
    {
        public bool IsFolded { get; set; } = false;

        public Folder(string path, int indentLevel = 0) : base(path, indentLevel)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Path is not a directory");
            }
        }
    }

    public class File : FileSystemEntity
    {
        public File(string path, int indentLevel = 0) : base(path, indentLevel)
        {
            if (!System.IO.File.Exists(path))
            {
                throw new ArgumentException("File is not a directory");
            }
        }
    }

    public class FileSystemTree
    {
        public string RootFolder { private set; get; }
        public Folder Root => GetFolder(RootFolder);

        private Dictionary<string, Tuple<FileSystemEntity, List<FileSystemEntity>>> _fileSystemTree = new Dictionary<string, Tuple<FileSystemEntity, List<FileSystemEntity>>>();

        public FileSystemTree(string rootFolder)
        {
            RootFolder = rootFolder ?? throw new ArgumentNullException(nameof(rootFolder));
            RootFolder = Path.GetFullPath(rootFolder);

            if (!Directory.Exists(RootFolder))
            {
                throw new ArgumentException("Root Folder is not a directory!");
            }

            GenerateTreeStructure();
        }

        public IReadOnlyList<FileSystemEntity> GetSubItems(string path)
        {
            if (!_fileSystemTree.TryGetValue(path, out var subItems))
            {
                throw new Exception("Path is invalid!");
            }
            return subItems.Item2;
        }

        public Folder GetFolder(string path)
        {
            if (!_fileSystemTree.TryGetValue(path, out var subItems))
            {
                throw new Exception("Path is invalid!");
            }

            if (!(subItems.Item1 is Folder folder))
            {
                throw new Exception("Path is not a folder!");
            }

            return folder;
        }

        public void GenerateTreeStructure()
        {
            _fileSystemTree.Clear();

            Queue<Folder> queue = new Queue<Folder>();
            queue.Enqueue(new Folder(RootFolder));

            while (queue.Count > 0)
            {
                var folder = queue.Dequeue();

                var subFiles = Directory.EnumerateFiles(folder.Path, "*.unity").Select(subFile => new File(subFile, folder.IndentLevel + 1));
                var subFolders = Directory.EnumerateDirectories(folder.Path).Select(subFolder => new Folder(subFolder, folder.IndentLevel + 1));

                var subItems = new List<FileSystemEntity>(subFiles.Count() + subFolders.Count());

                subItems.AddRange(subFiles);
                subItems.AddRange(subFolders);

                _fileSystemTree.Add(folder.Path, new Tuple<FileSystemEntity, List<FileSystemEntity>>(folder, subItems));

                foreach (var subfolder in subFolders)
                {
                    queue.Enqueue(subfolder);
                }

            }
        }

    }

}