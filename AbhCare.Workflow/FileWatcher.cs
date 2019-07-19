using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbhCare.Workflow
{
    public class FileWatcher
    {
        private readonly string _folder;
        private readonly string _eventName;
        private readonly string _backupFolder;

        public event EventHandler FileDetectHandler;

        public FileWatcher(string folder, string eventName, string backupFolder)
        {
            _folder = folder;
            _eventName = eventName;
            _backupFolder = backupFolder;
        }

        public void Start()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            /* Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.CreationTime
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            watcher.Path = _folder;
            watcher.Filter = "*.txt";

            //Subscribe to the Created event.
            watcher.Created += Watcher_FileCreated;

            watcher.EnableRaisingEvents = true;
        }

        private void Watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            // 檢查產生檔案的名稱（work flow id）
            var filename = Path.GetFileNameWithoutExtension(e.FullPath);
            if (filename.Length != 36)
            {
                Console.WriteLine($"{filename} is not a workflow event id");
                return;
            }

            FileDetectHandler?.Invoke(this, new FileEventArgs
            {
                WorkflowId = filename,
                EventName = _eventName,
                EventData = "Test Event Data"
            });

            // Move to Parent Event handler for not being access too fast, it whill cuase file lock error
            //MoveToBackupFolder(e.FullPath);
        }

        public void MoveToBackupFolder(string fullPath)
        {
            //while(IsFileLock(fullPath))
            //{
            //    Thread.Sleep(1000);
            //}

            var destPath = Path.Combine(_backupFolder, Path.GetFileName(fullPath));
            if (File.Exists(destPath))
                File.Delete(destPath);

            File.Move(fullPath, destPath);
        }

        private bool IsFileLock(string fullPath)
        {
            try
            {
                using (Stream stream = new FileStream("MyFilename.txt", FileMode.Open))
                {
                }
                return false;
            }
            catch
            {
                return true;
            }
        }
    }

    public class FileEventArgs : EventArgs
    {
        public string WorkflowId { get; set; }
        public string EventName { get; set; }
        public string EventData { get; set; }
    }
}
