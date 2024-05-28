// @vitest-environment nuxt
import { expect, test } from 'vitest';
import { RhymeUtils } from '~/scripts/rhymeUtils';

const utils = new RhymeUtils();

test('Has same vowel adds score', () => {
  let score = utils.scoreSyllables('IH-T', 'B-IH');
  expect(score).toBe(1);
});

test('Whole mapper', () => {
  let poem = 'F-AO-R F-EY-M\nDH-AH S-EY-M AH';
  utils.runAlgorithm(poem);
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

test('Result has correct colors', () => {
  let poem = 'F-AO-R F-EY-M\nDH-AH S-EY-M AH';
  let syllables = utils.runAlgorithm(poem);
  expect(syllables[0].color).toBe('');
  expect(syllables[1].color).toBe('red');
  expect(syllables[2].color).toBe('blue');
  expect(syllables[3].color).toBe('red');
  expect(syllables[4].color).toBe('blue');
});

test('parse syllables', () => {
  let poem = 'F-AO-R F-EY-M\nDH-AH S-EY-M AH';
  let parsedSyllables = utils.parseSyllables(poem);
  let containsNewLine = false;
  parsedSyllables.forEach((syllable: string) => {
    if (syllable.indexOf('\n') >= 0) {
      containsNewLine = true;
    }
  });
  expect(containsNewLine).toBe(false);
});
