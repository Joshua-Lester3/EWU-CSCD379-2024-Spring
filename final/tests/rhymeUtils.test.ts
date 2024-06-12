// @vitest-environment nuxt
import { expect, test } from 'vitest';
import { RhymeUtils } from '~/scripts/rhymeUtils';

const utils = new RhymeUtils();

test('Has same vowel adds score', () => {
  let score = utils.scoreSyllables('IH-T', 'B-IH');
  expect(score).toBe(1);
});

test('Whole mapper', async () => {
  let poemPronunciation = 'F-AO-R F-EY-M\nDH-AH S-EY-M AH';
  let poem = 'For fame\nThe same ah';
  await utils.runAlgorithm(poemPronunciation, poem);
  let mapper = utils.mapper;
  // Commenting "consonant-after check" approach out. Videos I'm taking interpretation from are using only vowels and I want to model it after that for now.
  // expect(mapper[1][3]).toBe(6.5);
  // expect(mapper[3][1]).toBe(6.5);
  expect(mapper[2][4]).toBe(1);
  expect(mapper[4][2]).toBe(1);
  for (let outerIndex = 0; outerIndex < 5; outerIndex++) {
    for (let innerIndex = 0; innerIndex < 5; innerIndex++) {
      if (
        !(outerIndex === 1 && innerIndex === 3) &&
        !(outerIndex === 3 && innerIndex === 1) &&
        !(outerIndex === 4 && innerIndex === 2) &&
        !(outerIndex === 2 && innerIndex === 4)
      ) {
        expect(mapper[outerIndex][innerIndex] <= 0).toBe(true);
      }
    }
  }
});

test('Result has correct colors', async () => {
  let poemPronunciation = 'F-AO-R F-EY-M\nDH-AH S-EY-M AH';
  let poem = 'For fame\nThe same ah';
  let words = await utils.runAlgorithm(poemPronunciation, poem);
  expect(words[0].syllables[0].color).toBe('');
  expect(words[1].syllables[0].color).toBe('');
  expect(words[2].syllables[0].color).toBe('blue');
  expect(words[3].syllables[0].color).toBe('');
  expect(words[4].syllables[0].color).toBe('blue');
});

// test('parse syllables', () => {
//   let poem = 'F-AO-R F-EY-M\nDH-AH S-EY-M AH';
//   let parsedSyllables = utils.parseSyllables(poem);
//   let containsNewLine = false;
//   parsedSyllables.forEach((syllable: string) => {
//     if (syllable.indexOf('\n') >= 0) {
//       containsNewLine = true;
//     }
//   });
//   expect(containsNewLine).toBe(false);
// });
