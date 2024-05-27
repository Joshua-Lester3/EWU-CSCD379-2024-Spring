export class RhymeUtils {
  public vowels: Array<string> = [
    'AA',
    'AE',
    'AH',
    'AO',
    'AW',
    'AY',
    'EH',
    'ER',
    'EY',
    'IH',
    'IY',
    'OW',
    'OY',
    'UH',
    'UW',
  ];

  public consonants: Array<string> = [
    'B',
    'CH',
    'D',
    'DH',
    'F',
    'G',
    'HH',
    'JH',
    'K',
    'L',
    'M',
    'N',
    'NG',
    'P',
    'R',
    'S',
    'SH',
    'T',
    'TH',
    'V',
    'W',
    'Y',
    'Z',
    'ZH',
  ];

  public colors: Array<string> = ['red', 'blue', 'green', 'pink'];

  public mapper: Array<Array<number>> = new Array<Array<number>>();

  public getWordToRhyme(textarea: string) {
    let newlineIndex = textarea.length;
    while (textarea[newlineIndex - 1] !== '\n' && newlineIndex - 1 >= 0) {
      newlineIndex--;
    }
    if (textarea[--newlineIndex] === '\r') {
      newlineIndex--;
    }
    let spaceIndex = newlineIndex;
    while (spaceIndex - 1 >= 0 && textarea[spaceIndex - 1] !== ' ') {
      spaceIndex--;
    }
    return textarea.substring(spaceIndex, newlineIndex);
  }

  public runAlgorithm(poemPronunciation: string) {
    let syllables: Array<string> = this.parseSyllables(poemPronunciation);
    this.mapper = new Array<number>(syllables.length)
      .fill(0)
      .map(() => new Array<number>(syllables.length).fill(0));

    // Make sure syllables don't rhyme with theirself
    for (let index = 0; index < syllables.length; index++) {
      this.mapper[index][index] = -1;
    }

    for (let outerIndex = 0; outerIndex < syllables.length; outerIndex++) {
      for (let innerIndex = 0; innerIndex < syllables.length; innerIndex++) {
        if (outerIndex !== innerIndex) {
          let score = this.scoreSyllables(
            syllables[outerIndex],
            syllables[innerIndex]
          );
          this.mapper[outerIndex][innerIndex] = score;
          this.mapper[innerIndex][outerIndex] = score;
        }
      }
    }

    let result = new Array<Syllable>();
    for (let index = 0; index < syllables.length; index++) {
      let syllable = this.mapper[0][index];
    }
  }

  public parseSyllables(poemPronunciation: string) {
    let lines = poemPronunciation.split('\n');
    let syllables = new Array<string>();
    lines.forEach(line => {
      let lineOfSyllables = line.split(' ');
      lineOfSyllables.forEach(syllable => {
        syllables.push(syllable);
      });
    });
    return syllables;
  }

  public scoreSyllables(syllableOne: string, syllableTwo: string): number {
    let score = 0;
    let phonemesOne = syllableOne.split('-');
    let phonemesTwo = syllableTwo.split('-');
    let foundVowelIndexes = this.hasSameVowel(phonemesOne, phonemesTwo);
    if (foundVowelIndexes.exists) {
      score += 1;
      if (
        phonemesOne.length > foundVowelIndexes.indexOne + 1 &&
        phonemesTwo.length > foundVowelIndexes.indexTwo + 1 &&
        phonemesOne[foundVowelIndexes.indexOne + 1] ===
          phonemesTwo[foundVowelIndexes.indexTwo + 1]
      ) {
        score += 5.5;
      }
    }

    return score;
  }

  // TODO: figure out how to choose colors - my idea is to add what the rhyme is to the
  // rhymes field in each indexpair (it's an array because it can have multiple rhymes)
  // you have to use the whole mapper because each row contains different possible matches
  // look for the highest score for an individual column and make match it to the other one
  // use the column/row indices to figure out what they match to

  private hasSameVowel(
    phonemesOne: Array<string>,
    phonemesTwo: Array<string>
  ): IndexPair {
    let pair = new IndexPair();
    let vowelOne = 'PLACEHOLDER';
    let indexOne;
    for (indexOne = 0; indexOne < phonemesOne.length; indexOne++) {
      if (this.vowels.indexOf(phonemesOne[indexOne]) > 0) {
        vowelOne = phonemesOne[indexOne];
        break;
      }
    }
    let indexTwo = phonemesTwo.indexOf(vowelOne);
    if (indexTwo >= 0) {
      pair.indexOne = indexOne;
      pair.indexTwo = indexTwo;
    }
    return pair;
  }
}

class Syllable {
  public syllable: string;
  public color: string;

  constructor(syllable: string, color: string) {
    this.syllable = syllable;
    this.color = color;
  }
}

class IndexPair {
  public indexOne: number = -1;
  public indexTwo: number = -1;
  public rhymes: Array<string> = new Array<string>();
  public get exists(): boolean {
    return this.indexOne >= 0;
  }
}

export enum AlgorithmType {
  PerfectRhyme,
  ChooseRhyme,
}
