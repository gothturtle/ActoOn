using static System.Console;

namespace ActonTranslation
{
    public enum TestMode
    {
        off = 0,
        on = 1
    }

    public enum TranslationState
    {
        none = 0,
        started = 1,
        done = 2
    }

    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">file paths to translate</param>
        static void Main(string[] args)
        {
            FileUtility fileUtility = new FileUtility();

            //!!!!! Test configuratons !!!!!
            //  Below are 5 different test file configurations 
            //  Location:  'TestTranslationFiles' folder in the 

            //      file(s): Set1.txt
            // args = fileUtility.GetArgs(1); 

            //      file(s): Set1.txt, Set2.txt
            // args = fileUtility.GetArgs(2);

            //      files(s): Set1.txt, Set2.txt, Set3.txt
            // args = fileUtility.GetArgs(3);

            //      file(s): Sm1.txt, Sm2.txt, Sm3.txt, Sm4.txt
            // args = fileUtility.GetArgs(4);

            //      file(s): Sm1.txt, Sm2.txt, Sm3.txt, Sm4.txt, Sm5.txt
            // args = fileUtility.GetArgs(5);

            //  Validates the filepath arguments which should have been passed to the application =====

            //===== Testing Shortcut if you run with just one number as a param and no 
            bool usingTestMode = false;
            if(args.Length == 1 && args[0].Length == 1 && (args[0] == "1" || args[0] == "2" || args[0] == "3" || args[0] == "4" || args[0] == "5"))
            {
                args = fileUtility.GetArgs(int.Parse(args[0]));
                usingTestMode = true;
            }

            string FilesValidationErrorMessage = fileUtility.GetFilepathsErrorMessage(args);

            if (FilesValidationErrorMessage.Length == 0)
            {
                TranslationHandler th = new TranslationHandler(args);
                if (usingTestMode) th.TestModeEnum = TestMode.on;
                th.StartTranslationObjects();
            }
            else
            {
                WriteLine(FilesValidationErrorMessage);
            }

            WriteLine("Program has completed.  Press any key to exit.");
            ReadKey();
        }
    }
}
