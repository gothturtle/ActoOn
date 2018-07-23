using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace ActonTranslation
{
    public class TranslationHandler
    {
        #region Fields

        private string[] filesToTranslate;
        private List<TranslationObject> translationObjectsList;
        private List<TranslationJob> translatedJobsList = new List<TranslationJob>();
        private int BatchLineNumber { get; set; }
        private Stopwatch timer = new Stopwatch();
        public TestMode TestModeEnum;

        public string TranslationServiceURL {
            get
            {
                return "https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=sv&dt=t&q=";
            }
        }

        #endregion

        #region Public Methods
        public TranslationHandler(string[] filesToTranslate)
        {
            this.filesToTranslate = filesToTranslate;
            this.translationObjectsList = new List<TranslationObject>();
        }

        public void StartTranslationObjects()
        {
            timer.Start();

            if(TestModeEnum == TestMode.on) WriteLine("Translations will now begin ...");

            foreach (string s in filesToTranslate)
                translationObjectsList.Add(new TranslationObject(translationObjectsList.Count + 1, s, this));

            Parallel.ForEach(translationObjectsList, item => item.TranslateFile());
        }

        internal void TranslationObjectCompleted()
        {
            if (TranslationsAreComplete() == true)
            {
                timer.Stop();

                TimeSpan ts = timer.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                if (TestModeEnum == TestMode.on) WriteLine("Translations files are complete, elapsed time: " + elapsedTime);

                WriteLine("Press any key to exit.");
            }
        }

        #endregion

        internal void ProcessTranslationJob(TranslationJob job)
        {
            if (TestModeEnum == TestMode.on) WriteLine("TranslationObject Id: " + job.Id.ToString() + " line#" + job.LineNumber + " word: " + job.OriginalWord + " translation: " + job.TranslatedWord);

            Task LogTask = new Task(() => { WriteLog(job.TranslatedWord); });

            Task QueTask = LogTask.ContinueWith((tx) => { translatedJobsList.Add(job); });

            Task BatchTask = QueTask.ContinueWith((tx) => { ProcessBatch(job); });

            LogTask.Start();

            Task.WaitAll(LogTask, QueTask, BatchTask);
        }

        private bool ProcessBatch(TranslationJob job)
        {
            if (translatedJobsList.Where(x => x.LineNumber == BatchLineNumber).Count() == translationObjectsList.Count)
            {
                List<TranslationJob> completedJobs = translatedJobsList.Where(x => x.LineNumber == BatchLineNumber).OrderBy(y => y.TranslatedWord).ToList();

                string batchedResult = string.Empty;

                foreach (TranslationJob j in completedJobs)
                {
                    batchedResult += j.TranslatedWord + System.Environment.NewLine;
                    translatedJobsList.Remove(j);
                }
                WriteBatch(batchedResult);
                BatchLineNumber++;
            }
            return true;
        }

        private bool TranslationsAreComplete()
        {
            if(translationObjectsList.Where(x => x.TranslationState == TranslationState.done).Count() == translationObjectsList.Count)
            {
                return true;
            }
            return false;
        }

        #region File IO Methods

        private static bool WriteBatch(string text)
        {
            using (var mutex = new Mutex(false, "BatchMutex"))
            {
                mutex.WaitOne();
                FileUtility f = new FileUtility();
                string filePath = f.GetBatchedFilepath;
                File.AppendAllText(filePath, text);
                mutex.ReleaseMutex();
            }
            return true;
        }

        private static void WriteLog(string text)
        {
            using (var mutex = new Mutex(false, "AsYouGoMutex"))
            {
                mutex.WaitOne();
                FileUtility f = new FileUtility();
                string filePath = f.GetAsYouGoFilepath;
                File.AppendAllText(filePath, text + System.Environment.NewLine);
                mutex.ReleaseMutex();
            }
        }
        #endregion
    }
}
