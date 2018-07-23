using System.IO;

namespace ActonTranslation
{
    public class FileUtility
    {
        private string argsNotFound = "No files to translate were given as parameters.";
        private string pathsNotFound = "One or more file paths were not found.";
        private string extensionsNotTxt = "One of the files input is not a text file.";

        public string ArgsNotFound { get { return argsNotFound; } }
        public string PathsNotFound { get { return pathsNotFound; } }
        public string ExtensionsNotTxt { get { return extensionsNotTxt; } }

        //===== Normally I wouldn't do it this way;
        //      but directions say out put to current path and I 
        //      want it to work with my tests as well.
        public string GetAsYouGoFilepath
        {
            get
            {
                return @"..\..\..\ActoOn\bin\Debug\AsYouGo.txt";
            }
        }

        public string GetBatchedFilepath
        {
            get
            {
                return @"..\..\..\ActoOn\bin\Debug\Batched.txt";
            }
        }

        public FileUtility()
        {

        }

        public string GetFilepathsErrorMessage(string[] args)
        {
            string errorMessage = string.Empty;

            if (args.Length > 0)
            {
                foreach (string path in args)//===== validate file paths =====
                {
                    if (!IsValidFilePath(path))
                    {
                        errorMessage = PathsNotFound;
                        break;
                    }
                    else if (!IsValidFileExtension(path))//===== validate file extensions =====
                    {
                        errorMessage = ExtensionsNotTxt;
                        break;
                    }
                }
            }
            else
            {
                errorMessage = ArgsNotFound;
            }
            return errorMessage;
        }

        public bool IsValidFilePath(string path)
        {
            FileInfo f = new FileInfo(path);
            if (f == null) return false;
            else
                return f.Exists;
        }

        public bool IsValidFileExtension(string path)
        {
            string ext = Path.GetExtension(path);
            switch (ext.ToLower())
            {
                case ".txt":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Idea here is to easily allow us to inject some file arguments to test files contained in the test project 
        /// </summary>
        /// <param name="testNumber"></param>
        /// <returns></returns>
        public string[] GetArgs(int testNumber = 4)
        {
            if (testNumber > 5) testNumber = 5;

            string[] args = new string[testNumber];

            switch (testNumber)
            {
                    //============ 50 lines per file =====
                case 1:
                    args[0] = @"..\..\..\UnitTests\TestTranslationFiles\Set1.txt";
                    break;
                case 2:
                    args[0] = @"..\..\..\UnitTests\TestTranslationFiles\Set1.txt";
                    args[1] = @"..\..\..\UnitTests\TestTranslationFiles\Set2.txt";
                    break;
                case 3:
                    args[0] = @"..\..\..\UnitTests\TestTranslationFiles\Set1.txt";
                    args[1] = @"..\..\..\UnitTests\TestTranslationFiles\Set2.txt";
                    args[2] = @"..\..\..\UnitTests\TestTranslationFiles\Set3.txt";
                    break;

                    //============ 5 lines per file =====
                case 4:
                    args[0] = @"..\..\..\UnitTests\TestTranslationFiles\Sm1.txt";
                    args[1] = @"..\..\..\UnitTests\TestTranslationFiles\Sm2.txt";
                    args[2] = @"..\..\..\UnitTests\TestTranslationFiles\Sm3.txt";
                    args[3] = @"..\..\..\UnitTests\TestTranslationFiles\Sm4.txt";
                    break;
                case 5:
                    args[0] = @"..\..\..\UnitTests\TestTranslationFiles\Sm1.txt";
                    args[1] = @"..\..\..\UnitTests\TestTranslationFiles\Sm2.txt";
                    args[2] = @"..\..\..\UnitTests\TestTranslationFiles\Sm3.txt";
                    args[3] = @"..\..\..\UnitTests\TestTranslationFiles\Sm4.txt";
                    args[4] = @"..\..\..\UnitTests\TestTranslationFiles\Sm5.txt";
                    break;
            }
            return args;
        }
    }
}
