/*
 * Copyright (C) 2022-2025 Georgia Tech Research Institute
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * See the LICENSE file for the full license text.
*/
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace GPSSampleDecoder.Utils
{
	public sealed class EncryptionUtil
	{
      private const string salt = "slSOInkFVlwoeiSLk";
      private const string iv = "cbfknlFDLKSGzCVL";

      private EncryptionUtil()
      {
      }

      private static readonly Lazy<EncryptionUtil> lazy = new Lazy<EncryptionUtil>(() => new EncryptionUtil());

		public static EncryptionUtil Instance
      {
         get
         {
            return lazy.Value;
         }
      }

		public string Decrypt(string encrypted, string passcode)
		{
			try
			{
				string decrypted = encrypted;

				// check for a cleartext string

				if (encrypted[0] == '{')
				{
					return encrypted;
				}

				if (passcode.Length > 0)
				{
					var aesEngine = new AesEngine();
					var gcmBlockCipher = new GcmBlockCipher(aesEngine);

					var spec = new Rfc2898DeriveBytes(passcode, Encoding.UTF8.GetBytes(salt), 10000);

					byte[] key = spec.GetBytes(16);

					byte[] IV = Encoding.UTF8.GetBytes(iv);

					AeadParameters parameters = new AeadParameters(new KeyParameter(key), 128, IV, null);

					gcmBlockCipher.Init(false, parameters); // false for decryptionbyte[]

					byte[] cipherText = Convert.FromBase64String(encrypted);

					// Prepare output buffer
					byte[] plainText = new byte[gcmBlockCipher.GetOutputSize(cipherText.Length)];

					// Decrypt the data
					int len = gcmBlockCipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
					gcmBlockCipher.DoFinal(plainText, len);

					decrypted = Encoding.UTF8.GetString(plainText);
				}

				// uncompress

				byte[] bytes = Convert.FromBase64String(decrypted);
				using (var inputStream = new MemoryStream(bytes))
				using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
				using (var streamReader = new StreamReader(gZipStream))
				{
					var decompressed = streamReader.ReadToEnd();
					Console.WriteLine(decompressed);
					return decompressed;
				}
			}
			catch (InvalidCipherTextException e)
			{
				throw new CryptographicException("Decryption failed", e);
			}
		}
	}
}
