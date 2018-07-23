using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ActonTranslation;

namespace Tests
{
    [TestClass]
    public class TranslationTests
    {
        [TestMethod]
        public void InvalidArgs()
        {
            // Arrange
            FileUtility f = new FileUtility();

            string[] args = new string[] { };

            // Act
            string errorMessage = f.GetFilepathsErrorMessage(args);

            // Assert
            Assert.AreEqual(errorMessage, f.ArgsNotFound);
        }

        [TestMethod]
        public void InvalidFileExtension()
        {
            // Arrange
            FileUtility f = new FileUtility();

            // Act
            bool errorBool = f.IsValidFileExtension("asdf.tx");

            // Assert
            Assert.AreEqual(errorBool, false);
        }

        [TestMethod]
        public void InvalidPath()
        {
            // Arrange
            FileUtility f = new FileUtility();

            // Act
            bool error = f.IsValidFilePath("asdf.txt");

            // Assert
            Assert.AreEqual(error, false);
        }

        [TestMethod]
        public void Translates5SmallTestFilesWithoutAnException()
        {
            // Arrange
            FileUtility f = new FileUtility();
            string[] args = f.GetArgs(5);
            bool hadError = false;

            // Act
            try
            {
                TranslationHandler th = new TranslationHandler(args);
                th.StartTranslationObjects();
            }
            catch(Exception ex)
            {
                hadError = true;
            }

            // Assert            
            Assert.AreEqual(hadError, false);
        }
    }
}
