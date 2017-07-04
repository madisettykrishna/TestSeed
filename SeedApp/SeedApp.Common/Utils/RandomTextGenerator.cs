using System;
using System.Text;
using SeedApp.Common.Enums;

namespace SeedApp.Common.Utils
{
	public class RandomTextGenerator
	{
		private static readonly Random Rand = new Random();

		public static string GenerateRandomString(int length)
		{
			return GenerateRandomString(RandomStringMode.AlphaNumericLowerCaseUpperCase, length, length);
		}

		public static string GenerateRandomString(RandomStringMode mode, int length)
		{
			return GenerateRandomString(mode, length, length);
		}

		public static string GenerateRandomString(RandomStringMode mode, int minLength, int maxLength)
		{
			StringBuilder retVal = new StringBuilder();
			int stringLen = Rand.Next(minLength, maxLength);

			int asciiBase = -1;
			int sizeOfGenerationSpace = -1;
			int randomNum = -1;

			for (int i = 0; i < stringLen; i++)
			{
				// 48-57: 0-9, 65-90: A-Z, 97-122: a-z
				switch (mode)
				{
					case RandomStringMode.Numeric:
						asciiBase = 48;
						sizeOfGenerationSpace = 10;
						retVal.Append((char)Rand.Next(asciiBase, asciiBase + sizeOfGenerationSpace));
						break;
					case RandomStringMode.AlphaLowerCase:
						asciiBase = 97;
						sizeOfGenerationSpace = 26;
						retVal.Append((char)Rand.Next(asciiBase, asciiBase + sizeOfGenerationSpace));
						break;
					case RandomStringMode.AlphaLowerCaseUpperCase:
						asciiBase = 65;
						sizeOfGenerationSpace = 52;
						randomNum = Rand.Next(asciiBase, asciiBase + sizeOfGenerationSpace);
						if (randomNum > 90)
							randomNum += 6;

						retVal.Append((char)randomNum);
						break;
					case RandomStringMode.AlphaNumericLowerCase:
						asciiBase = 48;
						sizeOfGenerationSpace = 36;
						randomNum = Rand.Next(asciiBase, asciiBase + sizeOfGenerationSpace);
						if (randomNum > 57)
							randomNum += 39;

						retVal.Append((char)randomNum);
						break;
					case RandomStringMode.AlphaNumericLowerCaseUpperCase:
						asciiBase = 48;
						sizeOfGenerationSpace = 62;
						randomNum = Rand.Next(asciiBase, asciiBase + sizeOfGenerationSpace);
						if (randomNum <= 57)
						{
							// do not rebase the number in this case
						}
						else if (randomNum > 57 && randomNum < 84)
							randomNum += 7;
						else
							randomNum += 13;

						retVal.Append((char)randomNum);
						break;
					default:
						throw new ArgumentException("Invalid RandomStringMode specified");
				}
			}

			return retVal.ToString();
		}

		public static string GenerateRandomEmailAddress()
		{
			return GenerateRandomString(RandomStringMode.AlphaLowerCase, 10, 15) + "@" +
				   GenerateRandomString(RandomStringMode.AlphaLowerCase, 10, 15) + ".com";
		}
	}
}
