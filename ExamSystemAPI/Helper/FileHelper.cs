namespace ExamSystemAPI.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 生成新的文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GenerateFileName(string fileName) {
            var guid = Guid.NewGuid();
            string extension = Path.GetExtension(fileName);
            return guid + extension;
        }
    }
}
