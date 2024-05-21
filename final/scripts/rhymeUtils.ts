export class RhymeUtils {
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
}

export enum AlgorithmType {
  PerfectRhyme,
  ChooseRhyme,
}
