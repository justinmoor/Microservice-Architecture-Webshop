import { EurcurrencyPipe } from './eurcurrency.pipe';

describe('EurcurrencyPipe', () => {
  let pipe: EurcurrencyPipe;
  beforeEach(() => {
    pipe = new EurcurrencyPipe();
  });

  it('create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('formats 1.00 to 1.-', () => {
    expect(pipe.transform(1.00)).toBe('\u20AC 1,-');
  });

  it('formats 4.56 to € 4.56', () => {
    expect(pipe.transform(4.56)).toBe('\u20AC 4,56');
  });

  it('formats 4.565454 to € 4.56', () => {
    expect(pipe.transform(4.565454)).toBe('\u20AC 4,57');
  });
  it('formats 4.6 to € 4.60', () => {
    expect(pipe.transform(4.6)).toBe('\u20AC 4,60');
  });
});
