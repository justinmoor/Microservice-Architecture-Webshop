import { EurcurrencyPipe } from './eurcurrency.pipe';
import { pipe } from 'rxjs';

describe('EurcurrencyPipe', () => {
  it('create an instance', () => {
    const pipe = new EurcurrencyPipe();
    expect(pipe).toBeTruthy();
  });

  let pipe : EurcurrencyPipe;
  beforeEach(() => {
    pipe = new EurcurrencyPipe();
  })

  it('formats 1.00 to 1.-', () => {
    expect(pipe.transform(1.00)).toBe("\u20AC 1,-")
  })

  it('formats 4.56 to € 4.56', () => {
    expect(pipe.transform(4.56)).toBe("\u20AC 4,56")
  })
  
  it('formats 4.565454 to € 4.56', () => {
    expect(pipe.transform(4.56)).toBe("\u20AC 4,56")
  })
});
