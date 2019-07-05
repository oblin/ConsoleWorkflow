using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbhCare.Workflow
{
    public class FileWatcher
    {
        private readonly string _folder;
        private readonly string _eventName;
        public event EventHandler FileDetectHandler;

        public FileWatcher(string folder, string eventName)
        {
            _folder = folder;
            _eventName = eventName;
        }

        public void Start()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            /* Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.CreationTime
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            watcher.Path = "d:\\Temp\\";
            watcher.Filter = "*.txt";

            //Subscribe to the Created event.
            watcher.Created += Watcher_FileCreated;

            watcher.EnableRaisingEvents = true;
        }

        private void Watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            var filename = Path.GetFileNameWithoutExtension(e.FullPath);
            if (filename.Length != 36)
            {
                Console.WriteLine($"{filename} is not a workflow event id");
                return;
            }

            FileDetectHandler?.Invoke(this, new FileEventArgs
            {
                Id = filename,
                EventName = _eventName,
                EventData = "Test Event Data"
            });
        }
    }

    public class FileEventArgs : EventArgs
    {
        public string Id { get; set; }
        public string EventName { get; set; }
        public string EventData { get; set; }
    }
}
