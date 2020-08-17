using System;
using System.Diagnostics;
using System.Text;
using Tesseract;

namespace PhoneRecognizer.OCR
{
    public static class TesseractOCR
    {
        public static string RecognizeText(byte[] imageBytes)
        {
            string result = null;
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (Pix pix = Pix.LoadFromMemory(imageBytes))
                    {
                        using (var page = engine.Process(pix))
                        {
                            string text = page.GetText();
                            Trace.WriteLine($"Text (GetText): \r\n{text}");
                            result = $"Mean confidence is {page.GetMeanConfidence()}\r\nText={text.Trim()}";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }

            return result;
        }

        public static string RecognizeBlocks(byte[] imageBytes)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (Pix pix = Pix.LoadFromMemory(imageBytes))
                    {
                        using (var page = engine.Process(pix))
                        {
                            sb.AppendLine("Text (iterator):");
                            using (ResultIterator iter = page.GetIterator())
                            {
                                iter.Begin();

                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            do
                                            {
                                                if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                                {
                                                    sb.AppendLine("<BLOCK>");
                                                }

                                                sb.Append(iter.GetText(PageIteratorLevel.Word));
                                                sb.Append(" ");

                                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                                {
                                                    sb.AppendLine();
                                                }
                                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                            {
                                                sb.AppendLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                } while (iter.Next(PageIteratorLevel.Block));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }

            return sb.ToString();
        }
    }
}
