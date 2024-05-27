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
  expect(mapper[1][3]).toBe(6.5);
  expect(mapper[3][1]).toBe(6.5);
  expect(mapper[2][4]).toBe(1);
  expect(mapper[4][2]).toBe(1);
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
