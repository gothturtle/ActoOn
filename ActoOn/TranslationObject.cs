using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Console;

namespace ActonTranslation
{
    public class TranslationObject
    {
        //===== 
        public int Id { get; set; }
        public string Path { get; set; }
        public int CurrentLine { get; set; }
        public TranslationState TranslationState { get; set; }

        private TranslationHandler Handler = null;

        public TranslationObject(int id, string path, TranslationHandler h)
        {
            this.Id = id;
            this.Path = path;
            this.Handler = h;
        }

        public async void TranslateFile()
        {
            TranslationState = TranslationState.started;

            int counter = 0;
            string line = string.Empty;

            StreamReader reader = new StreamReader(Path);
            while ((line = reader.ReadLine()) != null)
            {
                TranslationJob job = new TranslationJob
                {
                    Id = this.Id,
                    OriginalWord = line
                };
                job.LineNumber = counter++;

                job = await TranslateJob(job);

                //TODO: do as Task?
                Handler.ProcessTranslationJob(job);
            }

            reader.Close();
            reader.Dispose();

            TranslationState = TranslationState.done;

            //===== Notify the parent that we have completed our translation file =====
            Handler.TranslationObjectCompleted();
        }

        async Task<TranslationJob> TranslateJob(TranslationJob job)
        {
            string s = "";

            using (var client = new HttpClient())
            {
                try
                {
                    s = await client.GetStringAsync(this.Handler.TranslationServiceURL + job.OriginalWord);
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    WriteLine("There was a problem returning the translation for: " + job.OriginalWord + System.Environment.NewLine +
                        message + System.Environment.NewLine);
                }
            }

            string[] words = s.Split(',');

            s = (words.Length > 1) ? words[0] : job.OriginalWord;

            try//===== strip non words chars, i.e, square brackets etc coming back in the response =====
            {
                s = Regex.Replace(s, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(3.5));
            }
            catch (RegexMatchTimeoutException) { }

            job.TranslatedWord = s;

            return job;
        }
    }
}
