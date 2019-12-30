import { ToTimeStringPipe } from './to-time-string.pipe';

describe('ToTimeStringPipe', () => {
  it('create an instance', () => {
    const pipe = new ToTimeStringPipe();
    expect(pipe).toBeTruthy();
  });
});
