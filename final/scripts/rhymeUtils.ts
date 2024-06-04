import Axios from 'axios';

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

  public colors: Array<string> = [
    'red',
    'blue',
    'green',
    'pink',
    'brown',
    'yellow',
    'orange',
  ];

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

  public async runAlgorithm(
    poemPronunciation: string,
    poem: string
  ): Promise<Array<Word>> {
    let words: Array<Array<string>> = this.parseSyllables(poemPronunciation);
    let syllablesCount = 0;
    words.forEach(word => {
      syllablesCount += word.length;
    });
    this.mapper = new Array<number>(syllablesCount)
      .fill(0)
      .map(() => new Array<number>(syllablesCount).fill(0));
    let rhymeCountMapper = new Array<number>(syllablesCount)
      .fill(0)
      .map(() => new Array<number>());

    // Make sure syllables don't rhyme with theirself
    for (let index = 0; index < syllablesCount; index++) {
      this.mapper[index][index] = -1;
    }

    for (let outerIndex = 0; outerIndex < syllablesCount; outerIndex++) {
      for (let innerIndex = 0; innerIndex < syllablesCount; innerIndex++) {
        if (outerIndex !== innerIndex) {
          let score = this.scoreSyllables(
            this.getSyllableByIndex(outerIndex, words),
            this.getSyllableByIndex(innerIndex, words)
          );
          if (score > 0) {
            rhymeCountMapper[outerIndex].push(innerIndex);
          }
          this.mapper[outerIndex][innerIndex] = score;
          this.mapper[innerIndex][outerIndex] = score;
        }
      }
    }

    let result = new Array<Syllable>();
    let colorIndex = 0;
    for (let index = 0; index < syllablesCount; index++) {
      let color = '';
      let syllableString = this.getSyllableByIndex(index, words);
      if (rhymeCountMapper[index].length > 0) {
        if (index < rhymeCountMapper[index][0]) {
          color = this.colors[colorIndex];
          colorIndex++;
        } else {
          color = result[rhymeCountMapper[index][0]].color;
        }
      }
      let syllable = new Syllable(syllableString, color);
      result.push(syllable);
    }
    return await this.convertSyllablesToWords(result, words, poem);
  }

  public async convertSyllablesToWords(
    syllables: Array<Syllable>,
    words: Array<Array<string>>,
    poem: string
  ): Promise<Array<Word>> {
    let wordsResult = new Array<Word>();
    let poemWords = poem.split(' ');
    let syllablesCount = 0;
    for (let i = 0; i < words.length; i++) {
      let word = words[i];
      let plainTextWord = poemWords[i];
      let plainTextSyllables: string[] = [];
      try {
        let url = `word/pronunciationToPlain?word=${plainTextWord}`;
        let response = await Axios.get(url);
        plainTextSyllables = response.data;
      } catch (error) {
        console.error('Error getting rhyme scheme', error);
      }
      if (plainTextSyllables.length != word.length) {
        wordsResult.push(new Word([new Syllable(plainTextWord, '')]));
      } else {
        let syllablesInWord = new Array<Syllable>();
        for (
          let syllableIndex = 0;
          syllableIndex < plainTextSyllables.length;
          syllableIndex++
        ) {
          if (syllableIndex > 0) {
            syllablesCount++;
          }
          syllables[syllablesCount].syllable =
            plainTextSyllables[syllableIndex];
          syllablesInWord.push(syllables[syllablesCount]);
        }
        wordsResult.push(new Word(syllablesInWord));
      }
      syllablesCount++;
    }

    return wordsResult;
  }

  public getSyllableByIndex(
    index: number,
    words: Array<Array<string>>
  ): string {
    let count = 0;
    for (let loopIndex = 0; loopIndex < words.length; loopIndex++) {
      let word = words[loopIndex];
      for (let x = 0; x < word.length; x++) {
        if (count === index) {
          return word[x];
        }
        count++;
      }
    }
    throw Error('index greater than or equal to number of syllables in words');
  }

  public parseSyllables(poemPronunciation: string) {
    let lines = poemPronunciation.split('\n').filter(s => s.trim() !== '');
    let syllables = new Array<Array<string>>();
    lines.forEach(line => {
      let lineOfWords = line.split('/').filter(s => s.trim() !== '');
      lineOfWords.forEach(word => {
        let syllablesInWord = word
          .trim()
          .split(' ')
          .filter(s => s.trim() !== '');
        syllables.push(syllablesInWord);
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
      // Commenting "consonant-after check" approach out. Videos I'm taking interpretation from are using only vowels and I want to model it after that for now.
      // if (
      //   phonemesOne.length > foundVowelIndexes.indexOne + 1 &&
      //   phonemesTwo.length > foundVowelIndexes.indexTwo + 1 &&
      //   phonemesOne[foundVowelIndexes.indexOne + 1] ===
      //     phonemesTwo[foundVowelIndexes.indexTwo + 1]
      // ) {
      //   score += 5.5;
      // }
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
      if (this.vowels.indexOf(phonemesOne[indexOne]) >= 0) {
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

export class Word {
  public syllables: Syllable[];

  constructor(syllables: Syllable[]) {
    this.syllables = syllables;
  }
}

export class Syllable {
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
