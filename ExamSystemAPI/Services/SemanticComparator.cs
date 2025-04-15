using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace ExamSystemAPI.Services
{
    public sealed class SemanticComparator : IDisposable
    {
        private const int MaxLength = 128;
        private readonly InferenceSession _session;
        private readonly Dictionary<string, int> _vocab;

        public SemanticComparator()
        {
            // 硬编码文件路径（根据实际位置调整）
            const string modelPath = @"Model/model.onnx";
            const string vocabPath = @"Model/vocab.txt";

            ValidateFileExists(modelPath);
            ValidateFileExists(vocabPath);

            _session = new InferenceSession(modelPath);
            _vocab = LoadVocabulary(vocabPath);
        }

        public float Compare(string textA, string textB)
        {
            var (inputIds, attentionMask, tokenTypeIds) = Preprocess(textA, textB);

            var inputs = new List<NamedOnnxValue>
        {
            CreateTensor("input_ids", inputIds),
            CreateTensor("attention_mask", attentionMask),
            CreateTensor("token_type_ids", tokenTypeIds)
        };

            using var results = _session.Run(inputs);
            return results[0].AsTensor<float>()[0];
        }

        #region 私有方法
        private (long[] InputIds, long[] AttentionMask, long[] TokenTypeIds) Preprocess(string textA, string textB)
        {
            // 简单分词逻辑（生产环境需优化）
            var tokensA = Tokenize(textA);
            var tokensB = Tokenize(textB);

            var allTokens = new List<string> { "[CLS]" };
            allTokens.AddRange(tokensA);
            allTokens.Add("[SEP]");
            allTokens.AddRange(tokensB);
            allTokens.Add("[SEP]");

            // 转换为ID
            var inputIds = new List<long>();
            foreach (var token in allTokens)
            {
                inputIds.Add(_vocab.TryGetValue(token, out var id) ? id : _vocab["[UNK]"]);
            }

            // 填充/截断
            inputIds = PadOrTruncate(inputIds, MaxLength, _vocab["[PAD]"]);

            // 生成其他输入
            var attentionMask = inputIds.ConvertAll(id => id != _vocab["[PAD]"] ? 1L : 0L);
            var tokenTypeIds = new List<long>();
            bool isSegmentB = false;
            foreach (var id in inputIds)
            {
                if (id == _vocab["[SEP]"]) isSegmentB = !isSegmentB;
                tokenTypeIds.Add(isSegmentB ? 1L : 0L);
            }

            return (inputIds.ToArray(), attentionMask.ToArray(), tokenTypeIds.ToArray());
        }

        private static List<string> Tokenize(string text)
            => new List<string>(text.ToLower().Split(new[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries));

        private static Dictionary<string, int> LoadVocabulary(string path)
        {
            var vocab = new Dictionary<string, int>();
            int index = 0;
            foreach (var line in File.ReadLines(path))
            {
                vocab[line.Trim()] = index++;
            }
            return vocab;
        }

        private static NamedOnnxValue CreateTensor(string name, long[] data)
            => NamedOnnxValue.CreateFromTensor(name, new DenseTensor<long>(data, new[] { 1, data.Length }));

        private static List<long> PadOrTruncate(IList<long> source, int maxLength, long padValue)
        {
            var result = new List<long>(maxLength);
            for (int i = 0; i < maxLength; i++)
            {
                result.Add(i < source.Count ? source[i] : padValue);
            }
            return result;
        }

        private static void ValidateFileExists(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"关键文件缺失: {Path.GetFullPath(path)}");
        }
        #endregion

        #region IDisposable
        private bool _disposed;
        public void Dispose()
        {
            if (_disposed) return;
            _session?.Dispose();
            _disposed = true;
        }
        #endregion
    }
}