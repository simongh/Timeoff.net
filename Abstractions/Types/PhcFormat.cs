using System.Security.Cryptography;

namespace Timeoff.Types
{
    // https://github.com/P-H-C/phc-string-format/blob/master/phc-sf-spec.md
    internal struct PhcFormat : IEquatable<byte[]>
    {
        private const int PBKDF2_ITERATIONS = 600000;
        private const int CURRENT_FORMAT = 1;

        public HashAlgorithmName HashAlgorithm { get; init; } = HashAlgorithmName.SHA256;

        public int IterationCount { get; init; } = PBKDF2_ITERATIONS;

        public int Version { get; init; } = CURRENT_FORMAT;

        public byte[] Hash { get; set; } = [];

        public byte[] Salt { get; init; } = [];

        public readonly bool ShouldUpgrade => HashAlgorithm != HashAlgorithmName.SHA256 || IterationCount != PBKDF2_ITERATIONS;

        public PhcFormat()
        { }

        public override readonly string ToString()
        {
            return $"$pbkdf2-{HashName}$v={Version}$i={IterationCount}${Convert.ToBase64String(Salt)}${Convert.ToBase64String(Hash)}";
        }

        private readonly string HashName
        {
            get
            {
                if (HashAlgorithm == HashAlgorithmName.SHA1)
                    return "sha1";

                if (HashAlgorithm == HashAlgorithmName.SHA256)
                    return "sha256";

                throw new NotSupportedException($"{HashAlgorithm.Name} is not supported");
            }
        }

        public static bool TryParse(string hash, out PhcFormat value)
        {
            if (hash == null)
            {
                value = default;
                return false;
            }

            var parts = hash.Split('$', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 3)
            {
                value = default;
                return false;
            }

            var version = CURRENT_FORMAT;
            var i = PBKDF2_ITERATIONS;
            foreach (var item in parts[1..^2])
            {
                if (item.StartsWith('v'))
                    version = int.Parse(item[2..]);
                if (item.StartsWith('i'))
                    i = int.Parse(item[2..]);
            }

            HashAlgorithmName? ha = parts[0] switch
            {
                "pbkdf2-sha1" => HashAlgorithmName.SHA1,
                "pbkdf2-sha256" => HashAlgorithmName.SHA256,
                _ => null,
            };
            if (ha == null)
            {
                value = default;
                return false;
            }

            var salt = Convert.FromBase64String(parts[^2]);
            var secret = Convert.FromBase64String(parts[^1]);

            value = new()
            {
                HashAlgorithm = ha!.Value,
                Version = version,
                IterationCount = i,
                Salt = salt,
                Hash = secret
            };

            return true;
        }

        /// <summary>
        /// Compares two byte arrays in length-constant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
        public readonly bool Equals(byte[]? other)
        {
            if (other == null)
                return false;

            var diff = (uint)Hash.Length ^ (uint)other.Length;
            for (var i = 0; i < Hash.Length && i < other.Length; i++)
                diff |= (uint)(Hash[i] ^ other[i]);

            return diff == 0;
        }
    }
}